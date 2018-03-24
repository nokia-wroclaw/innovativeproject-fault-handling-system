namespace Fault_handling_system.Models
{
    public class EmailSenderSettings
    {
        public EmailSenderSettings()
        {
        }

        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; } = 465;
        public string MailAddress { get; set; }
        public string MailLogin { get; set; }
        public string MailPassword { get; set; }
    }
}
