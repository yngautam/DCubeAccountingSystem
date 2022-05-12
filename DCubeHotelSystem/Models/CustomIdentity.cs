using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace DCubeHotelSystem.Models
{
    public class CustomIdentity:IIdentity

    {
        public IIdentity Identity { get; set; }
        public int UserId { get; set; }

        public string FullName { get; set; }

      
        public string Email { get; set; }

        public int UserRoleId { get; set; }

        public string UserRoleName { get; set; }

        //Get the name of the Current User
        public string Name
        {
            get { return Identity.Name; }
        }
        //Get the type of authentication used.
        public string AuthenticationType
        {
            get { return Identity.AuthenticationType; }
        }
        //Gets a value that indicates whether the user has been authenticated.
        public bool IsAuthenticated
        {
            get
            {
                return Identity.IsAuthenticated;
            }
        }

        public CustomIdentity(IIdentity identity)
        {
            Identity = identity;

            var customMembershipUser = (CustomUserMembership)Membership.GetUser(identity.Name);
            if (customMembershipUser != null)
            {
                FullName = customMembershipUser.Fullname;
                Email = customMembershipUser.Email;
                UserRoleId = customMembershipUser.UserRoleId;
                UserRoleName = customMembershipUser.UserRoleName;
            }
        }
    }
}