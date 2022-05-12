using DCubeHotelDomain.Models;
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
    public class PermissionController : BaseAPIController
    {
        private IDCubeRepository<HotelPermission> PermissionRepository = null;

        public PermissionController()
        {
            this.PermissionRepository = new DCubeRepository<HotelPermission>();
        }
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (var uof = new UnitOfWork())
                uof.StartTransaction();
            return ToJson(PermissionRepository.GetAllData());
        }
        [HttpPost]
        public HttpResponseMessage Post(HotelPermission value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                try
                {
                    uof.StartTransaction();
                    PermissionRepository.Insert(value);
                    PermissionRepository.Save();
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
        public HttpResponseMessage Put(int id, HotelPermission value)
        {
            int result = 0;
            using (var uof = new UnitOfWork())
            {
                if (id >= 1)
                {
                    try
                    {
                        uof.StartTransaction();
                        PermissionRepository.Update(value);
                        PermissionRepository.Save();
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
