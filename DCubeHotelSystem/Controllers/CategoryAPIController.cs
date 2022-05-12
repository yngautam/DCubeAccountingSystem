using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class CategoryAPIController : BaseAPIController
    {
        private IDCubeRepository<Category> categoryrepo = null;


        public CategoryAPIController()
        {
            this.categoryrepo = new DCubeRepository<Category>();
        }
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return ToJson(categoryrepo.GetAllData().OrderBy(o => o.Name));
        }

        [HttpPost]
        public HttpResponseMessage Post(Category value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                try
                {
                    uof.StartTransaction();
                    categoryrepo.Insert(value);
                    categoryrepo.Save();
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

                    db.ExceptionLogs.Add(logger);
                    db.SaveChanges();
                    result = 0;
                    uof.RollBackTransaction();
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                uof.CommitTransaction();
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPut]
        public HttpResponseMessage Put(int id, Category value)
        {
            if (id >= 1)
            {
                int result = 0;
                using (var uof = new UnitOfWork())
                {
                    try
                    {
                        uof.StartTransaction();
                        categoryrepo.Update(value);
                        categoryrepo.Save();
                        result = 1;
                    }
                    catch (Exception ex)
                    {

                    }
                    uof.CommitTransaction();
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return ToJson(categoryrepo);
            }
        }
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                try
                {
                    uof.StartTransaction();
                    categoryrepo.Delete(id);
                    categoryrepo.Save();
                    result = 1;
                }
                catch (Exception ex)
                {

                }
                uof.CommitTransaction();
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
