using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models.AccountViewModels
{
    [Authorize(Roles = "Admin")]
    public class ManageUsersViewModel
    {
        [Display(Name = "Lista użytkowników")]
        public List<ApplicationUser> Users { get; set; }

        public IList<string>[] Roles { get; set; }
    
    }
}
