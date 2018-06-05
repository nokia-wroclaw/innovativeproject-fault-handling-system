using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
	/// <summary>
	/// Class representing admin's contact data.
	/// The data is read from appsettings.json and used in Contact page.
	/// </summary>
	public class AdminContact
    {
		/// <value>Admin's full name.</value>
		public string FullName { get; set; }
		/// <value>Admin's email address.</value>
		public string Email { get; set; }
		/// <value>Admin's phone number.</value>
		public string PhoneNumber { get; set; }
    }
}
