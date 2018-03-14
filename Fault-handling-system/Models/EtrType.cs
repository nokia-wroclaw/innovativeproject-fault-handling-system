using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
    public class EtrType
    {
		[Key]
		//public int Id { get; set; }
		public string Type { get; set; }
    }
}
