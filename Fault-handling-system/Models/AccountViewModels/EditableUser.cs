using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models.AccountViewModels
{
     /// <summary>
     /// EditableUser class inherits after IdentityUser.
     /// Containts additional properities to make user roles editable.
     /// </summary>
    public class EditableUser : IdentityUser
    {
        /// <value>A role to assign/unassign</value>
        [Required]
        public string Role { get; set; }
        /// <value>List of available user roles/value>
        public IList<string> Roles { get; set; }
        /// <value>Roles to chose from in a dropdown list</value>
        public IEnumerable<SelectListItem> ERoles { get; set; }
    }
}
