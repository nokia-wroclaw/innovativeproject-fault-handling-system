using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models.AccountViewModels
{
    public class EditableUser : IdentityUser
    {
        [Required]
        public string Role { get; set; }
        public IList<string> Roles { get; set; }
        public IEnumerable<SelectListItem> ERoles { get; set; }
    }
}
