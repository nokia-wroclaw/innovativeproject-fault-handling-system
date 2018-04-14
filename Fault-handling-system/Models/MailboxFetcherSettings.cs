namespace Fault_handling_system.Models
{

    /// <summary>
    /// A class representing application settings that are read by <c>MailboxFetcher</c>
    /// </summary>
    public class MailboxFetcherSettings
    {
        /// <summary>The constructor; not called directly, but by Dependency Injection
        /// mechanism.</summary>
        public MailboxFetcherSettings()
        {
        }

        /// <summary>DoMailboxFetching may be set to false in order to disable mailbox fetching
        /// in the background. This may be useful for developers who don't want to see
        /// logs from fetching and parsing.</summary>
        public bool DoMailboxFetching { get; set; } = true;

        /// <summary>CheckInterval specifies interval in seconds between two sequential
        /// fetches of the mailbox.</summary>
        public int CheckInterval { get; set; } = 15;

        /// <summary>ImapServer provides the hostname of the IMAP server that needs to be
        /// connected to in order to fetch mail.</summary>
        public string ImapServer { get; set; }

        /// <summary>ImapPort specifies TCP port on which IMAP should be accessed.
        /// The default works for most mail servers.</summary>
        public int ImapPort { get; set; } = 993;

        /// <summary>MailLogin specifies the user login to the mailbox.</summary>
        public string MailLogin { get; set; }

        /// <summary>MailPassword specifies the user password to the mailbox.</summary>
        public string MailPassword { get; set; }
    }
}
