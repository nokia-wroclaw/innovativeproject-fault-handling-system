using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
	public class Report
    {
		[Key]
		public int Id { get; set; }
		public String EtrNumber { get; set; } //eg. ETR-000089629 cut ETR-00... and make it an integer?
		public int? NokiaCaseId { get; set; } //PO Number?
		public int RfaId { get; set; } //CSC/Link ID?
		public string RfaName { get; set; } //CSC/Link Name?
		public int ZoneId { get; set; }
		public virtual Zone Zone { get; set; }
		public string AssignedTo { get; set; } //change to enum or make a model for it?
		public string Priority { get; set; }
		//public int EtrTypeId { get; set; }
		public EtrType EtrType { get; set; }
		//public int EtrStatusId { get; set; }
		public EtrStatus EtrStatus { get; set; }
		[Required]
		public string RequestorId { get; set; } //ApplicationUser id's type is nvarchar
		public virtual ApplicationUser Requestor { get; set; }
		public string NsnCoordinatorId { get; set; }
		public virtual ApplicationUser NsnCoordinator { get; set; }
		public string SubcontractorId { get; set; }
		public virtual ApplicationUser Subcontractor { get; set; }
		public int Grade { get; set; }
		//public int TroubleTypeId { get; set; }
		public TroubleType TroubleType { get; set; }
		public DateTime DateIssued { get; set; }
		public DateTime? DateSent { get; set; }
		public DateTime? EtrToDes { get; set; }
		public DateTime? ClosingDate { get; set; }
		public string EtrDescription { get; set; }
		[DisplayFormat(NullDisplayText = "There's no comment for this report.")]
		public string Comment { get; set; }
    }
}
