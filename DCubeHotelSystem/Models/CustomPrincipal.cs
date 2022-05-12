using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace DCubeHotelSystem.Models
{
    public class CustomPrincipal:IPrincipal
    {
        public bool IsInRole (string role)
        {
            return Identity is CustomIdentity && string.Compare(role, ((CustomIdentity)Identity).UserRoleName, StringComparison.CurrentCultureIgnoreCase) == 0;
        }
        //Gets the identity of the current principal
        public IIdentity Identity { get; private set; }
        public CustomPrincipal(CustomIdentity identity)
        {
            Identity = identity;
        }
    }
}