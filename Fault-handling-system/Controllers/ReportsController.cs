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

namespace Fault_handling_system.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
			if (User.IsInRole("Admin")) //if user is admin they can view all reports
			{
				var applicationDbContext = _context.Report.Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone);
				return View(await applicationDbContext.ToListAsync());
			}
			else //otherwise they can view only those thery are enrolled in in any way
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var applicationDbContext = _context.Report.Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone).Where(r => r.NsnCoordinatorId == userId || r.SubcontractorId == userId || r.RequestorId == userId);//added where
				return View(await applicationDbContext.ToListAsync());
			}
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
        public IActionResult Create()
        {
            ViewData["EtrStatusId"] = new SelectList(_context.EtrStatus, "Id", "Status");
            ViewData["EtrTypeId"] = new SelectList(_context.EtrType, "Id", "Type");
            ViewData["NsnCoordinatorId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["RequestorId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["SubcontractorId"] = new SelectList(_context.Users, "Id", "UserName");
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
            ViewData["EtrStatusId"] = new SelectList(_context.EtrStatus, "Id", "Status", report.EtrStatusId);
            ViewData["EtrTypeId"] = new SelectList(_context.EtrType, "Id", "Type", report.EtrTypeId);
            ViewData["NsnCoordinatorId"] = new SelectList(_context.Users, "Id", "UserName", report.NsnCoordinatorId);
            ViewData["RequestorId"] = new SelectList(_context.Users, "Id", "UserName", report.RequestorId);
            ViewData["SubcontractorId"] = new SelectList(_context.Users, "Id", "UserName", report.SubcontractorId);
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
    }
}
