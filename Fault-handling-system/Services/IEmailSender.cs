using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    /// <summary>Class implementing this interface is used by the application to send email
    /// for account confirmation and password reset.</summary>
    public interface IEmailSender
    {
        /// <summary>Sends mail. Mail properties are passed as parameters.</summary>
        /// <param name="email">Recipient email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="message">Email message body</param>
        Task SendEmailAsync(string email, string subject, string message);
    }
}
