using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

            Report report = new Report();
            MatchCollection mc = Regex.Matches(message, "^([^:\n]+): ([^:\n]+)$",
                                               RegexOptions.Multiline);

            foreach (Match match in mc) {
                string key = match.Groups[1].Value;
                string value = match.Groups[2].Value;
                switch (key) {
                case "Polkomtel ETR Number":
                    report.EtrNumber = value;
                    break;
                case "Nokia case id":
                    if (value != "NULL") {
                        try {
                            report.NokiaCaseId = Convert.ToInt64(value);
                        } catch (FormatException) {
                            _logger.LogError("Couldn't parse number: {0}: '{1}'", key, value);
                        }
                    }
                    break;
                case "Assigned To":
                    report.AssignedTo = value;
                    break;
                case "RFA ID":
                    try {
                        report.RfaId = Convert.ToInt64(value);
                    } catch (FormatException) {
                        _logger.LogError("Couldn't parse number: {0}: '{1}'", key, value);
                    } catch (OverflowException) {
                        // TODO Change the type in the model
                        _logger.LogError("Overflow error: {0}: '{1}'", key, value);
                    }
                    break;
                case "Ocena":
                    try {
                        report.Grade = Convert.ToInt32(value);
                    } catch (FormatException) {
                        _logger.LogError("Couldn't parse number: {0}: '{1}'", key, value);
                    }
                    break;
                case "Trouble Type/Case Title":
                    // TODO parse it
                    break;
                case "Subsystem":
                    // TODO which field is it?
                    break;
                case "Vendor Priority/Severity":
                    report.Priority = value;
                    break;
                case "Trouble Reported Date":
                    // TODO parse it
                    break;
                case "Network Element ID":
                    // TODO which field is it?
                    break;
                case "Network Site Name":
                    // TODO which field is it?
                    break;
                case "Zone":
                    try {
                        report.ZoneId = Convert.ToInt32(value);
                    } catch (FormatException) {
                        _logger.LogError("Couldn't parse number: {0}: '{1}'", key, value);
                    }
                    break;
                case "Software Version":
                    // TODO which field is it?
                    break;
                case "Originator Name/Requestor Name":
                    // TODO map it to the requestor entity
                    break;
                case "Trouble Coordinator":
                    // TODO map it to the coordinator entity
                    break;
                case "Content-Type":
                case "Content-Disposition":
                case "Content-Transfer-Encoding":
                case "Content-Id":
                    // Ignore
                    break;
                default:
                    _logger.LogDebug("Unknown key-value pair: {0}: {1}", key, value);
                    break;
                }
            }

            return null;
        }
    }
}
