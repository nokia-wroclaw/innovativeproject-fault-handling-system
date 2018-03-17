using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Fault_handling_system.Services
{
    public class MailboxFetcher : IMailboxFetcher
    {
        private readonly ILogger<MailboxFetcher> _logger;
        private readonly string _host;
        private readonly int _port;
        private readonly bool _useSSL;
        private readonly string _username;
        private readonly string _password;

        public MailboxFetcher(ILogger<MailboxFetcher> logger,
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
            return Task.CompletedTask;
        }
    }
}
