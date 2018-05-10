using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fault_handling_system.Data;
using Fault_handling_system.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;
using Newtonsoft.Json;
using OfficeOpenXml.Style;

namespace Fault_handling_system.Controllers
{
    /// <summary>
    /// The main controller for reports.
    /// Contains actions for report views that are used to show and manage reports - CRUD operations as well as
    /// sorting and filtering methods.
    /// </summary>
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
		private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        /// <summary>
        /// ReportsController constructor.
        /// </summary>
        /// <param name="hostingEnvironment">Provides application-management functions and application services.</param>
        /// <param name="context">Instance of <c>ApplicationDbContext</c> is responsible for communication with SQL server</param>
        /// <param name="userManager">Instance of <c>UserManager</c> class.</param>
        public ReportsController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
			_userManager = userManager;
        }

        // GET: Reports
        /// <summary>
        /// Action <c>Index</c> can render a view with list of reports from database.
        /// </summary>
        /// <remarks>
        /// This action operates on <c>Report</c> class fields.
        /// <para>If user is assigned to role "Admin" he can see a complete list of reports from database. 
        /// Otherwise, action will only show reports associated with that user.</para>
        /// <para>Reports are selected and sorted using LINQ operators.</para>
        /// </remarks>
        /// <param name="sortOrder">Contains sorting order string for view, e.g. sort by priority ascending.</param>
        /// <param name="etrnumberS">Search string for sorting by EtrNumber value.</param>
        /// <param name="etrnumberC">Current search value for EtrNumber, used to restore previous results for next sorting.</param>
        /// <param name="priorityS">Search string for sorting by Priority value.</param>
        /// <param name="priorityC">Current search value for Priority to restore previous results for next sorting.</param>
        /// <param name="rfaidS">Search string for sorting by RfaId value.</param>
        /// <param name="rfaidC">Current search value for RfaId to restore previous results for next sorting.</param>
        /// <param name="rfanameS">Search string for sorting by RfaName value.</param>
        /// <param name="rfanameC">Current search value for RfaName to restore previous results for next sorting.</param>
        /// <param name="gradeS">Search string for sorting by Grade value.</param>
        /// <param name="gradeC">Current search value for Grade to restore previous results for next sorting.</param>
        /// <param name="troubletypeS">Search string for sorting by TroubleType value.</param>
        /// <param name="troubletypeC">Current search value for TroubleType to restore previous results for next sorting.</param>
        /// <param name="dateissuedfromS">Search string for sorting by DateIssuedFrom value.</param>
        /// <param name="dateissuedtoS">Search string for sorting by DateIssuedTo value.</param>
        /// <param name="dateissuedfromC">Current search value for DateIssuedFrom to restore previous results for next sorting.</param>
        /// <param name="dateissuedtoC">Current search value for DateIssuedTo to restore previous results for next sorting.</param>
        /// <param name="datesentfromS">Search string for sorting by DateSentFrom value.</param>
        /// <param name="datesenttoS">Search string for sorting by DateSentTo value.</param>
        /// <param name="datesentfromC">Current search value for DateSentFrom to restore previous results for next sorting.</param>
        /// <param name="datesenttoC">Current search value for DateSentTo to restore previous results for next sorting.</param>
        /// <param name="etrstatusS">Search string for sorting by EtrStatus value.</param>
        /// <param name="etrstatusC">Current search value for EtrStatus to restore previous results for next sorting.</param>
        /// <param name="etrtypeS">Search string for sorting by EtrType value.</param>
        /// <param name="etrtypeC">Current search value for EtrType to restore previous results for next sorting.</param>
        /// <param name="nsncoordS">Search string for sorting by NsnCoordinator value.</param>
        /// <param name="nsncoordC">Current search value for NsnCoordinator to restore previous results for next sorting.</param>
        /// <param name="subconS">Search string for sorting by Subcontractor value.</param>
        /// <param name="subconC">Current search value for Subcontractor to restore previous results for next sorting.</param>
        /// <param name="zoneS">Search string for sorting by Zone value.</param>
        /// <param name="zoneC">Current search value for Zone to restore previous results for next sorting.</param>
        /// <returns>
        /// ViewResult - list of selected reports
        /// </returns>
        public async Task<IActionResult> Index(string sortOrder, string etrnumberS, string priorityS, string etrnumberC, string priorityC, string rfaidS, string rfaidC, string rfanameS, string rfanameC, string gradeS, string gradeC, string troubletypeS, string troubletypeC, string dateissuedfromS, string dateissuedtoS, string dateissuedfromC, string dateissuedtoC, string datesentfromS, string datesenttoS, string datesentfromC, string datesenttoC, string etrstatusS, string etrstatusC, string etrtypeS, string etrtypeC, string nsncoordS, string nsncoordC, string subconS, string subconC, string zoneS, string zoneC)
        {
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			//lets us see only users of given roles in our dropdowns
			//this is for dropdowns in filter sidebar
			var requestors = await _userManager.GetUsersInRoleAsync("Requestor");
			var nsnCoordinators = await _userManager.GetUsersInRoleAsync("Nokia Coordinator");
			var subcontractors = await _userManager.GetUsersInRoleAsync("Subcontractor");
			var filters = await (from x in _context.ReportFilter
								 where x.UserId.Equals(userId)
								 select x).ToListAsync();
			ReportFilter chosenFilter = null;

			/*foreach (var x in filters)
			{
				if (x.Id == fid)
					chosenFilter = x;
			}*/

			ViewData["EtrStatusFilter"] = new SelectList(_context.EtrStatus, "Status", "Status", null);
			ViewData["EtrTypeFilter"] = new SelectList(_context.EtrType, "Type", "Type", null);
			ViewData["NsnCoordinatorFilter"] = new SelectList(nsnCoordinators, "UserName", "UserName", null);
			ViewData["SubcontractorFilter"] = new SelectList(subcontractors, "UserName", "UserName", null);
			ViewData["ZoneFilter"] = new SelectList(_context.Zone, "ZoneName", "ZoneName", null);
			ViewData["SavedFilters"] = new SelectList(filters, "Id", "Name", null);
			//---
			ViewBag.EtrNumSortParm = sortOrder == "etrnumber_desc" ? "etrnumber" : "etrnumber_desc";
            ViewBag.RfaIdSortParm = sortOrder == "rfaid_desc" ? "rfaid" : "rfaid_desc";
            ViewBag.RfaNameSortParm = sortOrder == "rfaname_desc" ? "rfaname" : "rfaname_desc";
            ViewBag.PrioritySortParm = sortOrder == "priority_desc" ? "priority" : "priority_desc";
            ViewBag.GradeSortParm = sortOrder == "grade_desc" ? "grade" : "grade_desc";
            ViewBag.TroubleTypeSortParm = sortOrder == "troubletype_desc" ? "troubletype" : "troubletype_desc";
            ViewBag.DateIssuedSortParm = sortOrder == "dateissued_desc" ? "dateissued" : "dateissued_desc";
            ViewBag.DateSentSortParm = sortOrder == "datesent_desc" ? "datesent" : "datesent_desc";
            ViewBag.EtrStatusSortParm = sortOrder == "etrstatus_desc" ? "etrstatus" : "etrstatus_desc";
            ViewBag.EtrTypeSortParm = sortOrder == "etrtype_desc" ? "etrtype" : "etrtype_desc";
            ViewBag.NsnCoordSortParm = sortOrder == "nsncoord_desc" ? "nsncoord" : "nsncoord_desc";
            ViewBag.SubconSortParm = sortOrder == "subcon_desc" ? "subcon" : "subcon_desc";
            ViewBag.ZoneSortParm = sortOrder == "zone_desc" ? "zone" : "zone_desc";

            //restore current filters
            if (String.IsNullOrEmpty(etrnumberS))
                etrnumberS = etrnumberC;
            if (String.IsNullOrEmpty(rfaidS))
                    rfaidS = rfaidC;
            if (String.IsNullOrEmpty(rfanameS))
                rfanameS = rfanameC;
            if (String.IsNullOrEmpty(priorityS))
                priorityS = priorityC;
            if (String.IsNullOrEmpty(gradeS))
                gradeS = gradeC;
            if (String.IsNullOrEmpty(troubletypeS))
                troubletypeS = troubletypeC;
            if (String.IsNullOrEmpty(dateissuedfromS))
                dateissuedfromS = dateissuedfromC;
			if (String.IsNullOrEmpty(dateissuedtoS))
				dateissuedtoS = dateissuedtoC;
			if (String.IsNullOrEmpty(datesentfromS))
                datesentfromS = datesentfromC;
			if (String.IsNullOrEmpty(datesenttoS))
				datesenttoS = datesenttoC;
			if (String.IsNullOrEmpty(etrstatusS))
                etrstatusS = etrstatusC;
            if (String.IsNullOrEmpty(etrtypeS))
                etrtypeS = etrtypeC;
            if (String.IsNullOrEmpty(nsncoordS))
                nsncoordS = nsncoordC;
            if (String.IsNullOrEmpty(subconS))
                subconS = subconC;
            if (String.IsNullOrEmpty(zoneS))
                zoneS = zoneC;

            //restore current filters
            ViewBag.etrnumberC = etrnumberS;
            ViewBag.rfaidC = rfaidS;
            ViewBag.rfanameC = rfanameS;
            ViewBag.priorityC = priorityS;
            ViewBag.gradeC = gradeS;
            ViewBag.troubletypeC = troubletypeS;
            ViewBag.dateissuedfromC = dateissuedfromS;
			ViewBag.dateissuedtoC = dateissuedtoS;
			ViewBag.datesentfromC = datesentfromS;
			ViewBag.datesenttoC = datesenttoS;
			ViewBag.etrstatusC = etrstatusS;
            ViewBag.etrtypeC = etrtypeS;
            ViewBag.nsncoordC = nsncoordS;
            ViewBag.subconC = subconS;
            ViewBag.zoneC = zoneS;

			IQueryable<Report> applicationDbContext;

			if (User.IsInRole("Admin"))
			{
				applicationDbContext = _context.Report.Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone).AsQueryable();
			}
			else
			{
				//var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				applicationDbContext = _context.Report.Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone).Where(r => r.NsnCoordinatorId == userId || r.SubcontractorId == userId || r.RequestorId == userId);//added where
			}

			Repositories.ReportRepository reportRepository = new Repositories.ReportRepository();
			if (chosenFilter == null)
				applicationDbContext = reportRepository.GetFilteredReports(applicationDbContext, etrnumberS, priorityS, rfaidS, rfanameS, gradeS, troubletypeS, dateissuedfromS, dateissuedtoS, datesentfromS, datesenttoS, etrstatusS, etrtypeS, nsncoordS, subconS, zoneS);
			else
				applicationDbContext = reportRepository.GetFilteredReports(applicationDbContext, chosenFilter);

            //sorting order
            switch (sortOrder)
            {
                case "etrnumber":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.EtrNumber);
                    break;
                case "etrnumber_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.EtrNumber);
                    break;
                case "rfaid":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.RfaId);
                    break;
                case "rfaid_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.RfaId);
                    break;
                case "rfaname":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.RfaName);
                    break;
                case "rfaname_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.RfaName);
                    break;
                case "priority":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.Priority);
                    break;
                case "priority_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.Priority);
                    break;
                case "grade":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.Grade).Include(r => r.EtrStatus);
                    break;
                case "grade_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.Grade).Include(r => r.EtrStatus);
                    break;
                case "troubletype":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.TroubleType).Include(r => r.EtrStatus);
                    break;
                case "troubletype_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.TroubleType).Include(r => r.EtrStatus);
                    break;
                case "dateissued":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.DateIssued).Include(r => r.EtrStatus).Include(r => r.EtrType);
                    break;
                case "dateissued_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.DateIssued).Include(r => r.EtrStatus);
                    break;
                case "datesent":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.DateSent);
                    break;
                case "datesent_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.DateSent);
                    break;
                case "etrstatus":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.EtrStatus);
                    break;
                case "etrstatus_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.EtrStatus);
                    break;
                case "etrtype":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.EtrType);
                    break;
                case "etrtype_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.EtrType);
                    break;
                case "nsncoord":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.NsnCoordinator);
                    break;
                case "nsncoord_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.NsnCoordinator);
                    break;
                case "subcon":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.Subcontractor);
                    break;
                case "subcon_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.Subcontractor);
                    break;
                case "zone":
                    applicationDbContext = applicationDbContext.OrderBy(r => r.Zone);
                    break;
                case "zone_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(r => r.Zone);
                    break;
            }

			//var filteredReportsJson = JsonConvert.SerializeObject(applicationDbContext.ToList()); //serialize list of filtered reports to json so TempData can handle it
			//TempData["FilteredReports"] = filteredReportsJson;

			TempData["etrnumberS"] = etrnumberS;
			TempData["rfaidS"] = rfaidS;
			TempData["rfanameS"] = rfanameS;
			TempData["priorityS"] = priorityS;
			TempData["gradeS"] = gradeS;
			TempData["troubletypeS"] = troubletypeS;
			TempData["dateissuedfromS"] = dateissuedfromS;
			TempData["dateissuedtoS"] = dateissuedtoS;
			TempData["datesentfromS"] = datesentfromS;
			TempData["datesenttoS"] = datesentfromS;
			TempData["etrstatusS"] = etrstatusS;
			TempData["etrtypeS"] = etrtypeS;
			TempData["nsncoordS"] = nsncoordS;
			TempData["subconS"] = subconS;
			TempData["zoneS"] = zoneS;

			var reports = await applicationDbContext.ToListAsync();
			
			/*var filters = await (from x in _context.ReportFilter
						   where x.User.Equals(User)
						   select x).ToListAsync();*/

			return View(new ReportsAndFiltersViewModel(reports, chosenFilter));
        }

        // GET: Reports/Details/5
        /// <summary>
        /// Action <c>Details</c> render a view containing report details with description.
        /// </summary>
        /// <remarks>
        /// <para>This action creates <c>Report</c> class instance with total description of report selected form database by ID.</para>
        /// </remarks>
        /// <returns>ViewResult - report details</returns>
        /// <param name="id">Report ID in database</param>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.EtrStatus)
                .Include(r => r.EtrType)
                .Include(r => r.NsnCoordinator)
                .Include(r => r.Requestor)
                .Include(r => r.Subcontractor)
                .Include(r => r.Zone)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Reports/Create
        /// <summary>
        /// Contains form and dropdown lists for fields to fill. Used to create new report.
        /// </summary>
        /// <returns>ViewResult with create report form.</returns>
        public async Task<IActionResult> Create()
        {
			//lets us see only users of given roles in our dropdowns
			var requestors = await _userManager.GetUsersInRoleAsync("Requestor");
			var nsnCoordinators = await _userManager.GetUsersInRoleAsync("Nokia Coordinator");
			var subcontractors = await _userManager.GetUsersInRoleAsync("Subcontractor");

			ViewData["EtrStatusId"] = new SelectList(_context.EtrStatus, "Id", "Status");
            ViewData["EtrTypeId"] = new SelectList(_context.EtrType, "Id", "Type");
            ViewData["NsnCoordinatorId"] = new SelectList(nsnCoordinators, "Id", "UserName");
            ViewData["RequestorId"] = new SelectList(requestors, "Id", "UserName");
            ViewData["SubcontractorId"] = new SelectList(subcontractors, "Id", "UserName");
            ViewData["ZoneId"] = new SelectList(_context.Zone, "Id", "ZoneName");
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// This action accepts user input and posts those input to the server to add new report to the database.
        /// </summary>
        /// <param name="report">Values of report fields</param>
        /// <returns>ViewResult and created report</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EtrNumber,NokiaCaseId,RfaId,RfaName,ZoneId,AssignedTo,Priority,EtrTypeId,EtrStatusId,RequestorId,NsnCoordinatorId,SubcontractorId,Grade,TroubleType,DateIssued,DateSent,EtrToDes,ClosingDate,EtrDescription,Comment")] Report report)
        {
			var existing = await (from x in _context.Report
								  where x.EtrNumber.Equals(report.EtrNumber)
								  select x).SingleOrDefaultAsync();
			if (existing != null) ModelState.AddModelError("EtrNumber", "A report with given Etr Number already exists.");

			if (ModelState.IsValid)
            {
				_context.Add(report);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
            }
            ViewData["EtrStatusId"] = new SelectList(_context.EtrStatus, "Id", "Status", report.EtrStatusId);
            ViewData["EtrTypeId"] = new SelectList(_context.EtrType, "Id", "Type", report.EtrTypeId);
            ViewData["NsnCoordinatorId"] = new SelectList(_context.Users, "Id", "UserName", report.NsnCoordinatorId);
            ViewData["RequestorId"] = new SelectList(_context.Users, "Id", "UserName", report.RequestorId);
            ViewData["SubcontractorId"] = new SelectList(_context.Users, "Id", "UserName", report.SubcontractorId);
            ViewData["ZoneId"] = new SelectList(_context.Zone, "Id", "ZoneName", report.ZoneId);
            return View(report);
        }

        // GET: Reports/Edit/5
        /// <summary>
        /// Has fields to fill by user for selected report used to edit existing report.
        /// </summary>
        /// <param name="id">Report ID</param>
        /// <returns>ViewResult with report to edit</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report.SingleOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

			//lets us see only users of given roles in our dropdowns
			var requestors = await _userManager.GetUsersInRoleAsync("Requestor");
			var nsnCoordinators = await _userManager.GetUsersInRoleAsync("Nokia Coordinator");
			var subcontractors = await _userManager.GetUsersInRoleAsync("Subcontractor");

			ViewData["EtrStatusId"] = new SelectList(_context.EtrStatus, "Id", "Status", report.EtrStatusId);
            ViewData["EtrTypeId"] = new SelectList(_context.EtrType, "Id", "Type", report.EtrTypeId);
            ViewData["NsnCoordinatorId"] = new SelectList(nsnCoordinators, "Id", "UserName", report.NsnCoordinatorId);
            ViewData["RequestorId"] = new SelectList(requestors, "Id", "UserName", report.RequestorId);
            ViewData["SubcontractorId"] = new SelectList(subcontractors, "Id", "UserName", report.SubcontractorId);
            ViewData["ZoneId"] = new SelectList(_context.Zone, "Id", "ZoneName", report.ZoneId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// This action accepts user input and posts those input to the server to update report in database.
        /// </summary>
        /// <param name="id">ID of edited report </param>
        /// <param name="report">Values of report fields</param>
        /// <returns>View with updated report</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EtrNumber,NokiaCaseId,RfaId,RfaName,ZoneId,AssignedTo,Priority,EtrTypeId,EtrStatusId,RequestorId,NsnCoordinatorId,SubcontractorId,Grade,TroubleType,DateIssued,DateSent,EtrToDes,ClosingDate,EtrDescription,Comment")] Report report)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EtrStatusId"] = new SelectList(_context.EtrStatus, "Id", "Status", report.EtrStatusId);
            ViewData["EtrTypeId"] = new SelectList(_context.EtrType, "Id", "Type", report.EtrTypeId);
            ViewData["NsnCoordinatorId"] = new SelectList(_context.Users, "Id", "UserName", report.NsnCoordinatorId);
            ViewData["RequestorId"] = new SelectList(_context.Users, "Id", "UserName", report.RequestorId);
            ViewData["SubcontractorId"] = new SelectList(_context.Users, "Id", "UserName", report.SubcontractorId);
            ViewData["ZoneId"] = new SelectList(_context.Zone, "Id", "ZoneName", report.ZoneId);
            return View(report);
        }

        /// <summary>
        /// This action allows user with "Admin" role to choose Nokia Coordinator for selected report
        /// </summary>
        /// <param name="id">Nokia Coordinator Id</param>
        /// <returns>View with assign select list</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assign(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nsnCoordinators = await _userManager.GetUsersInRoleAsync("Nokia Coordinator");
            ViewData["NsnCoordinatorId"] = new SelectList(nsnCoordinators, "Id", "UserName");

            var report = await _context.Report
                .Include(r => r.EtrStatus)
                .Include(r => r.EtrType)
                .Include(r => r.NsnCoordinator)
                .Include(r => r.Requestor)
                .Include(r => r.Subcontractor)
                .Include(r => r.Zone)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }
        
        /// <summary>
        /// Assigns Nokia Coordinator to selected report and saves changes in database.
        /// </summary>
        /// <param name="id">Nokia Coordinator ID</param>
        /// <returns>Redirects to <c>UserPage</c> action of <c>AccountController</c>.</returns>
        [HttpPost]
        [Authorize(Roles="Admin")]
        public RedirectToActionResult Assign(int id)
        {
            string nsnCoordinator = Request.Form["NsnCoordinatorId"];
            
            if(nsnCoordinator != null)
            {
                var report = new Report();
                report.Id = id;
                report.NsnCoordinatorId = nsnCoordinator;
                _context.Entry(report).Property("NsnCoordinatorId").IsModified = true;
                _context.SaveChanges();
            }
            
            return RedirectToAction("UserPage", "Account"); 
        }


        // GET: Reports/Delete/5
        /// <summary>
        /// Allows user to delete selected report.
        /// </summary>
        /// <param name="id">Report ID</param>
        /// <returns>View with report to remove.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.EtrStatus)
                .Include(r => r.EtrType)
                .Include(r => r.NsnCoordinator)
                .Include(r => r.Requestor)
                .Include(r => r.Subcontractor)
                .Include(r => r.Zone)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        /// <summary>
        /// Removes report (selected by id) from database.
        /// </summary>
        /// <param name="id">Report ID</param>
        /// <returns>Redirects to <c>Index</c> action.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Report.SingleOrDefaultAsync(m => m.Id == id);
            _context.Report.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if report with selected <paramref name="id"/> and returns result.
        /// </summary>
        /// <param name="id">Report ID to find</param>
        /// <returns>True or false</returns>
        private bool ReportExists(int id)
        {
            return _context.Report.Any(e => e.Id == id);
        }
        
		[HttpPost]
		public async Task<IActionResult> SaveFilter(/*[Bind("Id,UserId,Name,EtrNumber,Priority,RfaId,RfaName,Grade,TroubleType,DateIssuedFrom,DateIssuedTo,DateIssuedFromWeeksAgo,DateIssuedFromDaysAgo,DateIssuedToWeeksAgo,DateIssuedToDaysAgo,DateSentFrom,DateSentTo,DateSentFromWeeksAgo,DateSentFromDaysAgo,DateSentToWeeksAgo,DateSentToDaysAgo,EtrStatus,EtrType,NsnCoordinatorId,SubcontractorId,Zone")] ReportFilter reportFilter*/string filterName, string etrnumberS, string priorityS, string rfaidS, string rfanameS, string gradeS, string troubletypeS, string dateissuedfromS, string dateissuedtoS, string dateissuedfromWeeksS, string dateissuedfromDaysS, string dateissuedtoWeeksS, string dateissuedtoDaysS, string datesentfromS, string datesenttoS, string datesentfromWeeksS, string datesentfromDaysS, string datesenttoWeeksS, string datesenttoDaysS, string etrstatusS, string etrtypeS, string nsncoordS, string subconS, string zoneS)
		{
			/*if (ModelState.IsValid)
			{
				_context.Add(reportFilter);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return RedirectToAction(nameof(Index));*/

			int diFromWeeksTemp, diFromDaysTemp, diToWeeksTemp, diToDaysTemp;
			int dsFromWeeksTemp, dsFromDaysTemp, dsToWeeksTemp, dsToDaysTemp;
			int? diFromWeeks, diFromDays, diToWeeks, diToDays;
			int? dsFromWeeks, dsFromDays, dsToWeeks, dsToDays;

			diFromWeeks = int.TryParse(dateissuedfromWeeksS, out diFromWeeksTemp) ? diFromWeeksTemp : (int?)null;
			diFromDays = int.TryParse(dateissuedfromDaysS, out diFromDaysTemp) ? diFromDaysTemp : (int?)null;
			diToWeeks = int.TryParse(dateissuedtoWeeksS, out diToWeeksTemp) ? diToWeeksTemp : (int?)null;
			diToDays = int.TryParse(dateissuedtoDaysS, out diToDaysTemp) ? diToDaysTemp : (int?)null;
			dsFromWeeks = int.TryParse(datesentfromWeeksS, out dsFromWeeksTemp) ? dsFromWeeksTemp : (int?)null;
			dsFromDays = int.TryParse(datesentfromDaysS, out dsFromDaysTemp) ? dsFromDaysTemp : (int?)null;
			dsToWeeks = int.TryParse(datesenttoWeeksS, out dsToWeeksTemp) ? dsToWeeksTemp : (int?)null;
			dsToDays = int.TryParse(datesenttoDaysS, out dsToDaysTemp) ? dsToDaysTemp : (int?)null;

			if (diFromWeeks != null || diFromDays != null) dateissuedfromS = null;
			if (diToWeeks != null || diToDays != null) dateissuedtoS = null;
			if (dsFromWeeks != null || dsFromDays != null) datesentfromS = null;
			if (dsToWeeks != null || dsToDays != null) datesenttoS = null;

			ReportFilter reportFilter = new ReportFilter
			{
				UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
				Name = filterName,
				EtrNumber = etrnumberS,
				Priority = priorityS,
				RfaId = rfaidS,
				RfaName = rfanameS,
				Grade = gradeS,
				TroubleType = troubletypeS,
				DateIssuedFrom = dateissuedfromS,
				DateIssuedTo = dateissuedtoS,
				DateIssuedFromWeeksAgo = diFromWeeks,
				DateIssuedFromDaysAgo = diFromDays,
				DateIssuedToWeeksAgo = diToWeeks,
				DateIssuedToDaysAgo = diToDays,
				DateSentFrom = datesentfromS,
				DateSentTo = datesenttoS,
				DateSentFromWeeksAgo = dsFromWeeks,
				DateSentFromDaysAgo = dsFromDays,
				DateSentToWeeksAgo = dsToWeeks,
				DateSentToDaysAgo = dsToDays,
				EtrStatus = etrstatusS,
				EtrType = etrtypeS,
				NsnCoordinatorId = nsncoordS,
				SubcontractorId = subconS,
				Zone = zoneS
			};

			//bool isValid = TryValidateModel(reportFilter);

			if (TryValidateModel(reportFilter))
			{
				var existingFilter = await (from x in _context.ReportFilter
									 where x.UserId.Equals(reportFilter.UserId)
									 && x.Name.Equals(reportFilter.Name)
									 select x).FirstOrDefaultAsync();
				if (existingFilter != null)
				{
					existingFilter.EtrNumber = reportFilter.EtrNumber;
					existingFilter.Priority = reportFilter.Priority;
					existingFilter.RfaId = reportFilter.RfaId;
					existingFilter.RfaName = reportFilter.RfaName;
					existingFilter.Grade = reportFilter.Grade;
					existingFilter.TroubleType = reportFilter.TroubleType;
					existingFilter.DateIssuedFrom = reportFilter.DateIssuedFrom;
					existingFilter.DateIssuedTo = reportFilter.DateIssuedTo;
					existingFilter.DateIssuedFromWeeksAgo = reportFilter.DateIssuedFromWeeksAgo;
					existingFilter.DateIssuedFromDaysAgo = reportFilter.DateIssuedFromDaysAgo;
					existingFilter.DateIssuedToWeeksAgo = reportFilter.DateIssuedToWeeksAgo;
					existingFilter.DateIssuedToDaysAgo = reportFilter.DateIssuedToDaysAgo;
					existingFilter.DateSentFrom = reportFilter.DateSentFrom;
					existingFilter.DateSentTo = reportFilter.DateSentTo;
					existingFilter.DateSentFromWeeksAgo = reportFilter.DateSentFromWeeksAgo;
					existingFilter.DateSentFromDaysAgo = reportFilter.DateSentFromDaysAgo;
					existingFilter.DateSentToWeeksAgo = reportFilter.DateSentToWeeksAgo;
					existingFilter.DateSentToDaysAgo = reportFilter.DateSentToDaysAgo;
					existingFilter.EtrStatus = reportFilter.EtrStatus;
					existingFilter.EtrType = reportFilter.EtrType;
					existingFilter.NsnCoordinatorId = reportFilter.NsnCoordinatorId;
					existingFilter.SubcontractorId = reportFilter.SubcontractorId;
					existingFilter.Zone = reportFilter.Zone;

					_context.Update(existingFilter);
					await _context.SaveChangesAsync();
				}
				else
				{
					_context.Add(reportFilter);
					await _context.SaveChangesAsync();
				}
				//return RedirectToAction(nameof(Index));
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<JsonResult> GetFilter(int? id)
		{
			//string toReturn = null;
			ReportFilter filter;
			if (id == null)
			{
				filter = new ReportFilter();
				//toReturn = JsonConvert.SerializeObject(filter);
			}
			else
			{
				filter = await (from x in _context.ReportFilter
									  where x.Id == id
									  select x).SingleOrDefaultAsync();
				//toReturn = JsonConvert.SerializeObject(filter);
			}

			return Json(filter);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteFilter(string filterName)
		{
			if (String.IsNullOrEmpty(filterName) || String.IsNullOrWhiteSpace(filterName))
			{
				return NotFound("404: there's no filter with name " + filterName);
			}

			var toDelete = await (from x in _context.ReportFilter
										where x.Name.Equals(filterName) && x.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))
										select x).SingleOrDefaultAsync();

			if (toDelete == null)
			{
				return NotFound("404: there's no filter with name " + filterName);
			}

			_context.ReportFilter.Remove(toDelete);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

        /// <summary>
        /// This action can export reports to excel file.
        /// </summary>
        /// <remarks>
        /// Method uses field names from <c>applicationDbContext</c> to name worksheet columns and iterate 
        /// selected reports collection from context to save them to the excel file.
        /// </remarks>
        /// <returns>Report.xlsx file with reports summary.</returns>
        [HttpPost]
        [Route("Export")]
        public FileContentResult Export()
        {
            const string DateFormat = "dd/MM/yyyy";
            byte[] bytes;
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Report.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }

			IQueryable<Report> applicationDbContext;

			if (User.IsInRole("Admin"))
			{
				applicationDbContext = _context.Report.Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone).AsQueryable();
			}
			else
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				applicationDbContext = _context.Report.Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone).Where(r => r.NsnCoordinatorId == userId || r.SubcontractorId == userId || r.RequestorId == userId);//added where
			}

			Repositories.ReportRepository reportRepository = new Repositories.ReportRepository();

			applicationDbContext = reportRepository.GetFilteredReports(applicationDbContext, TempData["etrnumberS"] as string, TempData["priorityS"] as string, TempData["rfaidS"] as string, TempData["rfanameS"] as string, TempData["gradeS"] as string, TempData["troubletypeS"] as string, TempData["dateissuedfromS"] as string, TempData["dateissuedtoS"] as string, TempData["datesentfromS"] as string, TempData["datesenttoS"] as string, TempData["etrstatusS"] as string, TempData["etrtypeS"] as string, TempData["nsncoordS"] as string, TempData["subconS"] as string, TempData["zoneS"] as string);
			if (applicationDbContext != null)
			{
				System.Diagnostics.Debug.WriteLine("!!!Załadowano zgłoszenia!!!");
			}
            //-------------------------------

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");
                int line = 1;
                //Add headers
                worksheet.Cells[line, 1].Value = "EtrNumber";
                worksheet.Cells[line, 2].Value = "NokiaCaseId";
                worksheet.Cells[line, 3].Value = "RfaId";
                worksheet.Cells[line, 4].Value = "RfaName";
                worksheet.Cells[line, 5].Value = "AssignedTo";
                worksheet.Cells[line, 6].Value = "Priority";
                worksheet.Cells[line, 7].Value = "Grade";
                worksheet.Cells[line, 8].Value = "TroubleType";
                worksheet.Cells[line, 9].Value = "DateIssued";                               
                worksheet.Cells[line, 10].Value = "DateSent";               
                worksheet.Cells[line, 11].Value = "EtrToDes";
                worksheet.Cells[line, 12].Value = "ClosingDate";            
                worksheet.Cells[line, 13].Value = "EtrDescription";
                worksheet.Cells[line, 14].Value = "Comment";
                worksheet.Cells[line, 15].Value = "EtrStatus";
                worksheet.Cells[line, 16].Value = "EtrType";
                worksheet.Cells[line, 17].Value = "NsnCoordinator";
                worksheet.Cells[line, 18].Value = "Requestor";
                worksheet.Cells[line, 19].Value = "Subcontractor";
                worksheet.Cells[line, 20].Value = "Zone";
                line++;
                //Add lines
                foreach (Report report in applicationDbContext)
                {
                    worksheet.Cells[line, 1].Value = report.EtrNumber;
                    worksheet.Cells[line, 2].Value = report.NokiaCaseId;
                    worksheet.Cells[line, 3].Value = report.RfaId;
                    worksheet.Cells[line, 4].Value = report.RfaName;
                    worksheet.Cells[line, 5].Value = report.AssignedTo;
                    worksheet.Cells[line, 6].Value = report.Priority;
                    worksheet.Cells[line, 7].Value = report.Grade;
                    worksheet.Cells[line, 8].Value = report.TroubleType;
                    worksheet.Cells[line, 9].Value = report.DateIssued;
                    worksheet.Cells[line, 9].Style.Numberformat.Format = DateFormat;

                    worksheet.Cells[line, 10].Value = report.DateSent;
                    worksheet.Cells[line, 10].Style.Numberformat.Format = DateFormat;

                    worksheet.Cells[line, 11].Value = report.EtrToDes;
                    worksheet.Cells[line, 12].Value = report.ClosingDate;
                    worksheet.Cells[line, 12].Style.Numberformat.Format = DateFormat;

                    worksheet.Cells[line, 13].Value = report.EtrDescription;
                    worksheet.Cells[line, 14].Value = report.Comment;
                    worksheet.Cells[line, 15].Value = report.EtrStatus.Status;
                    worksheet.Cells[line, 16].Value = report.EtrType.Type;
                    worksheet.Cells[line, 17].Value = report.NsnCoordinator;
                    worksheet.Cells[line, 18].Value = report.Requestor;
                    worksheet.Cells[line, 19].Value = report.Subcontractor;
                    worksheet.Cells[line, 20].Value = report.Zone.ZoneName;
                    line++;
                }
                
                bytes = package.GetAsByteArray();
            }
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}
