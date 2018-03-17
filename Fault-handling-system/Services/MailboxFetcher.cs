using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;
using Microsoft.Extensions.Logging;

namespace Fault_handling_system.Services
{
    public class MailboxFetcher : IMailboxFetcher
    {
        private readonly ILogger<MailboxFetcherService> _logger;
        private readonly string _host;
        private readonly int _port;
        private readonly bool _useSSL;
        private readonly string _username;
        private readonly string _password;

        public MailboxFetcher(ILogger<MailboxFetcherService> logger,
            string host, int port, bool useSSL, string username, string password)
        {
            _logger = logger;
            _host = host;
            _port = port;
            _useSSL = useSSL;
            _username = username;
            _password = password;
            _logger.LogInformation("Constructed MailboxFetcher");
        }

        public Task FetchMailbox()
        {
            _logger.LogInformation("Checking mailbox...");

            using (var client = new ImapClient()) {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s,c,h,e) => true;

                client.Connect(_host, _port, _useSSL);

                client.Authenticate(_username, _password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                _logger.LogInformation("Total messages: {0}", inbox.Count);
                _logger.LogInformation("Recent messages: {0}", inbox.Recent);

                for (int i = 0; i < inbox.Count; ++i) {
                    var message = inbox.GetMessage(i);
                    _logger.LogInformation("Subject: {0}", message.Subject);
                }

                client.Disconnect(true);
            }

            _logger.LogInformation("Finished checking.");
            return Task.CompletedTask;
        }
    }
}
