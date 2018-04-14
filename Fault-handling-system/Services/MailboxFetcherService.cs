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
    /// <summary>This is a service running in background that periodically
    /// fetches the mailbox via IMAP protocol, using <c>MailboxFetcher</c>.</summary>
    /// <remarks>This service relies on <c>MailboxFetcher</c> to fetch and parse
    /// reports, and to insert valid reports to the database.</remarks>
    public class MailboxFetcherService : BackgroundService
    {
        private readonly ILogger<MailboxFetcherService> _logger;
        private readonly MailboxFetcherSettings _settings;
        private readonly IMailboxFetcher _fetcher;

        /// <summary>The constructor; not called directly, but by Dependency Injection
        /// mechanism.</summary>
        public MailboxFetcherService(IOptions<MailboxFetcherSettings> settings,
                                     ILogger<MailboxFetcherService> logger,
                                     IMailboxFetcher fetcher)
        {
            _logger = logger;
            _settings = settings.Value;
            _fetcher = fetcher;
        }

        /// <summary>Method that executes the task of this service.</summary>
        /// <remarks>See the description of the class to read about what this service does.
        /// </remarks>
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
