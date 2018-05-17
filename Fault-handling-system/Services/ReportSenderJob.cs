using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    public class ReportSenderJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
            var schedulerContext = context.Scheduler.Context;
            var logger = (ILogger)schedulerContext.Get("logger");
            var emailSender = (EmailSender)schedulerContext.Get("emailSender");


            JobDataMap dataMap = context.JobDetail.JobDataMap;

            string interval = dataMap.GetString("Interval");
            int filterId = dataMap.GetInt("FilterId");

            string email = "fhs_test_scheduler@wp.pl";
            string subject = "ReportSenderJob";
            string message = "ReportSenderJob message";

            logger.LogInformation("email: "+email+ Environment.NewLine+" subject: " +subject+ Environment.NewLine+" message: " + message+Environment.NewLine+"interval: "+interval+ Environment.NewLine+"FilterId: "+filterId);

            
            await emailSender.SendEmailAsync(email, subject, message);
        
    }
}
}
