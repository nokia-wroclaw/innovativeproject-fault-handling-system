using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
	/// <summary>
	/// Model representing EtrStatus db table in code.
	/// </summary>
	public class EtrStatus
    {
		/// <value>Primary key.</value>
		[Key]
		public int Id { get; set; }
		/// <value>Text value of EtrStatus.</value>
		[Required]
		public string Status { get; set; }
    }
}
