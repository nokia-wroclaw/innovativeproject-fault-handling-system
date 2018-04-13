using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
	/// <summary>
	/// Model representing EtrType db table in code.
	/// </summary>
	public class EtrType
    {
		/// <value>Primary key.</value>
		[Key]
		public int Id { get; set; }
		/// <value>String value of EtrType.</value>
		[Required]
		public string Type { get; set; }
    }
}
