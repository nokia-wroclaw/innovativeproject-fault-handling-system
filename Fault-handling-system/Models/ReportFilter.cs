using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
    public class ReportFilter
    {
		[Required]
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
				if (DateIssuedFromDaysAgo == null && DateIssuedFromWeeksAgo == null)
					return DateIssuedFrom;
				int days, weeks;
				if (DateIssuedFromDaysAgo == null && DateIssuedFromWeeksAgo != null)
				{
					weeks = (int)DateIssuedFromWeeksAgo;
					days = 0;
				}
				else if (DateIssuedFromDaysAgo != null && DateIssuedFromWeeksAgo == null)
				{
					days = (int)DateIssuedFromDaysAgo;
					weeks = 0;
				}
				else
				{
					days = (int)DateIssuedFromDaysAgo;
					weeks = (int)DateIssuedFromWeeksAgo;
				}

				DateTime returnDate = DateTime.Today;
				returnDate.AddDays(-(weeks * 7 + days));
				string returnString = returnDate.ToString("yyyy-MM-dd");
				return returnString;
			}
			set => DateIssuedFrom = value;
		}
		[Range(1, 6)]
		public int? DateIssuedFromDaysAgo { get; set; }
		public int? DateIssuedFromWeeksAgo { get; set; }
		public string DateIssuedTo
		{
			get
			{
				if (DateIssuedToDaysAgo == null && DateIssuedToWeeksAgo == null)
					return DateIssuedTo;
				int days, weeks;
				if (DateIssuedToDaysAgo == null && DateIssuedToWeeksAgo != null)
				{
					weeks = (int)DateIssuedToWeeksAgo;
					days = 0;
				}
				else if (DateIssuedToDaysAgo != null && DateIssuedToWeeksAgo == null)
				{
					days = (int)DateIssuedToDaysAgo;
					weeks = 0;
				}
				else
				{
					days = (int)DateIssuedToDaysAgo;
					weeks = (int)DateIssuedToWeeksAgo;
				}

				DateTime returnDate = DateTime.Today;
				returnDate.AddDays(-(weeks * 7 + days));
				string returnString = returnDate.ToString("yyyy-MM-dd");
				return returnString;
			}
			set => DateIssuedTo = value;
		}
		[Range(1, 6)]
		public int? DateIssuedToDaysAgo { get; set; }
		public int? DateIssuedToWeeksAgo { get; set; }
		public string DateSentFrom
		{
			get
			{
				if (DateSentFromDaysAgo == null && DateSentFromWeeksAgo == null)
					return DateSentFrom;
				int days, weeks;
				if (DateSentFromDaysAgo == null && DateSentFromWeeksAgo != null)
				{
					weeks = (int)DateSentFromWeeksAgo;
					days = 0;
				}
				else if (DateSentFromDaysAgo != null && DateSentFromWeeksAgo == null)
				{
					days = (int)DateSentFromDaysAgo;
					weeks = 0;
				}
				else
				{
					days = (int)DateSentFromDaysAgo;
					weeks = (int)DateSentFromWeeksAgo;
				}

				DateTime returnDate = DateTime.Today;
				returnDate.AddDays(-(weeks * 7 + days));
				string returnString = returnDate.ToString("yyyy-MM-dd");
				return returnString;
			}
			set => DateSentFrom = value;
		}
		[Range(1, 6)]
		public int? DateSentFromDaysAgo { get; set; }
		public int? DateSentFromWeeksAgo { get; set; }
		public string DateSentTo
		{
			get
			{
				if (DateSentToDaysAgo == null && DateSentToWeeksAgo == null)
					return DateSentTo;
				int days, weeks;
				if (DateSentToDaysAgo == null && DateSentToWeeksAgo != null)
				{
					weeks = (int)DateSentToWeeksAgo;
					days = 0;
				}
				else if (DateSentToDaysAgo != null && DateSentToWeeksAgo == null)
				{
					days = (int)DateSentToDaysAgo;
					weeks = 0;
				}
				else
				{
					days = (int)DateSentToDaysAgo;
					weeks = (int)DateSentToWeeksAgo;
				}

				DateTime returnDate = DateTime.Today;
				returnDate.AddDays(-(weeks * 7 + days));
				string returnString = returnDate.ToString("yyyy-MM-dd");
				return returnString;
			}
			set => DateSentTo = value;
		}
		[Range(1, 6)]
		public int? DateSentToDaysAgo { get; set; }
		public int? DateSentToWeeksAgo { get; set; }
	}
}
