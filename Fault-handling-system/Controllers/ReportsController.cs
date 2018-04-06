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
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
		private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public ReportsController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
			_userManager = userManager;
        }

        // GET: Reports
        public async Task<IActionResult> Index(string sortOrder, string etrnumberS, string priorityS, string etrnumberC, string priorityC, string rfaidS, string rfaidC, string rfanameS, string rfanameC, string gradeS, string gradeC, string troubletypeS, string troubletypeC, string dateissuedfromS, string dateissuedtoS, string dateissuedfromC, string dateissuedtoC, string datesentfromS, string datesenttoS, string datesentfromC, string datesenttoC, string etrstatusS, string etrstatusC, string etrtypeS, string etrtypeC, string nsncoordS, string nsncoordC, string subconS, string subconC, string zoneS, string zoneC)
        {
			//lets us see only users of given roles in our dropdowns
			//this is for dropdowns in filter sidebar
			var requestors = await _userManager.GetUsersInRoleAsync("Requestor");
			var nsnCoordinators = await _userManager.GetUsersInRoleAsync("Nokia Coordinator");
			var subcontractors = await _userManager.GetUsersInRoleAsync("Subcontractor");

			ViewData["EtrStatusFilter"] = new SelectList(_context.EtrStatus, "Status", "Status", null);
			ViewData["EtrTypeFilter"] = new SelectList(_context.EtrType, "Type", "Type", null);
			ViewData["NsnCoordinatorFilter"] = new SelectList(nsnCoordinators, "UserName", "UserName", null);
			ViewData["SubcontractorFilter"] = new SelectList(subcontractors, "UserName", "UserName", null);
			ViewData["ZoneFilter"] = new SelectList(_context.Zone, "ZoneName", "ZoneName", null);
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
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				applicationDbContext = _context.Report.Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone).Where(r => r.NsnCoordinatorId == userId || r.SubcontractorId == userId || r.RequestorId == userId);//added where
			}

			Repositories.ReportRepository reportRepository = new Repositories.ReportRepository();

			applicationDbContext = reportRepository.GetFilteredReports(applicationDbContext, etrnumberS, priorityS, rfaidS, rfanameS, gradeS, troubletypeS, dateissuedfromS, dateissuedtoS, datesentfromS, datesenttoS, etrstatusS, etrtypeS, nsncoordS, subconS, zoneS);

			//filters
			/*if (!String.IsNullOrEmpty(etrnumberS))
                applicationDbContext = applicationDbContext.Where(r => r.EtrNumber.Contains(etrnumberS));
            if (!String.IsNullOrEmpty(nokiacaseidS))
                applicationDbContext = applicationDbContext.Where(r => r.NokiaCaseId.ToString().Contains(nokiacaseidS));
            if (!String.IsNullOrEmpty(rfaidS))
                applicationDbContext = applicationDbContext.Where(r => r.RfaId.ToString().Contains(rfaidS));
            if (!String.IsNullOrEmpty(rfanameS))
                applicationDbContext = applicationDbContext.Where(r => r.RfaName.Contains(rfanameS));
            if (!String.IsNullOrEmpty(assignedtoS))
                applicationDbContext = applicationDbContext.Where(r => r.AssignedTo.Contains(assignedtoS));
            if (!String.IsNullOrEmpty(priorityS))
                applicationDbContext = applicationDbContext.Where(r => r.Priority.Contains(priorityS));
            if (!String.IsNullOrEmpty(gradeS))
                applicationDbContext = applicationDbContext.Where(r => r.Grade.ToString().Contains(gradeS));
            if (!String.IsNullOrEmpty(troubletypeS))
                applicationDbContext = applicationDbContext.Where(r => r.TroubleType.Contains(troubletypeS));
            if (!String.IsNullOrEmpty(dateissuedS))
                applicationDbContext = applicationDbContext.Where(r => r.DateIssued.ToString().Contains(dateissuedS));
            if (!String.IsNullOrEmpty(datesentS))
                applicationDbContext = applicationDbContext.Where(r => r.DateSent.ToString().Contains(datesentS));
            if (!String.IsNullOrEmpty(etrtodesS))
                applicationDbContext = applicationDbContext.Where(r => r.EtrToDes.ToString().Contains(etrtodesS));
            if (!String.IsNullOrEmpty(closingdateS))
                applicationDbContext = applicationDbContext.Where(r => r.ClosingDate.ToString().Contains(closingdateS));
            if (!String.IsNullOrEmpty(etrstatusS))
                applicationDbContext = applicationDbContext.Where(r => r.EtrStatus.Status.Contains(etrstatusS));
            if (!String.IsNullOrEmpty(etrtypeS))
                applicationDbContext = applicationDbContext.Where(r => r.EtrType.Type.Contains(etrtypeS));
            if (!String.IsNullOrEmpty(nsncoordS))
                applicationDbContext = applicationDbContext.Where(r => r.NsnCoordinator.UserName.Contains(nsncoordS));
            if (!String.IsNullOrEmpty(requestorS))
                applicationDbContext = applicationDbContext.Where(r => r.Requestor.UserName.Contains(requestorS));
            if (!String.IsNullOrEmpty(subconS))
                applicationDbContext = applicationDbContext.Where(r => r.Subcontractor.UserName.Contains(subconS));
            if (!String.IsNullOrEmpty(zoneS))
                applicationDbContext = applicationDbContext.Where(r => r.Zone.ZoneName.Contains(zoneS));*/

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

			return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reports/Details/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EtrNumber,NokiaCaseId,RfaId,RfaName,ZoneId,AssignedTo,Priority,EtrTypeId,EtrStatusId,RequestorId,NsnCoordinatorId,SubcontractorId,Grade,TroubleType,DateIssued,DateSent,EtrToDes,ClosingDate,EtrDescription,Comment")] Report report)
        {
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

        // GET: Reports/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Report.SingleOrDefaultAsync(m => m.Id == id);
            _context.Report.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Report.Any(e => e.Id == id);
        }

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
