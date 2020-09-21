using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIdentity.ViewModel
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailExists", controller: "Account")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password doesn't match")]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]       
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
