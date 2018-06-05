using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
	/// <summary>
	/// Model representing Report db table in code.
	/// </summary>
	public class Report
    {
		/// <value>Primary key.</value>
		[Key]
		public int Id { get; set; }
		/// <value>Etr number. Required.</value>
		[Required]
		[DisplayName("Etr Number")]
		public string EtrNumber { get; set; } //eg. ETR-000089629 cut ETR-00... and make it an integer?
		/// <value>Nokia id for the case. Also known as PO number. Nullable.</value>
		[DisplayName("Nokia Case Id")]
		public long? NokiaCaseId { get; set; } //PO Number
		/// <value>Rfa id. Also known as CSC/Link ID. Required.</value>
		[DisplayName("Rfa Id")]
		public long RfaId { get; set; } //CSC/Link ID
		/// <value>Rfa name. Also known as CSC/Link Name. Nullable.</value>
		[DisplayName("Rfa Name")]
		public string RfaName { get; set; } //CSC/Link Name
		/// <value>Zone foreign key. Nullable.</value>
		[DisplayName("Zone")]
		public int ZoneId { get; set; }
		/// <value>Navigation property for Zone provided by its id.</value>
		public virtual Zone Zone { get; set; }
		/// <value>Assigned to. Nullable.</value>
		[DisplayName("Assigned To")]
		public string AssignedTo { get; set; }
		/// <value>Case's priority. Nullable.</value>
		public string Priority { get; set; }
		/// <value>EtrType foreign key. Nullable.</value>
		[Required]
		[DisplayName("Etr Type")]
		public int EtrTypeId { get; set; }
		/// <value>Navigation property for EtrType provided by its id.</value>
		[DisplayName("Etr Type")]
		public virtual EtrType EtrType { get; set; }
		/// <value>EtrStatus foreign key. Nullable.</value>
		[Required]
		[DisplayName("Etr Status")]
		public int EtrStatusId { get; set; }
		/// <value>Navigation property for EtrStatus provided by its id.</value>
		[DisplayName("Etr Status")]
		public virtual EtrStatus EtrStatus { get; set; }
		/// <value>ApplicationUser foreign key for a requestor. Required.</value>
		[Required]
		[DisplayName("Requestor")]
		public string RequestorId { get; set; } //ApplicationUser id's type is nvarchar
		/// <value>Navigation property for a requestor provided by their id.</value>
		[DisplayName("Requestor")]
		public virtual ApplicationUser Requestor { get; set; }
		/// <value>ApplicationUser foreign key for a Nokia coordinator. Nullable.</value>
		[DisplayName("Nsn Coordinator")]
		public string NsnCoordinatorId { get; set; }
		/// <value>Navigation property for a Nokia coordinator provided by their id.</value>
		[DisplayName("Nsn Coordinator")]
		public virtual ApplicationUser NsnCoordinator { get; set; }
		/// <value>ApplicationUser foreign key for a subcontractor. Nullable.</value>
		[DisplayName("Subcontractor")]
		public string SubcontractorId { get; set; }
		/// <value>Navigation property for a subcontractor.</value>
		[DisplayName("Subcontractor")]
		public virtual ApplicationUser Subcontractor { get; set; }
		/// <value>Case's grade. Accepts values from 1 to 5. Nullable.</value>
		[Range(1, 5)]
		public int? Grade { get; set; }
		/// <value>Type of the trouble. Nullable.</value>
		[DisplayName("Trouble Type")]
		public string TroubleType { get; set; }
		/// <value>Date when the problem occured. Required.</value>
		[DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
		[DisplayName("Date Issued")]
		public DateTime DateIssued { get; set; }
		/// <value>Date when the report was sent. Nullable.</value>
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		[DisplayName("Date Sent")]
		public DateTime? DateSent { get; set; }
		/// <value>Etr to Des date. Nullable.</value>
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		[DisplayName("Etr To Des")]
		public DateTime? EtrToDes { get; set; }
		/// <value>Case's closing date. Nullable.</value>
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		[DisplayName("Closing Date")]
		public DateTime? ClosingDate { get; set; }
		/// <value>Description for the Etr. Required.</value>
		[Required]
		[DisplayName("Description")]
		public string EtrDescription { get; set; }
		/// <value>Additional comment for the Etr. Nullable.</value>
		[DisplayFormat(NullDisplayText = "There's no comment for this report.")]
		public string Comment { get; set; }
    }
}
