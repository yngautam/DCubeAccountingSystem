using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class InventoryItemAPIController : BaseAPIController
    {
        private IDCubeRepository<InventoryItem> inventItemRepo = null;
          
        public InventoryItemAPIController()
        {
            this.inventItemRepo = new DCubeRepository<InventoryItem>();
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            return ToJson(inventItemRepo.GetAllData());
        }

        [HttpPost]
        public HttpResponseMessage Post(InventoryItem value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                try
                {
                    uof.StartTransaction();
                    inventItemRepo.Insert(value);
                    inventItemRepo.Save();
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
                    return Request.CreateResponse(HttpStatusCode.OK,result);

                }
                uof.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK,result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, InventoryItem value)
        {
            //db.Entry(User).State = EntityState.Modified;
            //return ToJson(db.SaveChanges());
            if(id>=1)
            {
                int result = 0;
                using (var uof = new UnitOfWork())
                {
                    try
                    {
                        uof.StartTransaction();
                        inventItemRepo.Update(value);
                        inventItemRepo.Save();
                        result = 1;
                    }
                    catch(Exception ex)
                    {
                        throw (ex);
                    }
                    uof.CommitTransaction();
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return ToJson(inventItemRepo);
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
                    inventItemRepo.Delete(id);
                    inventItemRepo.Save();
                    result = 1;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                uof.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
