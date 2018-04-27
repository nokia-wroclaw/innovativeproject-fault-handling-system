using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fault_handling_system.Models;

namespace Fault_handling_system.Data
{
	/// <summary>
	/// A class that represents a session with a database
	/// and provides API for communicating with a database
	/// allowing getting, manipulating and deleting data from it.
	/// </summary>
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
		/// <summary>
		/// Constructor that allows to initialize object with particular options.
		/// </summary>
		/// <param name="options">Options to be used by context.</param>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

		/// <summary>
		/// Configures database model schema needed for entity framework.
		/// </summary>
		/// <param name="builder">The builder used to construct the model.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
			builder.Entity<ApplicationUser>().HasIndex(x => x.Email).IsUnique();
			builder.Entity<Report>().HasIndex(x => x.EtrNumber).IsUnique();
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

		/// <value>Represents Report table from db.</value>
		public DbSet<Report> Report { get; set; }
		/// <value>Represents Zone table from db.</value>
		public DbSet<Zone> Zone { get; set; }
		/// <value>Represents EtrType table from db.</value>
		public DbSet<EtrType> EtrType { get; set; }
		/// <value>Represents EtrStatus table from db.</value>
		public DbSet<EtrStatus> EtrStatus { get; set; }
		public DbSet<ReportFilter> ReportFilter { get; set; }
	}
}
