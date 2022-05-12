using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DCubeHotelSystem.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter your user name")]
        [StringLength(maximumLength: 100, MinimumLength = 8, ErrorMessage = "Enter 8 charachter user name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [StringLength(maximumLength: 100, MinimumLength = 8, ErrorMessage = "Enter 8 charachter password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool Remember { get; set; }
    }
}
