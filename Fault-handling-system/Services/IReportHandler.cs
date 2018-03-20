using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    public interface IReportHandler
    {
        /// <summary>
        /// Method that reads a single mail that possibly contains a valid report.
        /// </summary>
        /// If the mail contains a valid report, it will be inserted into the database
        /// and the method will return true. Otherwise it will return false.
        bool HandleReport(string sender, string subject, string message);
    }
}
