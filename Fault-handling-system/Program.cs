using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fault_handling_system.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fault_handling_system
{
    /// <summary>
    /// The main class containing an entry point to the application. 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method being an entry point to the application.
        /// Provides services to administer users and roles.
        /// Invokes DbInitializer CreateRoles method in order to create roles
        /// and admin user if necessary. Builds host and runs the application.
        /// </summary>
        /// <param name="args">String array that represents the command-line arguments</param>
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				Data.DbInitializer.CreateRoles(userManager, roleManager).Wait();				
			}
			host.Run();
		}

        /// <summary>
        /// Creates a web application host.
        /// Invokes CreateDeafultBuilder method which builds the host.
        /// Defines the startup class.  
        /// </summary>
        /// <returns>
        /// Returns configured web host  
        /// </returns>
        /// <param name="args">String array that represents the command-line arguments</param>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
