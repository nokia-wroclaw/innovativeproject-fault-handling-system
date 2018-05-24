using Fault_handling_system.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Data
{
	/// <summary>
	/// Static class used to seed exemplary data to database.
	/// </summary>
	public static class DbInitializer
    {
		/// <summary>
		/// Seeds user roles to database and an exemplary admin
		/// provided they don't already exist.
		/// If they do the method doesn't do anything.
		/// Runs asynchronously.
		/// </summary>
		/// <remarks>Primarily used for development purposes so there's always some admin account.</remarks>
		/// <param name="userManager">Application's UserManager service</param>
		/// <param name="roleManager">Application's RoleManager service</param>
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

			var admins = await userManager.GetUsersInRoleAsync("Admin");

			if (admins.Count <= 0)
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

		public static async Task CreateEtrAttributes(ApplicationDbContext applicationDbContext)
		{
			var etrTypes = await (from x in applicationDbContext.EtrType
								  select x).ToListAsync();
			var etrStatuses = await (from x in applicationDbContext.EtrStatus
									 select x).ToListAsync();
			var zones = await (from x in applicationDbContext.Zone
							   select x).ToListAsync();
			List<EtrType> typesToAdd = new List<EtrType>();
			List<EtrStatus> statusesToAdd = new List<EtrStatus>();
			List<Zone> zonesToAdd = new List<Zone>();

			if (etrTypes.Find(x => x.Type.Equals("BSS")) == null)
				typesToAdd.Add(new EtrType() { Type = "BSS" });
			if (etrTypes.Find(x => x.Type.Equals("HW")) == null)
				typesToAdd.Add(new EtrType() { Type = "HW" });
			if (etrTypes.Find(x => x.Type.Equals("O_BENNING")) == null)
				typesToAdd.Add(new EtrType() { Type = "O_BENNING" });
			if (etrTypes.Find(x => x.Type.Equals("O_E///")) == null)
				typesToAdd.Add(new EtrType() { Type = "O_E///" });
			if (etrTypes.Find(x => x.Type.Equals("O_PKL")) == null)
				typesToAdd.Add(new EtrType() { Type = "O_PKL" });
			if (etrTypes.Find(x => x.Type.Equals("O_TK")) == null)
				typesToAdd.Add(new EtrType() { Type = "O_TK" });
			if (etrTypes.Find(x => x.Type.Equals("PRZYPISAC")) == null)
				typesToAdd.Add(new EtrType() { Type = "PRZYPISAC" });
			if (etrTypes.Find(x => x.Type.Equals("SUB")) == null)
				typesToAdd.Add(new EtrType() { Type = "SUB" });

			if (etrStatuses.Find(x => x.Status.Equals("Closed")) == null)
				statusesToAdd.Add(new EtrStatus() { Status = "Closed" });
			if (etrStatuses.Find(x => x.Status.Equals("In Realization")) == null)
				statusesToAdd.Add(new EtrStatus() { Status = "In Realization" });
			if (etrStatuses.Find(x => x.Status.Equals("Withdraw")) == null)
				statusesToAdd.Add(new EtrStatus() { Status = "Withdraw" });

			if (zones.Find(x => x.ZoneName.Equals("Warszawa")) == null)
				zonesToAdd.Add(new Zone() { ZoneName = "Warszawa" });
			if (zones.Find(x => x.ZoneName.Equals("Katowice")) == null)
				zonesToAdd.Add(new Zone() { ZoneName = "Katowice" });
			if (zones.Find(x => x.ZoneName.Equals("Poznan")) == null)
				zonesToAdd.Add(new Zone() { ZoneName = "Poznan" });
			if (zones.Find(x => x.ZoneName.Equals("Gdansk")) == null)
				zonesToAdd.Add(new Zone() { ZoneName = "Gdansk" });
			if (zones.Find(x => x.ZoneName.Equals("Unknown")) == null)
				zonesToAdd.Add(new Zone() { ZoneName = "Unknown" });

			if (typesToAdd.Count > 0)
			{
				applicationDbContext.AddRange(typesToAdd);
				await applicationDbContext.SaveChangesAsync();
			}
			if (statusesToAdd.Count > 0)
			{
				applicationDbContext.AddRange(statusesToAdd);
				await applicationDbContext.SaveChangesAsync();
			}
			if (zonesToAdd.Count > 0)
			{
				applicationDbContext.AddRange(zonesToAdd);
				await applicationDbContext.SaveChangesAsync();
			}
		}
	}
}
