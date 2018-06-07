using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Fault_handling_system.Models;
using System.Text.Encodings.Web;
using System.Text;

namespace Fault_handling_system.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderSettings _settings;

        public EmailSender(IOptions<EmailSenderSettings> settings)
        {
            _settings = settings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient smtpClient = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort);
            MailMessage msg = new MailMessage(_settings.MailAddress, email, subject, message);

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_settings.MailLogin, _settings.MailPassword);
            msg.IsBodyHtml = true;

            smtpClient.SendMailAsync(msg);
            return Task.CompletedTask;
        }

        public Task SendDailyReport(string email, List<Report> todaysReports) 
        {
            String subject = "Daily report";
            string message = buildMessage(email, todaysReports);

            SmtpClient smtpClient = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_settings.MailLogin, _settings.MailPassword);
            
            MailMessage msg = new MailMessage(_settings.MailAddress, email, subject, message);
            msg.IsBodyHtml = true;
            smtpClient.SendMailAsync(msg);

            return Task.CompletedTask;
        }

        private string buildMessage(string email, List<Report> todaysReports) 
        {
            StringBuilder builder = new StringBuilder();

            string welcome = "<h3>Hello, " + email + "!</h3><br>";
            string farewell = "<br>Kind regards,<br>Nokia FHS Team";
            string reportsCountInfo = "There are: " + todaysReports.Count + " new reports.<br><br>";
            string etrNumbersTitle = "Their ETR numbers:";

            builder.Append(welcome);
            builder.Append(reportsCountInfo);
            
            if(todaysReports.Count > 0)
            {
                builder.Append(etrNumbersTitle);
                builder.Append("<ol type = \"1\">");
                todaysReports.ForEach(report => builder.Append("<li>" + report.EtrNumber + "</li>"));
                builder.Append("</ol>");
            }

            builder.Append(farewell);

            return builder.ToString();
        }
    }
}
