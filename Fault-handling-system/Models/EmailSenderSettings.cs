namespace Fault_handling_system.Models
{
    /// <summary>
    /// A class representing application settings that are read by <c>EmailSender</c>
    /// </summary>
    public class EmailSenderSettings
    {
        /// <summary>The constructor; not called directly, but by Dependency Injection
        /// mechanism.</summary>
        public EmailSenderSettings()
        {
        }

        /// <summary>SmtpServer provides the hostname of the SMTP server that needs to be
        /// connected to in order to send mail.</summary>
        public string SmtpServer { get; set; }

        /// <summary>SmtpPort specifies TCP port on which SMTP should be accessed.
        /// The default works for most mail servers.</summary>
        public int SmtpPort { get; set; } = 465;

        /// <summary>MailAddress specifies the e-mail address from which mail should be sent.</summary>
        public string MailAddress { get; set; }

        /// <summary>MailLogin specifies the user login to the mailbox.</summary>
        public string MailLogin { get; set; }

        /// <summary>MailPassword specifies the user password to the mailbox.</summary>
        public string MailPassword { get; set; }
    }
}
