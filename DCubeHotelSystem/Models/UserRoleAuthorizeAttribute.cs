using DCubeHotelSystem.DataContext;
using DCubeHotelUser;
using DCubeHotelUser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DCubeHotelSystem.Models
{
    public class UserRoleAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
       
         private HotelDbContext db = new HotelDbContext();



        public UserRoleAuthorizeAttribute()
        {
            
        }
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            string CurrentUser = HttpContext.Current.User.Identity.Name;

            var userRoles = new string[] { };

            int index = Roles.IndexOf(",");

            string[] userrole = null;

            if (index > 0)
            {
                userrole = Roles.Split(',');
            }

            //userRoles = DCubeHotelUser.Roles.SelectRolesForUser(CurrentUser, db);

            //bool bUserRole = false;
            //for (int i = 0; i < userRoles.Length; i++)
            //{
            //    string ur = userRoles[i].ToString();
            //    bUserRole = userRoles.Contains(ur);
            //    if (bUserRole == true)
            //    {
            //        break;
            //    }
            //}
            if (CurrentUser == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", Action = "Login" }));
           
            }
            else
            {
                //if (bUserRole == false)
                //{
                //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", Action = "AccessDenied" }));
                //}
            }
        }
    }
}