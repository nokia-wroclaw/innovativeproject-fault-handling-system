using Fault_handling_system.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fault_handling_system.Models;

namespace Fault_handling_system.Repositories
{
    public class ReportRepository
    {
		private readonly ApplicationDbContext dbContext;

		public ReportRepository (ApplicationDbContext applicationDbContext)
		{
			dbContext = applicationDbContext;
		}		
    }
}
