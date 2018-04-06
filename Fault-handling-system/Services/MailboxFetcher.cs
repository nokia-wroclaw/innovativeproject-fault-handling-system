using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fault_handling_system.Models;
using Fault_handling_system.Data;

namespace Fault_handling_system.Services
{
    public class MailboxFetcher : IMailboxFetcher
    {
        private readonly ILogger<MailboxFetcher> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IReportParser _reportParser;
        private string _host;
        private int _port;
        private bool _useSSL;
        private string _username;
        private string _password;

        public MailboxFetcher(ILogger<MailboxFetcher> logger,
                              IServiceProvider serviceProvider,
                              IReportParser reportParser)
        {
            _logger = logger;
            _logger.LogInformation("Constructed MailboxFetcher");
            _serviceProvider = serviceProvider;
            _reportParser = reportParser;
        }

        public void Configure(string host, int port, bool useSSL, string username, string password)
        {
            _host = host;
            _port = port;
            _useSSL = useSSL;
            _username = username;
            _password = password;
        }

        public bool FetchMailbox()
        {
            _logger.LogInformation("Checking mailbox...");

            using (var client = new ImapClient()) {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s,c,h,e) => true;

                client.Connect(_host, _port, _useSSL);

                try {
                    client.Authenticate(_username, _password);

                    // Find folders: parsed, failed
                    var folders = client.GetFolders(client.PersonalNamespaces[0]);
                    MailKit.IMailFolder parsed = null;
                    MailKit.IMailFolder failed = null;
                    foreach (var folder in folders) {
                        if (folder.Name == "parsed")
                            parsed = folder;
                        else if (folder.Name == "failed")
                            failed = folder;
                    }

                    if (parsed == null || failed == null) {
                        _logger.LogError("Couldn't find 'parsed' or 'failed' folder in the mailbox");
                        client.Disconnect(true);
                        return false;
                    } else {
                        _logger.LogInformation("Successfully found 'parsed' and 'failed' folders");
                        parsed.Open(FolderAccess.ReadWrite);
                        failed.Open(FolderAccess.ReadWrite);
                    }

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);

                    _logger.LogInformation("Total messages: {0}", inbox.Count);
                    _logger.LogInformation("Recent messages: {0}", inbox.Recent);

                    // Not sure if we can iterate forwards and move the messages at the same time.
                    // Thus iterating backwards.
                    for (int i = inbox.Count - 1; i >= 0; --i) {
                        var message = inbox.GetMessage(i);

                        string sender;
                        string subject;
                        string body;

                        if (message.Sender != null)
                            sender = message.Sender.Address;
                        else
                            sender = "<null>";
                        if (message.Subject != null)
                            subject = message.Subject;
                        else
                            subject = "<null>";
                        if (message.Body != null)
                            body  = message.Body.ToString();
                        else
                            body = "<null>";

                        Report report = _reportParser.ParseReport(sender, subject, body);

                        if (report != null) {
                            _logger.LogInformation("Successfully parsed report '{0}' from {1}",
                                                   subject, sender);
                            inbox.AddFlags(i, MessageFlags.Seen, true);
                            inbox.MoveTo(i, parsed);

                            using (IServiceScope scope = _serviceProvider.CreateScope()) {
                                var context = scope.ServiceProvider
                                                        .GetRequiredService<ApplicationDbContext>();
                                context.Report.Add(report);
                                context.SaveChanges();
                            }
                        } else {
                            _logger.LogWarning("Failed to parse report '{0}' from {1}",
                                               subject, sender);
                            inbox.AddFlags(i, MessageFlags.Seen, true);
                            inbox.MoveTo(i, failed);
                        }
                    }

                    client.Disconnect(true);
                } catch (ImapProtocolException e) {
                    _logger.LogError("An exception occured while trying to fetch mailbox: {0}", e);
                }
            }

            _logger.LogInformation("Finished checking.");
            return true;
        }
    }
}
