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
    public class UserPermissionAPIController : BaseAPIController
    {
        private IDCubeRepository<HotelUserPermission> HotelUserPermissionRepository = null;

        public UserPermissionAPIController()
        {
            this.HotelUserPermissionRepository = new DCubeRepository<HotelUserPermission>();
        }
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (var uof = new UnitOfWork())
                uof.StartTransaction();
            return ToJson(HotelUserPermissionRepository.GetAllData());
        }
        [HttpPost]
        public HttpResponseMessage Post(HotelUserPermission value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                try
                {
                    uof.StartTransaction();
                    HotelUserPermissionRepository.Insert(value);
                    HotelUserPermissionRepository.Save();
                    result = 1;
                }
                catch (Exception ex)
                {
                    uof.RollBackTransaction();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }
                uof.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPut]
        public HttpResponseMessage Put(int id, HotelUserPermission value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                if (id >= 1)
                {
                    try
                    {
                        uof.StartTransaction();
                        HotelUserPermissionRepository.Update(value);
                        HotelUserPermissionRepository.Save();
                        result = 1;
                    }
                    catch (Exception ex)
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
    }
}
