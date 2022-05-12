using DCubeHotelDomain.Models;
using DCubeHotelSystem.Models;
using DCubeHotelSystem.Models.Interface;
using DCubeHotelUser;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class LoginAPIController : BaseAPIController
    {
        private IDCubeRepository<HotelUser> hotelUserRepo = null;
        private IDCubeRepository<HotelRole> HotelRoleRepo = null;
        public IMembershipService MembershipService { get; set; }
        public UnitOfWork uof = new UnitOfWork();
        public LoginAPIController()
        {
            this.hotelUserRepo = new DCubeRepository<HotelUser>();
            this.HotelRoleRepo = new DCubeRepository<HotelRole>();
        }
        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext requestContext)
        {
            if (MembershipService == null)
            {
                MembershipService = new AccountMembershipService();
            }
            base.Initialize(requestContext);
        }

        public HttpResponseMessage Get()
        {

            return ToJson(hotelUserRepo.GetAllData());
        }

        [AllowAnonymous]
        [HttpPost]

        public HttpResponseMessage Login(ViewModel.LoginViewModel login)
        {

            int result = 0;
            try
            {
                var users = hotelUserRepo.GetAllData().Where(o => o.UserName.ToLower() == login.UserName.ToLower() && o.Password == login.Password).FirstOrDefault();
                var roles = HotelRoleRepo.GetAllData().ToList();

                if (users == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);

                if (users != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["as:AudienceSecret"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new System.Security.Claims.Claim[]
                        {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, users.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var RoleNames = (from userRole in users.Roles
                                     join role in roles on userRole.RoleId
                                     equals role.Id
                                     select new { RoleId = role.Id, userRole.UserId, RoleName = role.PermissionList }).ToList();
                    users.Roles.Clear();
                    foreach (var rolename in RoleNames)
                    {
                        users.Roles.Add(new IdentityUserRole { RoleId = rolename.RoleId, UserId = rolename.UserId });
                    }
                    HotelRole ObjHotelRole = new HotelRole();
                    ObjHotelRole = HotelRoleRepo.GetAllData().Where(o => o.Id == RoleNames.FirstOrDefault().RoleId).FirstOrDefault();

                    users.RoleName = ObjHotelRole.PermissionList;

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    users.Token = tokenHandler.WriteToken(token);

                    // remove password before returning
                    users.Password = null;

                    return Request.CreateResponse(HttpStatusCode.OK, users);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
