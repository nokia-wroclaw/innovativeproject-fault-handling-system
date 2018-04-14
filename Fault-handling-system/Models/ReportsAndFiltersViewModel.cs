using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
    public class ReportsAndFiltersViewModel
    {
		public IEnumerable<Report> Reports { get; set; }
		public IEnumerable<ReportFilter> Filters { get; set; }
		public ReportsAndFiltersViewModel (IEnumerable<Report> reports, IEnumerable<ReportFilter> filters)
		{
			Reports = reports;
			Filters = filters;
		}
    }
}
