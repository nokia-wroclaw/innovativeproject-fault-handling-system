using Fault_handling_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Repositories
{
	/// <summary>
	/// Interface for ReportRepository
	/// </summary>
	public interface IReportRepository
    {
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
		IQueryable<Report> GetFilteredReports(IQueryable<Report> applicationDbContext, string etrnumberS, string priorityS, string rfaidS, string rfanameS, string gradeS, string troubletypeS, string dateissuedfromS, string dateissuedtoS, string datesentfromS, string datesenttoS, string etrstatusS, string etrtypeS, string nsncoordS, string subconS, string zoneS);
		/// <summary>
		/// Gets report by its id.
		/// </summary>
		/// <param name="id">Report's id.</param>
		/// <returns>Report object or null if not found.</returns>
		Task<Report> GetReportById(int? id);
		/// <summary>
		/// Gets report by its id with entities referenced by foreign keys.
		/// </summary>
		/// <param name="id">Report's id.</param>
		/// <returns>Report object or null if not found.</returns>
		Task<Report> GetReportByIdWithNavigationProperties(int? id);
		/// <summary>
		/// Gets report by its EtrNumber property's value.
		/// </summary>
		/// <param name="etrNumber">Report's EtrNumber property value.</param>
		/// <returns>Report object or null if not found.</returns>
		Task<Report> GetReportByEtrNumber(string etrNumber);
		/// <summary>
		/// Gets all reports with entities referenced by navigation properties.
		/// </summary>
		/// <returns>All reports from database.</returns>
		IQueryable<Report> GetAllReportsWithNavigationProperties();
		/// <summary>
		/// Gets all reports where passed userId is present in Requestor, NsnCoordinator or Subcontractor
		/// properties.
		/// </summary>
		/// <param name="userId">User's id.</param>
		/// <returns>Reports where given user is present in Requestor, NsnCoordinator or Subcontractor
		/// properties.</returns>
		IQueryable<Report> GetReportsWhereInvolved(string userId);
	}
}
