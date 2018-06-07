using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Fault_handling_system.Models;


namespace Fault_handling_system.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailWithAttachmentsAsync(string email, string subject, string message, Attachment[] attachments);
    }
}
