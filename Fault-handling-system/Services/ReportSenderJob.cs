using Fault_handling_system.Data;
using Fault_handling_system.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
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
            var serviceProvider = (IServiceProvider)schedulerContext.Get("serviceProvider");
            //var DBcontext = (ApplicationDbContext)schedulerContext.Get("context");
            
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            string interval = dataMap.GetString("Interval");
            int filterId = dataMap.GetInt("FilterId");
            int SchedulerFilterId = dataMap.GetInt("SchedulerFilterId");
            String NewLine = Environment.NewLine;

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var DBcontext = scope.ServiceProvider
                                        .GetRequiredService<ApplicationDbContext>();


                var ScheduleFilter = DBcontext.ScheduleFilter
                .Include(r => r.Filter)
                .Include(r => r.User)
                .Where(r => r.Id == SchedulerFilterId).First();


                String email = ScheduleFilter.User.Email;
                String subject = ScheduleFilter.Filter.Name;
                var MailinigList = ScheduleFilter.MailingList.Split(',');
                String message = "Welcome " + ScheduleFilter.User.UserName +","+ NewLine
                    + "your filtered reports in attachment. " ;

                logger.LogInformation("email: " + email + Environment.NewLine + " subject: " + subject + Environment.NewLine + " message: " + message + Environment.NewLine + "interval: " + interval + Environment.NewLine + "FilterId: " + filterId);


                await emailSender.SendEmailAsync(email, subject, message);
                foreach(String mail in MailinigList)
                {
                    if (IsValidEmail(mail))
                    {
                        await emailSender.SendEmailAsync(mail, subject, message);
                    } else
                    {
                        logger.LogWarning("Email " + mail + " is not valid");
                    }
                }


            }

        }

        private bool IsValidEmail(String email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                
                return false;
            }
        }
    }
}
