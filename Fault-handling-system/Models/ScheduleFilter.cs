using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Models
{
    public class ScheduleFilter
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Required]
        public int FilterId { get; set; }
        public virtual ReportFilter Filter { get; set; }
        [Required]
        public String Interval { get; set; }
        public String Cron { get; set; }
        public String Hour { get; set; }
        public String DayOfWeek { get; set; }
        public String MailingList { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
