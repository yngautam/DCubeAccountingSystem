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
    public class RolePermissionAPIController : BaseAPIController
    {
        private IDCubeRepository<HotelRolePermission> HotelRolePermissionRepository = null;

        public RolePermissionAPIController()
        {
            this.HotelRolePermissionRepository = new DCubeRepository<HotelRolePermission>();
        }
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (var uof = new UnitOfWork())
                uof.StartTransaction();
            return ToJson(HotelRolePermissionRepository.GetAllData());
        }
        [HttpPost]
        public HttpResponseMessage Post(HotelRolePermission value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                try
                {
                    uof.StartTransaction();
                    HotelRolePermissionRepository.Insert(value);
                    HotelRolePermissionRepository.Save();
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
        public HttpResponseMessage Put(int id, HotelRolePermission value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                if (id >= 1)
                {
                    try
                    {
                        uof.StartTransaction();
                        HotelRolePermissionRepository.Update(value);
                        HotelRolePermissionRepository.Save();
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