using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Roles;
using DCubeHotelSystem.DataContext;
using DCubeHotelUser.Interfaces;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace DCubeHotelSystem.Models
{

    public class CustomMembershipProvider : MembershipProvider
    {

        private IUnitOfWork _unitOfWork;
        private HotelDbContext db = new HotelDbContext();
        private int _cacheTimeoutInMinutes = 30;


        public CustomMembershipProvider(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            //this.userRepository = new HotelUserRepository<HotelUser>();
        }

        //Initialize values from web.config
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            //Set Properties
            int val;
            if (!string.IsNullOrEmpty(config["cacheTimeoutInMinutes"]) && Int32.TryParse(config["cacheTimeoutInMinutes"], out val))
                _cacheTimeoutInMinutes = val;

            //Call base method
            base.Initialize(name, config);
        }
        //Verifies that the specified username and password exist in the data source
        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;
            List<HotelUser> users = new List<HotelUser>();
            var user = users.Where(o => o.UserName == username && o.Password == password);

            if (user == null)
                return false;
            else
                return true;
            //return user != null;
        }
        //Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var cacheKeyUser = string.Format("UserData_{0}", username);
            if (HttpRuntime.Cache[cacheKeyUser] != null)
                return (CustomUserMembership)HttpRuntime.Cache[cacheKeyUser];

            // var user = ;
            var user = _unitOfWork.UserRepository.FindByUserName(username);
            if (user == null)
                return null;

            var membershipUser = new CustomUserMembership(user);

            //Store in cache
            HttpRuntime.Cache.Insert(cacheKeyUser, membershipUser, null, DateTime.Now.AddMinutes(_cacheTimeoutInMinutes), Cache.NoSlidingExpiration);

            return membershipUser;
        }
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }
        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }
        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

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
