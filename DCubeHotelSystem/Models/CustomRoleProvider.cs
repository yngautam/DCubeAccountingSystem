using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Roles;
using DCubeHotelSystem.DataContext;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace DCubeHotelSystem.Models
{
    public class CustomRoleProvider: RoleProvider
    {
        private HotelDbContext db = new HotelDbContext();
        private IDCubeRepository<HotelUser> usersRepository = null;
        private IDCubeRepository<HotelUserRole> usersRoleRepository = null;
        private IDCubeRepository<HotelRole> rolesRepository = null;
       
           
        private int _cacheTimeoutInMinutes = 30;
        public CustomRoleProvider()
        {
            this.usersRepository = new DCubeRepository<HotelUser>();
            this.usersRoleRepository = new DCubeRepository<HotelUserRole>();
            this.rolesRepository = new DCubeRepository<HotelRole>();

        }
        //Initialize values from web.config

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            //Set Properties
            int val;
            if (!string.IsNullOrEmpty(config["cacheTimeoutInMinutes"]) && Int32.TryParse(config["cacheTimeoutInMinutes"], out val))
            {
                _cacheTimeoutInMinutes = val;
            }
            //Call base method 
            base.Initialize(name, config);
        }
        //Get a value indicating whether the specified user is in the specified role for the configured applicationname.
        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }
        //Gets a list of the roles that a specified user is in for the configured applicationName
        public override string[] GetRolesForUser(string username)
        {
            //Return if the user is not authenticated
            //if (!HttpContext.Current.User.Identity.IsAuthenticated)
            //    return null;

            //Return if present in Cache
            var cacheKeyRole = string.Format("UserRoles_{0}", username);
            if (HttpRuntime.Cache[cacheKeyRole] != null)
                return (string[])HttpRuntime.Cache[cacheKeyRole];

            //Get the roles from Database.

            //List<HotelUser> userlist = new List<HotelUser>();
            var userList = usersRepository.GetAllData();
            //List<HotelUserRole> userrolelist = new List<HotelUserRole>();
             var userRolelist = usersRoleRepository.GetAllData();
            //List<SchoolRole> rolelist = new List<SchoolRole>();
            var roleList = rolesRepository.GetAllData();

            var userRole = (from u in userList.Where(e => e.UserName.Contains(username))
                            join ur in userRolelist on u.Id equals ur.UserId.ToString()
                            join r in roleList on ur.RoleId.ToString() equals r.Id
                            select r.RoleName).ToArray();

            //Store in cache
            HttpRuntime.Cache.Insert(cacheKeyRole, userRole, null, DateTime.Now.AddMinutes(_cacheTimeoutInMinutes), Cache.NoSlidingExpiration);

            return userRole.ToArray();
        }
        //Add a new role to the data source for the configured applicationName
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
        //Removes a role from the data source for the configured applicationName
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }
        //Gets a value  indicating whether the specified role name already exists in the role data source for the configured applicationName
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        //Adds the specified user names to the specified roles for the configured application name
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        //Removes the specified user names from the specified roles for the configured applicationName
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        //Gets a list of users in the specified role for the configured applicationName
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        //Gets a list of all the roles for the configured applicationName
        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
        //Gets an array of user names in a role where the user name contains the specified user name to match.
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
        //Gets or sets the name of the application to store and retrive role information for.
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}