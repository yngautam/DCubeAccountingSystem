using DCubeHotelDomain.Models;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class UserAccountAPIController : BaseAPIController
    {
        private IDCubeRepository<HotelUser> HotelUserRepository;
        private IDCubeRepository<HotelUserRole> HotelUserRoleRepo;
        private IDCubeRepository<IdentityUserRole> IdentityUserRoleRepo;

        public UserAccountAPIController()
        {
            this.HotelUserRepository = (IDCubeRepository<HotelUser>)new DCubeRepository<HotelUser>();
            this.HotelUserRoleRepo = (IDCubeRepository<HotelUserRole>)new DCubeRepository<HotelUserRole>();
            this.IdentityUserRoleRepo = (IDCubeRepository<IdentityUserRole>)new DCubeRepository<IdentityUserRole>();
        }

        [HttpGet]
        [Route("api/UserAccountAPI/GetUsers")]
        public HttpResponseMessage GetUsers()
        {
            List<HotelUser> hotelUserList = new List<HotelUser>();
            return this.ToJson((object)this.HotelUserRepository.GetAllData().ToList<HotelUser>());
        }

        [HttpGet]
        [Route("api/UserAccountAPI/GetUser")]
        public HttpResponseMessage GetUser([FromUri] string Id)
        {
            HotelUser hotelUser = new HotelUser();
            return this.ToJson((object)this.HotelUserRepository.GetAllData().FirstOrDefault<HotelUser>((Func<HotelUser, bool>)(o => ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)o).Id.ToString() == Id)));
        }

        [HttpGet]
        [Route("api/UserAccountAPI/GetUserByName")]
        public HttpResponseMessage GetUserByName([FromUri] string username)
        {
            HotelUser hotelUser = new HotelUser();
            return this.ToJson((object)this.HotelUserRepository.GetAllData().FirstOrDefault<HotelUser>((Func<HotelUser, bool>)(o => ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)o).UserName == username)));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/UserAccountAPI/CreateUser")]
        public HttpResponseMessage CreateUser(CreateUserBindingModel createUserModel)
        {
            HotelUser hotelUser1 = new HotelUser();
            ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)hotelUser1).UserName = createUserModel.Username;
            ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)hotelUser1).Email = createUserModel.Email;
            hotelUser1.FirstName = createUserModel.FirstName;
            hotelUser1.LastName = createUserModel.LastName;
            hotelUser1.Level = createUserModel.UserLevel;
            hotelUser1.JoinDate = DateTime.Now.Date;
            hotelUser1.IsActive = createUserModel.IsActive;
            ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)hotelUser1).PhoneNumber = createUserModel.PhoneNumber;
            hotelUser1.ResetPassword = createUserModel.ResetPassword;
            hotelUser1.Password = createUserModel.Password;
            hotelUser1.RoleName = createUserModel.RoleName;
            hotelUser1.FullName = createUserModel.FirstName + createUserModel.LastName;
            HotelUser hotelUser2 = hotelUser1;
            this.HotelUserRepository.Insert(hotelUser2);
            this.HotelUserRepository.Save();
            return Request.CreateResponse(HttpStatusCode.OK, hotelUser2);
        }

        [HttpDelete]
        [Route("api/UserAccountAPI/DeleteUser")]
        public HttpResponseMessage DeleteUser([FromUri] string Id)
        {
            int num = 1;
            HotelUser hotelUser1 = new HotelUser();
            HotelUser hotelUser2 = this.HotelUserRepository.GetAllData().FirstOrDefault<HotelUser>((Func<HotelUser, bool>)(o => ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)o).Id.ToString() == Id));
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    this.HotelUserRepository.Delete((object)((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)hotelUser2).Id);
                    this.HotelUserRepository.Save();
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
        [Route("api/UserAccountAPI/EditUser")]
        public HttpResponseMessage EditUser(string id, HotelUser value)
        {
            int num = 1;
            HotelUser hotelUser1 = new HotelUser();
            HotelUser hotelUser2 = this.HotelUserRepository.GetAllData().FirstOrDefault<HotelUser>((Func<HotelUser, bool>)(o => ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)o).Id.ToString() == id));
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    if (hotelUser2 != null)
                    {
                        ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)hotelUser2).Email = ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)value).Email;
                        ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)hotelUser2).EmailConfirmed = ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)value).EmailConfirmed;
                        hotelUser2.FirstName = value.FirstName;
                        hotelUser2.FullName = value.FullName;
                        hotelUser2.IsActive = value.IsActive;
                        hotelUser2.JoinDate = value.JoinDate;
                        hotelUser2.LastName = value.LastName;
                        hotelUser2.Level = value.Level;
                        ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)hotelUser2).PhoneNumber = ((IdentityUser<string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>)value).PhoneNumber;
                        hotelUser2.ResetPassword = value.ResetPassword;
                        hotelUser2.Password = value.Password;
                        this.HotelUserRepository.Update(hotelUser2);
                        this.HotelUserRepository.Save();
                    }
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
