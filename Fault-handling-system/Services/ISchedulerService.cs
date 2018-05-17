using Fault_handling_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    public interface ISchedulerService
    {
        void AddHourly(int FilterId, String Hour);
        void AddDaily(int FilterId, String Hour);
        void AddWeekly(int FilterId, String Hour, String DayOfWeek);
        void AddCron(int FilterId, String cron);
    }
}
