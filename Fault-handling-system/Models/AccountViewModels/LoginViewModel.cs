using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models.AccountViewModels
{
    /// <summary>
    /// Model of login view.
    /// </summary>
    public class LoginViewModel
    {
        ///<value>User email</value>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <value>User password</value>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
