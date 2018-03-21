using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Fault_handling_system.Models;

namespace Fault_handling_system.Services
{
    public class ReportParser : IReportParser
    {
        private readonly ILogger<ReportParser> _logger;

        public ReportParser(ILogger<ReportParser> logger)
        {
            _logger = logger;
            _logger.LogInformation("Constructed ReportHandler");
        }

        public Report ParseReport(string sender, string subject, string message)
        {
            _logger.LogInformation("Handling report:\n" +
                                   "  Sender: {0}\n" +
                                   "  Subject: {1}",
                                   sender,
                                   subject);
            return null;
        }
    }
}
