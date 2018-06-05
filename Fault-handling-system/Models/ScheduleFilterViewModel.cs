using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
    public class ScheduleFilterViewModel
    {
		public IEnumerable<ScheduleFilter> ScheduleFilter { get; set; }


		public ScheduleFilterViewModel(IEnumerable<ScheduleFilter> scheduleFilter)
		{
            ScheduleFilter = scheduleFilter;
		}
    }
}
