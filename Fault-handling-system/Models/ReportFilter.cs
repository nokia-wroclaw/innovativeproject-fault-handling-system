using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
    public class ReportFilter
    {
		[Key]
		public int Id { get; set; }
		[Required]
		public string UserId { get; set; }
		public virtual ApplicationUser User { get; set; }
		public string Name { get; set; }
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
		[Range(1, 6)]
		public int? DateIssuedFromDaysAgo { get; set; }
		public int? DateIssuedFromWeeksAgo { get; set; }
		public string DateIssuedTo { get; set; }
		[Range(1, 6)]
		public int? DateIssuedToDaysAgo { get; set; }
		public int? DateIssuedToWeeksAgo { get; set; }
		public string DateSentFrom { get; set; }
		[Range(1, 6)]
		public int? DateSentFromDaysAgo { get; set; }
		public int? DateSentFromWeeksAgo { get; set; }
		public string DateSentTo { get; set; }
		[Range(1, 6)]
		public int? DateSentToDaysAgo { get; set; }
		public int? DateSentToWeeksAgo { get; set; }
	}
}
