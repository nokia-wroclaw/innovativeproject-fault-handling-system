using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fault_handling_system.Models
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	/// <summary>
	/// Default ApplicationUser class which derives from IdentityUser class.
	/// It's primary key is a string and provides such properties as
	/// UserName, Email, PasswordHash.
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Identity.IdentityUser"/>
	public class ApplicationUser : IdentityUser
    {
    }
}
