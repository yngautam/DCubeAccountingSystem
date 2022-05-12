using DCubeHotelDomain.Models;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class UserRoleAssignController : BaseAPIController
    {
        private IDCubeRepository<HotelUser> HotelUserRepository;
        private IDCubeRepository<HotelUserRole> HotelUserRoleRepo;
        private IDCubeRepository<IdentityUserRole> IdentityUserRoleRepo;

        public UserRoleAssignController()
        {
            this.HotelUserRepository = (IDCubeRepository<HotelUser>)new DCubeRepository<HotelUser>();
            this.HotelUserRoleRepo = (IDCubeRepository<HotelUserRole>)new DCubeRepository<HotelUserRole>();
            this.IdentityUserRoleRepo = (IDCubeRepository<IdentityUserRole>)new DCubeRepository<IdentityUserRole>();
        }

        [HttpPost]
        public HttpResponseMessage PostUserRole([FromUri] string UserId, [FromUri] string RoleId)
        {
            int num = 1;
            try
            {
                IdentityUserRole identityUserRole1 = new IdentityUserRole();
                IdentityUserRole id = this.IdentityUserRoleRepo.GetAllData().FirstOrDefault<IdentityUserRole>((Func<IdentityUserRole, bool>)(o => ((IdentityUserRole<string>)o).UserId.ToString() == UserId));
                if (id != null)
                {
                    this.IdentityUserRoleRepo.Delete((object)id);
                    this.IdentityUserRoleRepo.Save();
                }
                IdentityUserRole identityUserRole2 = new IdentityUserRole();
                ((IdentityUserRole<string>)identityUserRole2).RoleId = RoleId;
                ((IdentityUserRole<string>)identityUserRole2).UserId = UserId;
                this.IdentityUserRoleRepo.Insert(identityUserRole2);
                this.IdentityUserRoleRepo.Save();
                HotelUser hotelUser1 = new HotelUser();
                HotelUser hotelUser2 = this.HotelUserRepository.GetAllData().FirstOrDefault<HotelUser>((Func<HotelUser, bool>)(o => ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)o).Id.ToString() == UserId));
                hotelUser2.RoleName = RoleId;
                this.HotelUserRepository.Update(hotelUser2);
                this.HotelUserRepository.Save();
            }
            catch
            {
                num = 1;
            }
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }
    }
}
