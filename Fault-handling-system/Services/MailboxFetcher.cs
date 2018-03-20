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
        private readonly ILogger<MailboxFetcher> _logger;
        private string _host;
        private int _port;
        private bool _useSSL;
        private string _username;
        private string _password;

        public MailboxFetcher(ILogger<MailboxFetcher> logger)
        {
            _logger = logger;
            _logger.LogInformation("Constructed MailboxFetcher");
        }

        public void Configure(string host, int port, bool useSSL, string username, string password)
        {
            _host = host;
            _port = port;
            _useSSL = useSSL;
            _username = username;
            _password = password;
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
