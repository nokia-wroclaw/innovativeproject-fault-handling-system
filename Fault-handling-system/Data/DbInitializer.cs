using Fault_handling_system.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Data
{
    public static class DbInitializer
    {
		public static async Task CreateRoles(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			string adminRole = "Admin";
			string requestorRole = "Requestor";
			string nokiaCoordinatorRole = "Nokia Coordinator";
			string subcontractorRole = "Subcontractor";
			//string adminUsername = "admin@example.com";
			string adminEmail = "admin@example.com";
			string adminPassword = "Admin-123";



			if (await roleManager.FindByNameAsync(adminRole) == null)
			{
				await roleManager.CreateAsync(new IdentityRole(adminRole));
			}

			if (await roleManager.FindByNameAsync(requestorRole) == null)
			{
				await roleManager.CreateAsync(new IdentityRole(requestorRole));
			}

			if (await roleManager.FindByNameAsync(nokiaCoordinatorRole) == null)
			{
				await roleManager.CreateAsync(new IdentityRole(nokiaCoordinatorRole));
			}

			if (await roleManager.FindByNameAsync(subcontractorRole) == null)
			{
				await roleManager.CreateAsync(new IdentityRole(subcontractorRole));
			}

			if (await userManager.FindByEmailAsync(adminEmail) == null)
			{
				var adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
				var result = await userManager.CreateAsync(adminUser, adminPassword);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(adminUser, adminRole);
				}
				else System.Diagnostics.Debug.WriteLine("Failed to seed user.");
			}
		}
	}
}
