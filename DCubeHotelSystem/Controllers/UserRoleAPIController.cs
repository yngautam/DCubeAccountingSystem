using DCubeHotelDomain.Models;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class UserRoleAPIController : BaseAPIController
    {
        //private static IContainer container;
        private IDCubeRepository<HotelUserRole> hoteluserRoleRepository = null;
        private IDCubeRepository<ExceptionLog> execptionlogRepository = null;
        public UserRoleAPIController()
        {
            this.hoteluserRoleRepository = new DCubeRepository<HotelUserRole>();
            this.execptionlogRepository = new DCubeRepository<ExceptionLog>();
         }
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (var uof = new UnitOfWork())
                uof.StartTransaction();
                return ToJson(hoteluserRoleRepository.GetAllData());
        }
        [HttpPost]
        public HttpResponseMessage Post(HotelUserRole value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                try
                {
                    uof.StartTransaction();
                    hoteluserRoleRepository.Insert(value);
                    hoteluserRoleRepository.Save();
                    result = 1;

                }
                catch (Exception ex)
                {
                    ExceptionLog logger = new ExceptionLog();
                    {
                        logger.ExceptionMessage = ex.Message;
                        logger.ExceptionStackTrace = ex.StackTrace;
                        logger.ControllerName = ex.Source.ToString();
                        logger.ErrorLogDate = DateTime.Now;
                    };

                    execptionlogRepository.Insert(logger);
                    execptionlogRepository.Save();
                    result = 0;
                    uof.RollBackTransaction();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }
                uof.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPut]
        public HttpResponseMessage Put(int id, HotelUserRole value)
        {
            int result = 0;
            using (var uof= new UnitOfWork())
            {
                if (id >= 1)
                {
                    uof.StartTransaction();
                    hoteluserRoleRepository.Update(value);
                    hoteluserRoleRepository.Save();
                    result = 1;
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }
            }
        }
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            hoteluserRoleRepository.Delete(id);
            hoteluserRoleRepository.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
} 
