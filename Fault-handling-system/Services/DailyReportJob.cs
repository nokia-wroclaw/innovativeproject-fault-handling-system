using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fault_handling_system.Data;
using Fault_handling_system.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Fault_handling_system.Services
{
    public class DailyReportJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var schedulerContext = context.Scheduler.Context;
            var emailSender = (EmailSender) schedulerContext.Get("emailSender");
            var serviceProvider = (IServiceProvider) schedulerContext.Get("serviceProvider");

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                                        .GetRequiredService<ApplicationDbContext>();

                List<Report> yesterdaysReports = filterYesterdaysReports(dbContext);
                List<string> adminEmails = filterAdminEmails(dbContext);
                
                adminEmails.ForEach(email => emailSender.SendDailyReport(email, yesterdaysReports));
            }

        }

        private List<Report> filterYesterdaysReports(ApplicationDbContext dbContext)
        {
            var reportsWithDateSent = dbContext.Report.Where(r => r.DateSent != null).ToList();
            List<Report> yesterdaysReports = new List<Report>();

            foreach(Report report in reportsWithDateSent)
            {
                if(report.DateSent.Value.Date.Equals(DateTime.Now.AddDays(-1).Date))
                {
                    yesterdaysReports.Add(report);
                }
            }

            return yesterdaysReports;
        }

        private List<string> filterAdminEmails(ApplicationDbContext dbContext)
        {
            const string ADMIN = "Admin";
            var adminRole = dbContext.Roles.Where(r => r.Name.Equals(ADMIN)).First();
            var userRoleRelationIds = dbContext.UserRoles.Where(u => u.RoleId.Equals(adminRole.Id)).ToList();

            List<string> adminIds = new List<string>();
            userRoleRelationIds.ForEach(id => adminIds.Add(id.UserId));
            List<string> adminEmails = new List<string>();
            adminIds.ForEach(id => adminEmails.Add(dbContext.Users.Where(x => x.Id.Equals(id)).First().Email));

            return adminEmails;
        }

    }
}