//using Autofac;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Roles;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class RoleAPIController : BaseAPIController
    {    
        private IDCubeRepository<HotelRole> roleRepository = null;

        public RoleAPIController()
        {
            this.roleRepository = new DCubeRepository<HotelRole>();
        }
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (var uof = new UnitOfWork())
                uof.StartTransaction();
            return ToJson(roleRepository.GetAllData());
        }
        [HttpPost]
        public HttpResponseMessage Post(HotelRole value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                try
                {
                    uof.StartTransaction();
                    roleRepository.Insert(value);
                    roleRepository.Save();
                    result = 1;
                }
                catch (Exception ex)
                {
                    result = 0;
                    uof.RollBackTransaction();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }
                uof.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPut]
        public HttpResponseMessage Put(int id, HotelRole value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                if (id >= 1)
                {
                    try
                    {
                        uof.StartTransaction();
                        roleRepository.Update(value);
                        roleRepository.Save();
                        result = 1;
                    }
                    catch(Exception ex)
                    {
                        result = 0;
                        uof.RollBackTransaction();
                        return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                    
                }
            
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }
                uof.CommitTransaction();

            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                roleRepository.Delete(id);
                roleRepository.Save();
                return ToJson(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
