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
		/// <value>Line 1, can write anything, eg. admin's full name.</value>
		public string ContactLine1 { get; set; }
		/// <value>Line 2, can write anything, eg. admin's email address.</value>
		public string ContactLine2 { get; set; }
		/// <value>Line 3, can write anything, eg. admin's phone number.</value>
		public string ContactLine3 { get; set; }
    }
}
