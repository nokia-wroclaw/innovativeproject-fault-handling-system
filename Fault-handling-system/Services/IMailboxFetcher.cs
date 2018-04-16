using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    /// <summary>Class implementing this interface is used by <c>MailboxFetcherService</c>
    /// to perform fetching of mail in the mailbox.</summary>
    public interface IMailboxFetcher
    {
        /// <summary>Configures the mailbox fetcher.</summary>
        /// <remarks>By design, the constructor only accepts Dependency Injected objects as
        /// parameters; the parameters of the fetchers need to be passed here before use.
        /// </remarks>
        void Configure(string host, int port, bool useSSL, string username, string password);

        /// <summary>Fetches the mailbox</summary>
        /// <remarks>This method performs a single fetch of the mailbox using IMAP
        /// protocol and also passed the mail to <c>ReportParser</c>. It then
        /// tries to parse the report. This method then moves the mail to 'failed'
        /// or 'parsed' folder on the mailbox, and inserts the new report to the
        /// database if it was correct.</remarks>
        bool FetchMailbox();
    }
}
