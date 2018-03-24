namespace Fault_handling_system.Models
{
    public class MailboxFetcherSettings
    {
        public MailboxFetcherSettings()
        {
        }

        public int CheckInterval { get; set; } = 15;
        public string ImapServer { get; set; }
        public int ImapPort { get; set; } = 993;
        public string MailLogin { get; set; }
        public string MailPassword { get; set; }
    }
}
