using Fault_handling_system.Controllers;
using Fault_handling_system.Models;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    public class SchedulerService : ISchedulerService
    {
        private ILogger<SchedulerController> _logger;
        private IScheduler scheduler;
        private IEmailSender _emailSender;

        public SchedulerService(ILogger<SchedulerController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
            initSchedulerAsync();
        }

        async Task initSchedulerAsync()
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
        }
        async Task closeAsync()
        {
            await scheduler.Shutdown();
        }

        void ISchedulerService.AddHourly(int FilterId, String Hour)
        {
            IJobDetail job = JobBuilder.Create<ReportSenderJob>()
                    .UsingJobData("Interval", "Hourly")
                    .UsingJobData("FilterId", FilterId)
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                   .StartNow()
                   .WithSimpleSchedule(x => x
                       .WithIntervalInHours(1)
                       .RepeatForever())
                   .Build();

            AddNewJob(job, trigger).GetAwaiter().GetResult();
        }
        void ISchedulerService.AddDaily(int FilterId, String Hour)
        {
            IJobDetail job = JobBuilder.Create<ReportSenderJob>()
                    .UsingJobData("Interval", "Daily")
                    .UsingJobData("FilterId", FilterId)
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                   .StartNow()
                   .WithSimpleSchedule(x => x
                       .WithIntervalInHours(24)
                       .RepeatForever())
                   .Build();

            AddNewJob(job, trigger).GetAwaiter().GetResult();
        }
        void ISchedulerService.AddWeekly(int FilterId, String Hour, String DayOfWeek)
        {
            IJobDetail job = JobBuilder.Create<ReportSenderJob>()
                    .UsingJobData("Interval", "Weekly")
                    .UsingJobData("FilterId", FilterId)
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                   .StartNow()
                   .WithSimpleSchedule(x => x
                       .WithIntervalInHours(24*7)
                       .RepeatForever())
                   .Build();

            AddNewJob(job, trigger).GetAwaiter().GetResult();
        }
        void ISchedulerService.AddCron(int FilterId, String cron)
        {
            IJobDetail job = JobBuilder.Create<ReportSenderJob>()
                    .UsingJobData("Interval", "Weekly")
                    .UsingJobData("FilterId", FilterId)
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                   .StartNow()
                    .WithCronSchedule(cron)
                   .Build();

            AddNewJob(job, trigger).GetAwaiter().GetResult();
        }
        
        private async Task AddNewJob(IJobDetail job, ITrigger trigger)
        {
            try
            {
                 // Tell quartz to schedule the job using our trigger
                await scheduler.ScheduleJob(job, trigger);

            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}

