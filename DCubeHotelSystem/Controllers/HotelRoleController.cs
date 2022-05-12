using DCubeHotelDomain.Models;
using DCubeHotelUser;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    public class HotelRoleController : BaseAPIController
    {
        private IDCubeRepository<HotelRole> HotelRoleRepository;

        public HotelRoleController()
        {
            this.HotelRoleRepository = (IDCubeRepository<HotelRole>)new DCubeRepository<HotelRole>();
        }


        [HttpGet]
        [Route("api/HotelRole/GetRoles")]
        public HttpResponseMessage GetRoles() => this.ToJson((object)this.HotelRoleRepository.GetAllData());

        [HttpGet]
        [Route("api/HotelRole/GetRole")]
        public HttpResponseMessage GetRole(string Id) => this.ToJson((object)this.HotelRoleRepository.GetAllData().Where<HotelRole>((Func<HotelRole, bool>)(o => ((IdentityRole<string, IdentityUserRole>)o).Id == Id)));

        [HttpPost]
        [Route("api/HotelRole/PostRole")]
        public HttpResponseMessage Post(HotelRole value)
        {
            int num = 1;
            HotelRole hotelRole = new HotelRole();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    this.HotelRoleRepository.Insert(value);
                    this.HotelRoleRepository.Save();
                }
                catch (Exception ex)
                {
                    num = 0;
                }
                unitOfWork.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }

        [HttpPut]
        [Route("api/HotelRole/EditRole")]
        public HttpResponseMessage Put(string id, HotelRole value)
        {
            int num = 1;
            HotelRole hotelRole1 = new HotelRole();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    HotelRole hotelRole2 = this.HotelRoleRepository.GetAllData().FirstOrDefault<HotelRole>((Func<HotelRole, bool>)(o => ((IdentityRole<string, IdentityUserRole>)o).Id.ToString() == id));
                    hotelRole2.CreatedBy = value.CreatedBy;
                    hotelRole2.CreatedOn = value.CreatedOn;
                    hotelRole2.Description = value.Description;
                    hotelRole2.IsAdd = value.IsAdd;
                    hotelRole2.IsDelete = value.IsDelete;
                    hotelRole2.IsEdit = value.IsEdit;
                    hotelRole2.IsSysAdmin = value.IsSysAdmin;
                    hotelRole2.IsView = value.IsView;
                    hotelRole2.LastChangedBy = value.LastChangedBy;
                    hotelRole2.LastChangedDate = value.LastChangedDate;
                    ((IdentityRole<string, IdentityUserRole>)hotelRole2).Name = ((IdentityRole<string, IdentityUserRole>)value).Name;
                    hotelRole2.RoleName = value.RoleName;
                    hotelRole2.Selected = value.Selected;
                    hotelRole2.PermissionList = value.PermissionList;
                    unitOfWork.StartTransaction();
                    this.HotelRoleRepository.Update(hotelRole2);
                    this.HotelRoleRepository.Save();
                }
                catch (Exception ex)
                {
                    num = 0;
                }
                unitOfWork.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }

        [HttpDelete]
        [Route("api/HotelRole/DeleteRole")]
        public HttpResponseMessage Delete(string id)
        {
            int num = 1;
            HotelRole hotelRole1 = new HotelRole();
            HotelRole hotelRole2 = this.HotelRoleRepository.GetAllData().FirstOrDefault<HotelRole>((Func<HotelRole, bool>)(o => ((IdentityRole<string, IdentityUserRole>)o).Id.ToString() == id));
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    this.HotelRoleRepository.Delete((object)((IdentityRole<string, IdentityUserRole>)hotelRole2).Id);
                    this.HotelRoleRepository.Save();
                }
                catch (Exception ex)
                {
                    num = 0;
                }
                unitOfWork.CommitTransaction();
            }
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }
    }
}