using Fault_handling_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fault_handling_system.Repositories
{
    public interface IReportRepository
    {
		IQueryable<Report> GetFilteredReports(IQueryable<Report> applicationDbContext, string etrnumberS, string priorityS, string rfaidS, string rfanameS, string gradeS, string troubletypeS, string dateissuedfromS, string dateissuedtoS, string datesentfromS, string datesenttoS, string etrstatusS, string etrtypeS, string nsncoordS, string subconS, string zoneS);
		Task<Report> GetReportById(int? id);
		Task<Report> GetReportByIdWithNavigationProperties(int? id);
		Task<Report> GetReportByEtrNumber(string etrNumber);
		IQueryable<Report> GetAllReportsWithNavigationProperties();
		IQueryable<Report> GetReportsWhereInvolved(string userId);
	}
}
