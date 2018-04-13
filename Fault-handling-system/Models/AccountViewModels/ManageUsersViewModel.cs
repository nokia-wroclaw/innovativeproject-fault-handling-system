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
    /// ManageUsersViewModel class cotains informations necessary for ManageUsersView.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ManageUsersViewModel
    {
        /// <value>List of all users</value>
        [Display(Name = "Lista użytkowników")]
        public List<ApplicationUser> Users { get; set; }

        public IList<string>[] Roles { get; set; }

        public string Role { get; set; }
        public IEnumerable<SelectListItem> ERoles { get; set; }
    }
}
