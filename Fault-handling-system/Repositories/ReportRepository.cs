using Fault_handling_system.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fault_handling_system.Models;

namespace Fault_handling_system.Repositories
{
	/// <summary>
	/// Class performing operations on data.
	/// Mainly used when the action performed is more complicated
	/// than simple create, select, update, delete actions.
	/// </summary>
	public class ReportRepository
	{
		//private readonly ApplicationDbContext dbContext;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ReportRepository(/*ApplicationDbContext applicationDbContext*/)
		{
			//dbContext = applicationDbContext;
		}
		/// <summary>
		/// Gets collection of reports, filters the reports with given
		/// parameters and returns reports with filters applied.
		/// </summary>
		/// <param name="applicationDbContext">Reports</param>
		/// <param name="etrnumberS">EtrNumber filter</param>
		/// <param name="priorityS">Priority filter</param>
		/// <param name="rfaidS">RfaId filter</param>
		/// <param name="rfanameS">RfaName filter</param>
		/// <param name="gradeS">Grade filter</param>
		/// <param name="troubletypeS">TroubleType filter</param>
		/// <param name="dateissuedfromS">DateIssued starting date</param>
		/// <param name="dateissuedtoS">DateIssued ending date</param>
		/// <param name="datesentfromS">DateSent starting date</param>
		/// <param name="datesenttoS">DateSent ending date</param>
		/// <param name="etrstatusS">EtrStatus filter</param>
		/// <param name="etrtypeS">EtrType filter</param>
		/// <param name="nsncoordS">NsnCoordinator filter (their UserName)</param>
		/// <param name="subconS">Subcontractor filter (their UserName)</param>
		/// <param name="zoneS">Zone filter</param>
		/// <returns>Reports with filters applied.</returns>
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
