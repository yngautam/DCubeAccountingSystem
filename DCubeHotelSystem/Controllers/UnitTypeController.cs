using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class UnitTypeController : BaseAPIController
    {
        private IDCubeRepository<UnitType> UnitTypeRepo;

        public UnitTypeController()
        {
            this.UnitTypeRepo = new DCubeRepository<UnitType>();
        }

        [HttpGet]
        public HttpResponseMessage Get() => this.ToJson((object)this.UnitTypeRepo.GetAllData());

        [HttpGet]
        public HttpResponseMessage Get(int Id) => this.ToJson((object)this.UnitTypeRepo.GetAllData().Where<UnitType>((Func<UnitType, bool>)(o => o.Id == Id)));

        [HttpPost]
        public HttpResponseMessage Post(UnitType value)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    this.UnitTypeRepo.Insert(value);
                    this.UnitTypeRepo.Save();
                    num = 1;
                }
                catch (Exception ex)
                {
                    this.db.ExceptionLogs.Add(new ExceptionLog()
                    {
                        ExceptionMessage = ex.Message,
                        ExceptionStackTrace = ex.StackTrace,
                        ControllerName = ex.Source.ToString(),
                        ErrorLogDate = DateTime.Now
                    });
                    ((DbContext)this.db).SaveChanges();
                    num = 0;
                    unitOfWork.RollBackTransaction();
                    return Request.CreateResponse(HttpStatusCode.OK, num);
                }
                unitOfWork.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, UnitType value)
        {
            if (id < 1)
                return this.ToJson((object)this.UnitTypeRepo);
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    this.UnitTypeRepo.Update(value);
                    this.UnitTypeRepo.Save();
                    num = 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                unitOfWork.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    this.UnitTypeRepo.Delete((object)id);
                    this.UnitTypeRepo.Save();
                    num = 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                unitOfWork.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }

        [HttpPost]
        public HttpResponseMessage DeleteUnit(int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    this.UnitTypeRepo.Delete((object)id);
                    this.UnitTypeRepo.Save();
                    num = 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                unitOfWork.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }
    }
}
