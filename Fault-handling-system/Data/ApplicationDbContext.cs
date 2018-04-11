using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fault_handling_system.Models;

namespace Fault_handling_system.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

		public DbSet<Report> Report { get; set; }
		public DbSet<Zone> Zone { get; set; }
		public DbSet<EtrType> EtrType { get; set; }
		public DbSet<EtrStatus> EtrStatus { get; set; }
		//public DbSet<ReportFilter> ReportFilter { get; set; }
	}
}
