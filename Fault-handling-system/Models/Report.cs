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
		[Required]
		public string EtrNumber { get; set; } //eg. ETR-000089629 cut ETR-00... and make it an integer?
		public int? NokiaCaseId { get; set; } //PO Number
		public int RfaId { get; set; } //CSC/Link ID
		public string RfaName { get; set; } //CSC/Link Name
		public int ZoneId { get; set; }
		public virtual Zone Zone { get; set; }
		public string AssignedTo { get; set; } //change to enum or make a model for it?
		public string Priority { get; set; }
		[Required]
		public int EtrTypeId { get; set; }
		public virtual EtrType EtrType { get; set; }
		[Required]
		public int EtrStatusId { get; set; }
		public virtual EtrStatus EtrStatus { get; set; }
		[Required]
		public string RequestorId { get; set; } //ApplicationUser id's type is nvarchar
		public virtual ApplicationUser Requestor { get; set; }
		public string NsnCoordinatorId { get; set; }
		public virtual ApplicationUser NsnCoordinator { get; set; }
		public string SubcontractorId { get; set; }
		public virtual ApplicationUser Subcontractor { get; set; }
		[Range(1, 5)]
		public int? Grade { get; set; }
		public string TroubleType { get; set; }
		public DateTime DateIssued { get; set; }
		public DateTime? DateSent { get; set; }
		public DateTime? EtrToDes { get; set; }
		public DateTime? ClosingDate { get; set; }
		[Required]
		public string EtrDescription { get; set; }
		[DisplayFormat(NullDisplayText = "There's no comment for this report.")]
		public string Comment { get; set; }
    }
}
