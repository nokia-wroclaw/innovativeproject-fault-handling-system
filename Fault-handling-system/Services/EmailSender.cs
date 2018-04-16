using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Fault_handling_system.Models;

namespace Fault_handling_system.Services
{
    /// <summary>This class is used by the application to send email for account confirmation
    /// and password reset.
    /// For more details see https://go.microsoft.com/fwlink/?LinkID=532713</summary>
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderSettings _settings;

        /// <summary>The constructor; not called directly, but by Dependency Injection
        /// mechanism.</summary>
        public EmailSender(IOptions<EmailSenderSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>Sends mail. Mail properties are passed as parameters. SMTP settings
        /// are read automatically from the application configuration, see
        /// <c>EmailSenderSettings</c></summary>
        /// <param name="email">Recipient email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="message">Email message body</param>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient smtpClient = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort);
            MailMessage msg = new MailMessage(_settings.MailAddress, email, subject, message);

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_settings.MailLogin, _settings.MailPassword);
            msg.IsBodyHtml = true;

            smtpClient.Send(msg);
            return Task.CompletedTask;
        }
    }
}
