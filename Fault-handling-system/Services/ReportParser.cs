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
            bool hasRfaId = false;
            bool hasZoneId = false;
            bool hasEtrTypeId = false;
            bool hasEtrStatusId = false;

            Report report = new Report();

            // For "New ETR" mails, it is always "In Realization" - hardcode
            report.EtrStatusId = 2;
            hasEtrStatusId = true;

            MatchCollection mc = Regex.Matches(message, "^([^:\n]+): ([^\n]+)$",
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
                        hasRfaId = true;
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
                    report.TroubleType = value;
                    break;
                case "Subsystem":
                    // TODO map it to EtrType entity
                    report.EtrTypeId = 1;
                    hasEtrTypeId = true; // I believe this is this field; TODO check it
                    break;
                case "Vendor Priority/Severity":
                    report.Priority = value;
                    break;
                case "Trouble Start Time":
                    report.DateIssued = DateTime.Parse(value);
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
                        hasZoneId = true;
                    } catch (FormatException) {
                        _logger.LogError("Couldn't parse number: {0}: '{1}'", key, value);
                    }
                    break;
                case "Software Version":
                    // TODO which field is it?
                    break;
                case "Created Date":
                    report.DateSent = DateTime.Parse(value);
                    break;
                case "Originator Name/Requestor Name":
                    // TODO map it to the requestor entity
                    break;
                case "Trouble Coordinator":
                    // TODO map it to the coordinator entity
                    break;
                case "Content-Type":
                    // Antispam filter :) For now let's assume all reports are text/plain
                    if (value.StartsWith("text/html")) {
                        _logger.LogDebug("Antispam filter denies parsing of '{0}'", subject);
                        return null;
                    }
                    break;
                case "Content-Disposition":
                case "Content-Transfer-Encoding":
                case "Content-Id":
                    // Ignore
                    break;
                default:
                    // Ignore unknown key-value pairs
                    break;
                }
            }

            Match m = Regex.Match(message, @"^Description:\s(.*)", RegexOptions.Multiline);
            if (m.Success)
                report.EtrDescription = m.Groups[1].Value;

            if (report.RequestorId == null) {
                // If we didn't find the requestor, assign hardcoded unknown.requestor@example.com
                // TODO: This is just for demo. Do it better.
                report.RequestorId = "6963f5c6-c245-4369-8077-af688a6b639b";
            }

            // Check if the report has all required fields. If yes, return it; otherwise
            // return null to indicate that parsing failed.
            if (report.EtrNumber != null
             && report.EtrDescription != null
             && report.DateIssued != null
             && report.RequestorId != null
             && hasRfaId
             && hasZoneId
             && hasEtrTypeId
             && hasEtrStatusId) {
                return report;
            } else {
                string missingFields = "";
                if (report.EtrNumber == null)
                    missingFields += "EtrNumber ";
                if (report.EtrDescription == null)
                    missingFields += "EtrDescription ";
                if (report.DateIssued == null)
                    missingFields += "DateIssued ";
                if (report.RequestorId == null)
                    missingFields += "Requestor ";
                if (!hasRfaId)
                    missingFields += "RfaId ";
                if (!hasZoneId)
                    missingFields += "Zone ";
                if (!hasEtrTypeId)
                    missingFields += "EtrType ";
                if (!hasEtrStatusId)
                    missingFields += "EtrStatus ";

                if (report.EtrNumber != null)
                    _logger.LogDebug("Parsing of {0} failed because of missing fields: {1}",
                                     report.EtrNumber, missingFields);
                else
                    _logger.LogDebug("Parsing of '{0}' failed because of missing fields: {1}",
                                     subject, missingFields);

                return null;
            }
        }
    }
}
