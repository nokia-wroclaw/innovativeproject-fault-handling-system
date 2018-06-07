using Fault_handling_system.Data;
using Fault_handling_system.Models;
using Fault_handling_system.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Quartz;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    public class ReportSenderJob : IJob
{
        private IHostingEnvironment _hostingEnvironment;
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Main functionality of Job.
        /// Read all report from DB, filtered by Filter, export to Exel and send by Email
        /// It is started by Scheduler.
        /// </summary>
        /// <param name="context">Scheduler Context</param>
        public async Task Execute(IJobExecutionContext context)
        {
            var schedulerContext = context.Scheduler.Context;
            var logger = (ILogger)schedulerContext.Get("logger");
            var emailSender = (EmailSender)schedulerContext.Get("emailSender");
            serviceProvider = (IServiceProvider)schedulerContext.Get("serviceProvider");
            _hostingEnvironment = (IHostingEnvironment)schedulerContext.Get("hostingEnvironment");
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
                String[] MailinigList = { "" }; 
                if (ScheduleFilter.MailingList != null)
                {
                    MailinigList = ScheduleFilter.MailingList.Split(',');
                }
                 
                String message = "Welcome " + ScheduleFilter.User.UserName +","+ NewLine
                    + "your filtered reports in attachment. " ;

                logger.LogInformation("email: " + email + Environment.NewLine + " subject: " + subject + Environment.NewLine + " message: " + message + Environment.NewLine + "interval: " + interval + Environment.NewLine + "FilterId: " + filterId);

                Attachment[] attachments = { GetExellFile(DBcontext, ScheduleFilter.Filter) };
                await emailSender.SendEmailWithAttachmentsAsync(email, subject, message, attachments);
                foreach(String mail in MailinigList)
                {
                    if (IsValidEmail(mail))
                    {
                        await emailSender.SendEmailWithAttachmentsAsync(mail, subject, message, attachments);
                    } else
                    {
                        logger.LogWarning("Email " + mail + " is not valid");
                    }
                }


            }

        }

        /// <summary>
        /// Email validator
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>True or false</returns>
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

        /// <summary>
        /// Read reports from DB and export do Excel 
        /// </summary>
        /// <param name="dbContext">DB context</param>
        /// <param name="Filter">Filter</param>
        /// <return>Excel Attachment</return>
        private Attachment GetExellFile(ApplicationDbContext dbContext, ReportFilter Filter)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var _reportRepository = scope.ServiceProvider
                                        .GetRequiredService<IReportRepository>();
                const string DateFormat = "dd/MM/yyyy";
                byte[] bytes;
                string sWebRootFolder = _hostingEnvironment.WebRootPath;
                string sFileName = @"" + Filter.Name + "Report.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }

                IQueryable<Report> reports = _reportRepository.GetReportsWhereInvolved(Filter.UserId);

                reports = _reportRepository.GetFilteredReports(reports, Filter.EtrNumber, Filter.Priority, Filter.RfaId, Filter.RfaName, Filter.Grade, Filter.TroubleType, Filter.DateIssuedFrom, Filter.DateIssuedTo, Filter.DateSentFrom, Filter.DateSentTo, Filter.EtrStatus, Filter.EtrType, Filter.NsnCoordinatorId, Filter.SubcontractorId, Filter.Zone);



                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");
                    int line = 1;
                    //Add headers
                    worksheet.Cells[line, 1].Value = "EtrNumber";
                    worksheet.Cells[line, 2].Value = "NokiaCaseId";
                    worksheet.Cells[line, 3].Value = "RfaId";
                    worksheet.Cells[line, 4].Value = "RfaName";
                    worksheet.Cells[line, 5].Value = "AssignedTo";
                    worksheet.Cells[line, 6].Value = "Priority";
                    worksheet.Cells[line, 7].Value = "Grade";
                    worksheet.Cells[line, 8].Value = "TroubleType";
                    worksheet.Cells[line, 9].Value = "DateIssued";
                    worksheet.Cells[line, 10].Value = "DateSent";
                    worksheet.Cells[line, 11].Value = "EtrToDes";
                    worksheet.Cells[line, 12].Value = "ClosingDate";
                    worksheet.Cells[line, 13].Value = "EtrDescription";
                    worksheet.Cells[line, 14].Value = "Comment";
                    worksheet.Cells[line, 15].Value = "EtrStatus";
                    worksheet.Cells[line, 16].Value = "EtrType";
                    worksheet.Cells[line, 17].Value = "NsnCoordinator";
                    worksheet.Cells[line, 18].Value = "Requestor";
                    worksheet.Cells[line, 19].Value = "Subcontractor";
                    worksheet.Cells[line, 20].Value = "Zone";
                    line++;
                    //Add lines
                    foreach (Report report in reports)
                    {
                        worksheet.Cells[line, 1].Value = report.EtrNumber;
                        worksheet.Cells[line, 2].Value = report.NokiaCaseId;
                        worksheet.Cells[line, 3].Value = report.RfaId;
                        worksheet.Cells[line, 4].Value = report.RfaName;
                        worksheet.Cells[line, 5].Value = report.AssignedTo;
                        worksheet.Cells[line, 6].Value = report.Priority;
                        worksheet.Cells[line, 7].Value = report.Grade;
                        worksheet.Cells[line, 8].Value = report.TroubleType;
                        worksheet.Cells[line, 9].Value = report.DateIssued;
                        worksheet.Cells[line, 9].Style.Numberformat.Format = DateFormat;

                        worksheet.Cells[line, 10].Value = report.DateSent;
                        worksheet.Cells[line, 10].Style.Numberformat.Format = DateFormat;

                        worksheet.Cells[line, 11].Value = report.EtrToDes;
                        worksheet.Cells[line, 12].Value = report.ClosingDate;
                        worksheet.Cells[line, 12].Style.Numberformat.Format = DateFormat;

                        worksheet.Cells[line, 13].Value = report.EtrDescription;
                        worksheet.Cells[line, 14].Value = report.Comment;
                        worksheet.Cells[line, 15].Value = report.EtrStatus.Status;
                        worksheet.Cells[line, 16].Value = report.EtrType.Type;
                        worksheet.Cells[line, 17].Value = report.NsnCoordinator;
                        worksheet.Cells[line, 18].Value = report.Requestor;
                        worksheet.Cells[line, 19].Value = report.Subcontractor;
                        worksheet.Cells[line, 20].Value = report.Zone.ZoneName;
                        line++;
                    }

                    bytes = package.GetAsByteArray();

                }

                Attachment attachment = new Attachment(new MemoryStream(bytes), sFileName);

                return attachment;
            }
        }
    }
}
