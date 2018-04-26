using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models.AccountViewModels
{
    /// <summary>
    /// Model of RegisterView.
    /// Contains all necessairy variables for registering a new user.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class RegisterViewModel
    {
        /// <value>Login</value>
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        /// <value>Email address</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        /*
        /// <value>Password</value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <value>Password confirmation</value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        */

        /// <value>Initial role</value>
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }

    }
}
