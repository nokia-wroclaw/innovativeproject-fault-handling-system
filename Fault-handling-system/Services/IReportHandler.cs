using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    public interface IReportHandler
    {
        Task HandleReportAsync(string sender, string subject, string message);
    }
}
