using Fault_handling_system.Controllers;
using Fault_handling_system.Data;
using Fault_handling_system.Models;
using Fault_handling_system.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;


namespace Fault_handling_system.Services
{
    /// <summary>
    /// Scheduler Service.
    /// Contains all created job and processes them.
    /// Based on Quartz library.
    /// </summary>
    public class SchedulerService : ISchedulerService
    {
        private ILogger<SchedulerController> _logger;
        private ApplicationDbContext _context;
        private IScheduler scheduler;
        private IEmailSender _emailSender;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IReportRepository _reportRepository;

        /// <summary>
        /// SchedulerService constructor.
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="context">Instance of <c>ApplicationDbContext</c> is responsible for communication with SQL server</param>
        /// <param name="emailSender">emailSender</param>
        /// <param name="serviceProvider">serviceProvider</param>
        /// <param name="hostingEnvironment">IHostingEnvironment</param>
        /// <param name="reportRepository">IReportRepository</param>
        public SchedulerService(ILogger<SchedulerController> logger, ApplicationDbContext context, IEmailSender emailSender, IServiceProvider serviceProvider, IHostingEnvironment hostingEnvironment, IReportRepository reportRepository)
        {
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
            _serviceProvider = serviceProvider;
            _hostingEnvironment = hostingEnvironment;
            _reportRepository = reportRepository;
            InitSchedulerAsync();
            
        }

        /// <summary>
        /// Initlize Scheduler work.
        /// Create new Scheduler and put services to Context
        /// </summary>
        async Task InitSchedulerAsync()
        {
            // Grab the Scheduler instance from the Factory
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            scheduler = await factory.GetScheduler();


            // and start it off
            await scheduler.Start();

            scheduler.Context.Put("logger", _logger);
            scheduler.Context.Put("emailSender", _emailSender);
            scheduler.Context.Put("context", _context);
            scheduler.Context.Put("serviceProvider", _serviceProvider);
            scheduler.Context.Put("hostingEnvironment", _hostingEnvironment);
            scheduler.Context.Put("reportRepository", _reportRepository);

            InitDailyReportJob();

            //InitScheduledFilters();
        }

        /// <summary>
        /// Initlize scheduled jobs.
        /// Run all jobs with active status. Mainly, after app crash.
        /// </summary>
        async Task InitScheduledFilters()
        {
            var ScheduleFilters = _context.ScheduleFilter;
            foreach(ScheduleFilter scheduleFilter in ScheduleFilters)
            {
                if (scheduleFilter.Active)
                {
                   ((ISchedulerService)this).StartReportSendJob(scheduleFilter.Id);
                }              
            }
           
        }

        /// <summary>
        /// Initializes job responsible for sending daily reports to admins.
        /// </summary>
        void InitDailyReportJob()
        {
            IJobDetail job = JobBuilder.Create<DailyReportJob>()
                        .Build();

            string cron = "0 5 0 1/1 * ? *";

            ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithCronSchedule(cron)
                    .Build();

            AddNewJob(job, trigger).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Stop scheduler work and shutdown it.
        /// </summary>
        async Task CloseAsync()
        {
            await scheduler.Shutdown();
        }

        /// <summary>
        /// Add daily ReportSenderJob to Scheduler
        /// </summary>
        /// <param name="SchedulerFilterId">SchedulerFilterId</param>
        /// <param name="FilterId">FilterId</param>
        /// <param name="UserId">Creator Id</param>
        /// <param name="Hour">Hour</param>
        /// <param name="MailingLists">MailingLists</param>
        /// <returns>True of false</returns>
        bool ISchedulerService.AddHourly(int SchedulerFilterId, int FilterId,String UserId, String Hour, String MailingLists)
        {
            try
            {
                int MinutesPart;
                String interval = "Hourly";
                Int32.TryParse(Hour.Substring(3, 2), out MinutesPart);
                DateTimeOffset dateTimeOffset = new DateTimeOffset().AddMinutes(MinutesPart);
                IJobDetail job = JobBuilder.Create<ReportSenderJob>()
                        .WithIdentity("job"+SchedulerFilterId, "group" + SchedulerFilterId)                  
                        .UsingJobData("SchedulerFilterId", SchedulerFilterId)
                        .UsingJobData("FilterId", FilterId)
                        .UsingJobData("Interval", interval)
                        .Build();

                ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("job" + SchedulerFilterId, "group" + SchedulerFilterId)
                       .StartAt(dateTimeOffset)
                       .WithSimpleSchedule(x => x
                           .WithIntervalInHours(1)
                           .RepeatForever())
                       .Build();

                AddNewJob(job, trigger).GetAwaiter().GetResult();
                AddToDB(SchedulerFilterId, FilterId, UserId, interval, null, Hour, null, MailingLists);
                return true;
            }
            catch (Exception error)
            {
                _logger.LogError(error.ToString());
                return false;
            }
        }

        /// <summary>
        /// Add hourly ReportSenderJob to Scheduler
        /// </summary>
        /// <param name="SchedulerFilterId">SchedulerFilterId</param>
        /// <param name="FilterId">FilterId</param>
        /// <param name="UserId">Creator Id</param>
        /// <param name="Hour">Hour</param>
        /// <param name="MailingLists">MailingLists</param>
        /// <returns>True of false</returns>
        bool ISchedulerService.AddDaily(int SchedulerFilterId, int FilterId, String UserId, String Hour, String MailingLists)
        {
            try
            {
                int HourPart;
                int MinutesPart;
                Int32.TryParse(Hour.Substring(0, 2), out HourPart);
                Int32.TryParse(Hour.Substring(3, 2), out MinutesPart);
                String interval = "Daily";

                IJobDetail job = JobBuilder.Create<ReportSenderJob>()
                        .WithIdentity("job" + SchedulerFilterId, "group" + SchedulerFilterId)
                        .UsingJobData("SchedulerFilterId", SchedulerFilterId)
                        .UsingJobData("Interval", interval)
                        .UsingJobData("FilterId", FilterId)
                        .Build();

                ITrigger trigger = TriggerBuilder.Create()
                       .StartNow()
                       .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(HourPart, MinutesPart))
                       .Build();

                AddNewJob(job, trigger).GetAwaiter().GetResult();
                AddToDB(SchedulerFilterId, FilterId, UserId, interval, null, Hour, null, MailingLists);
                return true;
            }
            catch (Exception error)
            {
                _logger.LogError(error.ToString());
                return false;
            }

        }

        /// <summary>
        /// Add weekly ReportSenderJob to Scheduler
        /// </summary>
        /// <param name="SchedulerFilterId">SchedulerFilterId</param>
        /// <param name="FilterId">FilterId</param>
        /// <param name="UserId">Creator Id</param>
        /// <param name="Hour">Hour</param>
        /// <param name="DayOfWeek">Day of week</param>
        /// <param name="MailingLists">MailingLists</param>
        /// <returns>True of false</returns>
        bool ISchedulerService.AddWeekly(int SchedulerFilterId, int FilterId, String UserId, String Hour, String DayOfWeek, String MailingLists)
        {
            try
            {
                int HourPart;
                int MinutesPart;
                Int32.TryParse(Hour.Substring(0, 2), out HourPart);
                Int32.TryParse(Hour.Substring(3, 2), out MinutesPart);
                int Day = DayToInt(DayOfWeek);
                DateTimeOffset dateTimeOffset = new DateTimeOffset()
                    .AddDays(Day)
                    .AddHours(HourPart)
                    .AddMinutes(MinutesPart);
                String interval = "Weekly";
                IJobDetail job = JobBuilder.Create<ReportSenderJob>()
                        .WithIdentity("job" + SchedulerFilterId, "group" + SchedulerFilterId)
                        .UsingJobData("SchedulerFilterId", SchedulerFilterId)
                        .UsingJobData("Interval", interval)
                        .UsingJobData("FilterId", FilterId)
                        .Build();

                ITrigger trigger = TriggerBuilder.Create()
                       .StartNow()
                       .WithSchedule(CalendarIntervalScheduleBuilder
                            .Create()
                            .WithIntervalInWeeks(1)
                            .PreserveHourOfDayAcrossDaylightSavings(true))
                       .Build();

                AddNewJob(job, trigger).GetAwaiter().GetResult();
                AddToDB(SchedulerFilterId, FilterId, UserId, interval, null, Hour, DayOfWeek, MailingLists);
                return true;
            }
            catch (Exception error)
            {
                _logger.LogError(error.ToString());
                return false;
            }
        }

        /// <summary>
        /// Add cron ReportSenderJob to Scheduler
        /// </summary>
        /// <param name="SchedulerFilterId">SchedulerFilterId</param>
        /// <param name="FilterId">FilterId</param>
        /// <param name="UserId">Creator Id</param>
        /// <param name="cron">Unix cron</param>
        /// <param name="MailingLists">MailingLists</param>
        /// <returns>True of false</returns>
        bool ISchedulerService.AddCron(int SchedulerFilterId, int FilterId, String UserId, String cron, String MailingLists)
        {
            try
            {
                String QuartzCron = ConvertUnixCronToQuartzCron(cron);
                String interval = "Cron";
                IJobDetail job = JobBuilder.Create<ReportSenderJob>()
                            .WithIdentity("job" + SchedulerFilterId, "group" + SchedulerFilterId)
                            .UsingJobData("SchedulerFilterId", SchedulerFilterId)
                            .UsingJobData("Interval", interval)
                            .UsingJobData("FilterId", FilterId)
                            .Build();

                ITrigger trigger = TriggerBuilder.Create()
                       .StartNow()
                        .WithCronSchedule(QuartzCron)
                       .Build();
                
                AddNewJob(job, trigger).GetAwaiter().GetResult();
                AddToDB(SchedulerFilterId, FilterId, UserId, interval, cron, null, null, MailingLists);
                return true;
            }
            catch (Exception error)
            {
                _logger.LogError(error.ToString());
                return false;
            }
        }

        /// <summary>
        /// Start ReportSenderJob
        /// </summary>
        /// <param name="SchedulerFilterId">SchedulerFilterId</param>
        void ISchedulerService.StartReportSendJob(int SchedulerFilterId)
        {
            var ScheduleFilter = _context.ScheduleFilter
                .Where(r => r.Id == SchedulerFilterId).First();
            ScheduleFilter.Active = true;
            bool done = false;
            switch (ScheduleFilter.Interval)
            {
                case "Hourly":
                    done = ((ISchedulerService)this).AddHourly(ScheduleFilter.Id, ScheduleFilter.FilterId, ScheduleFilter.UserId, ScheduleFilter.Hour, ScheduleFilter.MailingList);
                    break;
                case "Daily":
                    done = ((ISchedulerService)this).AddDaily(ScheduleFilter.Id, ScheduleFilter.FilterId, ScheduleFilter.UserId, ScheduleFilter.Hour, ScheduleFilter.MailingList);
                    break;
                case "Weekly":
                    done = ((ISchedulerService)this).AddWeekly(ScheduleFilter.Id, ScheduleFilter.FilterId, ScheduleFilter.UserId, ScheduleFilter.Hour, ScheduleFilter.DayOfWeek ,ScheduleFilter.MailingList);
                    break;
                case "Cron":
                    done = ((ISchedulerService)this).AddCron(ScheduleFilter.Id, ScheduleFilter.FilterId, ScheduleFilter.UserId, ScheduleFilter.Cron, ScheduleFilter.MailingList);
                    break;
            }

            if (done)
            {
                _context.Update(ScheduleFilter);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Stop ReportSenderJob
        /// </summary>
        /// <param name="SchedulerFilterId">SchedulerFilterId</param>
        void ISchedulerService.StopReportSendJob(int SchedulerFilterId)
        {
            scheduler.DeleteJob(new JobKey("job"+SchedulerFilterId, "group"+SchedulerFilterId));
            var ScheduleFilter = _context.ScheduleFilter
                .Where(r => r.Id == SchedulerFilterId).First();
            ScheduleFilter.Active = false;
            _context.Update(ScheduleFilter);
            _context.SaveChanges();
        }

        /// <summary>
        /// Permamently remove ReportSenderJob
        /// </summary>
        /// <param name="SchedulerFilterId">SchedulerFilterId</param>
        void ISchedulerService.DeleteReportSendJob(int SchedulerFilterId)
        {
            scheduler.DeleteJob(new JobKey("job" + SchedulerFilterId, "group" + SchedulerFilterId));
            var ScheduleFilter = _context.ScheduleFilter
                .Where(r => r.Id == SchedulerFilterId).First();
            _context.Remove(ScheduleFilter);
            _context.SaveChanges();
        }

        /// <summary>
        /// Add new Job to Scheduler
        /// </summary>
        /// <param name="job">Job</param>
        /// <param name="trigger">Trigger</param>
        public async Task AddNewJob(IJobDetail job, ITrigger trigger)
        {
            try
            {
                // Tell quartz to schedule the job using our trigger
                await scheduler.ScheduleJob(job, trigger);

            }
            catch (SchedulerException se)
            {
                _logger.LogError(se.ToString());
            }
        }

        /// <summary>
        /// Converter day of week
        /// </summary>
        /// <param name="DayOfWeek">Day of Week</param>
        /// <returns>Number of day of week</returns>
        private int DayToInt(String DayOfWeek) {
            switch(DayOfWeek)
            {
                case "Monday":
                    return (int)System.DayOfWeek.Monday;
                case "Tuesday":
                    return (int)System.DayOfWeek.Tuesday;
                case "Wednesday":
                    return (int)System.DayOfWeek.Wednesday;
                case "Thursday":
                    return (int)System.DayOfWeek.Thursday;
                case "Friday":
                    return (int)System.DayOfWeek.Friday;
                case "Saturday":
                    return (int)System.DayOfWeek.Saturday;
                case "Sunday":
                    return (int)System.DayOfWeek.Sunday;
                default:
                    _logger.LogCritical("Wrong DayOfWeek");
                    return 0;
            }
        }

        /// <summary>
        /// Log information about Job and details to DB
        /// </summary>
        /// <param name="SchedulerFilterId">SchedulerFilterId</param>
        /// <param name="FilterId">FilterId</param>
        /// <param name="userId">Creator Id</param>
        /// <param name="Invterval">Job Interval</param>
        /// <param name="Cron">Unix Cron</param>
        /// <param name="Hour">Job hour</param>
        /// <param name="DayOfWeek">Day of Week</param>
        /// <param name="MailingLists">List of mails</param>
        private async void AddToDB(int SchedulerFilterId, int FilterId, String userId, String Invterval, String Cron, String Hour, String DayOfWeek, String MailingLists)
        {
            var id = SchedulerFilterId;
            var filterId = FilterId;
            var interval = Invterval;
            var cron = Cron;
            var hour = Hour;
            var dayOfWeek = DayOfWeek;
            ScheduleFilter scheduleFilter = new ScheduleFilter() {
                Id = id,
                UserId = userId,
                FilterId = filterId,
                Interval = interval,
                Cron = cron,
                Hour = hour,
                DayOfWeek =dayOfWeek,
                MailingList = MailingLists,
                Active = true};
            if (_context.ScheduleFilter.Any(o => o.Id == SchedulerFilterId))
            {
                var ScheduleFilter = _context.ScheduleFilter
               .Where(r => r.Id == SchedulerFilterId).First();
                ScheduleFilter.Active = true;
                _context.Update(ScheduleFilter);
            } else
            {
                _context.Add(scheduleFilter);
            }          
            _context.SaveChanges();
        }

        /// <summary>
        /// Convert Unix Cron to Quartz Cron expression
        /// </summary>
        /// <param name="UnixCron">Unix Cron</param>
        /// <returns>Quartz Cron</returns>
        private String ConvertUnixCronToQuartzCron(String UnixCron)
        {
            UnixCron = UnixCron.Trim();
            String QuartzCron;


            if (UnixCron[UnixCron.Length-1] == '*')
            {
                QuartzCron = "0 " + UnixCron.Substring(0,UnixCron.Length-1) + "? *";
            } else
            {
                QuartzCron = "0 " + UnixCron + " *";
            }

            return QuartzCron;
        }
    }
}

