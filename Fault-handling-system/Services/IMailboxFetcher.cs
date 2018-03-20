using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Services
{
    public interface IMailboxFetcher
    {
        Task FetchMailbox();
        void Configure(string host, int port, bool useSSL, string username, string password);
    }
}
