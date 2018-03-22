using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.poczta.onet.pl", 465);
            MailMessage msg = new MailMessage("pwr.fhs@onet.pl", email, subject, message);

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("pwr.fhs@onet.pl", "FaultHandlingSystem1");
            msg.IsBodyHtml = true;

            smtpClient.Send(msg);
            return Task.CompletedTask;
        }
    }
}
