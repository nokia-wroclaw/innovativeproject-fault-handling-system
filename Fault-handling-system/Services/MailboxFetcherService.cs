using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Fault_handling_system.Models;

namespace Fault_handling_system.Services
{
    public class MailboxFetcherService : BackgroundService
    {
        private readonly ILogger<MailboxFetcherService> _logger;
        private readonly MailboxFetcherSettings _settings;
        private readonly IMailboxFetcher _fetcher;

        public MailboxFetcherService(IOptions<MailboxFetcherSettings> settings,
                                     ILogger<MailboxFetcherService> logger,
                                     IMailboxFetcher fetcher)
        {
            _logger = logger;
            _settings = settings.Value;
            _fetcher = fetcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_settings.DoMailboxFetching) {
                _logger.LogInformation("Not fetching mailbox, as requested in configuration");
                return;
            }

            stoppingToken.Register(() => 
                    _logger.LogDebug("Mail fetcher task is stopping..."));

            _fetcher.Configure(_settings.ImapServer, _settings.ImapPort, true,
                     _settings.MailLogin, _settings.MailPassword);

            while (!stoppingToken.IsCancellationRequested) {
                bool result = _fetcher.FetchMailbox();
                int checkInterval = _settings.CheckInterval;
                _logger.LogInformation("Next check after {0} s...", checkInterval);
                await Task.Delay(checkInterval * 1000, stoppingToken);
            }
        }
    }
}
