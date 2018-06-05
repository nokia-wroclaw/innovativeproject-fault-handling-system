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
		/// 
		/// </summary>
		/// <param name="applicationDbContext">ApplicationDbContext used to connect with db.</param>
		/// <param name="etrnumberS">Etr Number.</param>
		/// <param name="priorityS">Priority.</param>
		/// <param name="rfaidS">Rfa Id.</param>
		/// <param name="rfanameS">Rfa Name.</param>
		/// <param name="gradeS">Grade.</param>
		/// <param name="troubletypeS">Trouble Type.</param>
		/// <param name="dateissuedfromS">Date Issued From.</param>
		/// <param name="dateissuedtoS">Date Issued To.</param>
		/// <param name="datesentfromS">Date Sent From.</param>
		/// <param name="datesenttoS">Date Sent To.</param>
		/// <param name="etrstatusS">Etr Status.</param>
		/// <param name="etrtypeS">Etr Type.</param>
		/// <param name="nsncoordS">Nsn Coordinator name.</param>
		/// <param name="subconS">Subcontractor name.</param>
		/// <param name="zoneS">Zone name.</param>
		/// <returns>Reports with filters applied</returns>
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
