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
		//private readonly ApplicationDbContext dbContext;

		public ReportRepository(/*ApplicationDbContext applicationDbContext*/)
		{
			//dbContext = applicationDbContext;
		}

		public IQueryable<Report> GetFilteredReports (IQueryable<Report> applicationDbContext, string etrnumberS, string priorityS, string rfaidS, string rfanameS, string gradeS, string troubletypeS, string dateissuedfromS, string dateissuedtoS, string datesentfromS, string datesenttoS, string etrstatusS, string etrtypeS, string nsncoordS, string subconS, string zoneS)
		{
			if (!String.IsNullOrEmpty(etrnumberS))
				applicationDbContext = applicationDbContext.Where(r => r.EtrNumber.Contains(etrnumberS));
			if (!String.IsNullOrEmpty(rfaidS))
				applicationDbContext = applicationDbContext.Where(r => r.RfaId.ToString().Contains(rfaidS));
			if (!String.IsNullOrEmpty(rfanameS))
				applicationDbContext = applicationDbContext.Where(r => r.RfaName.Contains(rfanameS));
			if (!String.IsNullOrEmpty(priorityS))
				applicationDbContext = applicationDbContext.Where(r => r.Priority.Contains(priorityS));
			if (!String.IsNullOrEmpty(gradeS))
				applicationDbContext = applicationDbContext.Where(r => r.Grade.ToString().Contains(gradeS));
			if (!String.IsNullOrEmpty(troubletypeS))
				applicationDbContext = applicationDbContext.Where(r => r.TroubleType.Contains(troubletypeS));
			if (!String.IsNullOrEmpty(dateissuedfromS))
			{
				DateTime dateIssuedFrom = DateTime.ParseExact(dateissuedfromS, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
				applicationDbContext = applicationDbContext.Where(r => r.DateIssued >= dateIssuedFrom);
			}
			if (!String.IsNullOrEmpty(dateissuedtoS))
			{
				DateTime dateIssuedTo = DateTime.ParseExact(dateissuedtoS, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
				applicationDbContext = applicationDbContext.Where(r => r.DateIssued <= dateIssuedTo);
			}
			if (!String.IsNullOrEmpty(datesentfromS))
			{
				DateTime dateSentFrom = DateTime.ParseExact(dateissuedtoS, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
				applicationDbContext = applicationDbContext.Where(r => r.DateSent >= dateSentFrom);
			}
			if (!String.IsNullOrEmpty(datesenttoS))
			{
				DateTime dateSentTo = DateTime.ParseExact(dateissuedtoS, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
				applicationDbContext = applicationDbContext.Where(r => r.DateSent <= dateSentTo);
			}
			if (!String.IsNullOrEmpty(etrstatusS))
				applicationDbContext = applicationDbContext.Where(r => r.EtrStatus.Status.Contains(etrstatusS));
			if (!String.IsNullOrEmpty(etrtypeS))
				applicationDbContext = applicationDbContext.Where(r => r.EtrType.Type.Contains(etrtypeS));
			if (!String.IsNullOrEmpty(nsncoordS))
				applicationDbContext = applicationDbContext.Where(r => r.NsnCoordinator.UserName.Contains(nsncoordS));
			if (!String.IsNullOrEmpty(subconS))
				applicationDbContext = applicationDbContext.Where(r => r.Subcontractor.UserName.Contains(subconS));
			if (!String.IsNullOrEmpty(zoneS))
				applicationDbContext = applicationDbContext.Where(r => r.Zone.ZoneName.Contains(zoneS));

			return applicationDbContext;
		}
    }
}
