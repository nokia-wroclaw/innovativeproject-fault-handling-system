using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
	/// <summary>
	/// Model representing Zone db table in code.
	/// </summary>
	public class Zone
    {
		/// <value>Primary key.</value>
		[Key]
		public int Id { get; set; }
		/// <value>Zone's name.</value>
		[Required]
		public string ZoneName { get; set; }
    }
}
