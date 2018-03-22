using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fault_handling_system.Models;

namespace Fault_handling_system.Services
{
    public interface IReportParser
    {
        /// <summary>
        /// Method that constructs a Report object from a single mail that possibly contains
        /// a valid report.
        /// </summary>
        /// If the mail contains a valid report, the method returns a constructed
        /// model object. Otherwise it returns null.
        Report ParseReport(string sender, string subject, string message);
    }
}
