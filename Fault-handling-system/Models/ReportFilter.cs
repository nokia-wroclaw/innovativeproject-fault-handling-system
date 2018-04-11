using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
    public class ReportFilter
    {
		public string UserId { get; set; }
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
		public string DateIssuedFrom
		{
			get
			{
				if (DateIssuedFromDaysAgo == 0 && DateIssuedFromWeeksAgo == 0)
					return DateIssuedFrom;
				DateTime returnDate = DateTime.Today;
				returnDate.AddDays(-(DateIssuedFromWeeksAgo * 7 + DateIssuedFromDaysAgo));
				string returnString = returnDate.ToString("yyyy-MM-dd");
				return returnString;
			}
			set => DateIssuedFrom = value;
		}
		[Range(1, 6)]
		public int DateIssuedFromDaysAgo { get; set; }
		public int DateIssuedFromWeeksAgo { get; set; }
		public string DateIssuedTo
		{
			get
			{
				if (DateIssuedToDaysAgo == 0 && DateIssuedToWeeksAgo == 0)
					return DateIssuedTo;
				DateTime returnDate = DateTime.Today;
				returnDate.AddDays(-(DateIssuedToWeeksAgo * 7 + DateIssuedToDaysAgo));
				string returnString = returnDate.ToString("yyyy-MM-dd");
				return returnString;
			}
			set => DateIssuedTo = value;
		}
		[Range(1, 6)]
		public int DateIssuedToDaysAgo { get; set; }
		public int DateIssuedToWeeksAgo { get; set; }
		public string DateSentFrom
		{
			get
			{
				if (DateSentFromDaysAgo == 0 && DateSentFromWeeksAgo == 0)
					return DateSentFrom;
				DateTime returnDate = DateTime.Today;
				returnDate.AddDays(-(DateSentFromWeeksAgo * 7 + DateSentFromDaysAgo));
				string returnString = returnDate.ToString("yyyy-MM-dd");
				return returnString;
			}
			set => DateSentFrom = value;
		}
		[Range(1, 6)]
		public int DateSentFromDaysAgo { get; set; }
		public int DateSentFromWeeksAgo { get; set; }
		public string DateSentTo
		{
			get
			{
				if (DateSentToDaysAgo == 0 && DateSentToWeeksAgo == 0)
					return DateSentTo;
				DateTime returnDate = DateTime.Today;
				returnDate.AddDays(-(DateSentToWeeksAgo * 7 + DateSentToDaysAgo));
				string returnString = returnDate.ToString("yyyy-MM-dd");
				return returnString;
			}
			set => DateSentTo = value;
		}
		[Range(1, 6)]
		public int DateSentToDaysAgo { get; set; }
		public int DateSentToWeeksAgo { get; set; }
	}
}
