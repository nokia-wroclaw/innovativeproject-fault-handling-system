using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
    public class ReportsAndFiltersViewModel
    {
		public IEnumerable<Report> Reports { get; set; }
		public ReportFilter Filter { get; set; }
		//public String UserId { get; set; }
		public ReportsAndFiltersViewModel (IEnumerable<Report> reports, ReportFilter filter)
		{
			Reports = reports;
			Filter = filter;
		}
    }
}
