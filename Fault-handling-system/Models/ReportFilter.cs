using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
    public class ReportFilter
    {
		public string UserId { get; set; }
		public string EtrNumber { get; set; }
		public string RfaId { get; set; }
		public string RfaName { get; set; }
		public string Zone { get; set; }
		public string Priority { get; set; }
		public string EtrType { get; set; }
		public string EtrStatus { get; set; }
		public string Grade { get; set; }
		public string TroubleType { get; set; }
		public string NsnCoordinatorId { get; set; }
		public string SubcontractorId { get; set; }
		public string DateIssuedFrom { get; set; }
		public string DateIssuedTo { get; set; }
		public string DateSentFrom { get; set; }
		public string DateSentTo { get; set; }
	}
}
