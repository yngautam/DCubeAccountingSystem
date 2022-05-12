using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DCubeHotelSystem.Models
{
    public class CustomUserMembership:MembershipUser
    {
        public string Fullname { get; set; }

        public int UserRoleId { get; set; }

        public string UserRoleName { get; set; }
        public CustomUserMembership(HotelUser user)
            : base("CustomMembershipProvider",user.UserName, user.Id, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Fullname = user.FullName;
        }

    }
}