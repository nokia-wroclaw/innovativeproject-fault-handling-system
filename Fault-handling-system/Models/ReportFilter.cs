using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
	/// <summary>
	/// Model representing ReportFilter db table in code.
	/// Used to filter the reports so the user sees only the ones they need to see.
	/// </summary>
	public class ReportFilter
    {
		/// <value>Filter's id. Primary key.</value>
		[Key]
		public int Id { get; set; }
		/// <value>Id of the user who created the filter.</value>
		[Required]
		public string UserId { get; set; }
		/// <value>User's navigation property.</value>
		public virtual ApplicationUser User { get; set; }
		/// <value>Filter's name.</value>
		[Required]
		public string Name { get; set; }
		/// <value>Etr Number.</value>
		public string EtrNumber { get; set; }
		/// <value>Rfa Id.</value>
		public string RfaId { get; set; }
		/// <value>Rfa Name.</value>
		public string RfaName { get; set; }
		/// <value>Zone name.</value>
		public string Zone { get; set; }
		/// <value>Priority.</value>
		public string Priority { get; set; }
		/// <value>Etr Type value.</value>
		public string EtrType { get; set; }
		/// <value>Etr Status value.</value>
		public string EtrStatus { get; set; }
		/// <value>Grade.</value>
		public string Grade { get; set; }
		/// <value>Trouble Type.</value>
		public string TroubleType { get; set; }
		/// <value>Nsn Coordinator's name.</value>
		public string NsnCoordinatorId { get; set; }
		/// <value>Subcontractor's name.</value>
		public string SubcontractorId { get; set; }
		/// <value>Date Issued from value.</value>
		public string DateIssuedFrom { get; set; }
		/// <value>How many days ago Date Issued from value is searched.</value>
		[Range(1, 6)]
		public int? DateIssuedFromDaysAgo { get; set; }
		/// <value>How many weeks ago Date Issued from value is searched.</value>
		public int? DateIssuedFromWeeksAgo { get; set; }
		/// <value>Date Issued to value.</value>
		public string DateIssuedTo { get; set; }
		/// <value>How many days ago Date Issued to value is searched.</value>
		[Range(1, 6)]
		public int? DateIssuedToDaysAgo { get; set; }
		/// <value>How many weeks ago Date Issued to value is searched.</value>
		public int? DateIssuedToWeeksAgo { get; set; }
		/// <value>Date Sent from value.</value>
		public string DateSentFrom { get; set; }
		/// <value>How many days ago Date Sent from value is searched.</value>
		[Range(1, 6)]
		public int? DateSentFromDaysAgo { get; set; }
		/// <value>How many weeks ago Date Sent from value is searched.</value>
		public int? DateSentFromWeeksAgo { get; set; }
		/// <value>Date Sent to value.</value>
		public string DateSentTo { get; set; }
		/// <value>How many days ago Date Sent to value is searched.</value>
		[Range(1, 6)]
		public int? DateSentToDaysAgo { get; set; }
		/// <value>How many weeks ago Date Sent to value is searched.</value>
		public int? DateSentToWeeksAgo { get; set; }
	}
}
