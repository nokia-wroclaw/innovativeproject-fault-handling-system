using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Fault_handling_system.Services
{
    public class ReportHandler : IReportHandler
    {
        private readonly ILogger<ReportHandler> _logger;

        public ReportHandler(ILogger<ReportHandler> logger)
        {
            _logger = logger;
            _logger.LogInformation("Constructed ReportHandler");
        }

        public bool HandleReport(string sender, string subject, string message)
        {
            _logger.LogInformation("Handling report:\n" +
                                   "  Sender: {0}\n" +
                                   "  Subject: {1}",
                                   sender,
                                   subject);
            return false;
        }
    }
}
