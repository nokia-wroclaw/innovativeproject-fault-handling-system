using Quartz;
using System;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    public interface ISchedulerService
    {
        bool AddHourly(int SchedulerFilterId, int FilterId, String UserId, String Hour, String MailingLists);
        bool AddDaily(int SchedulerFilterId, int FilterId, String UserId, String Hour, String MailingLists);
        bool AddWeekly(int SchedulerFilterId, int FilterId, String UserId, String Hour, String DayOfWeek, String MailingLists);
        bool AddCron(int SchedulerFilterId, int FilterId, String UserId, String cron, String MailingLists);

        Task AddNewJob(IJobDetail job, ITrigger trigger);

        void StartReportSendJob(int SchedulerFilterId);
        void StopReportSendJob(int SchedulerFilterId);
        void DeleteReportSendJob(int SchedulerFilterId);
    }
}
