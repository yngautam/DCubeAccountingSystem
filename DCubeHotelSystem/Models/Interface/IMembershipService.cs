using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCubeHotelSystem.Models.Interface
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}