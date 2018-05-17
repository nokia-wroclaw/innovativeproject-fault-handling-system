using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fault_handling_system.Data;
using Fault_handling_system.Models;
using Fault_handling_system.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fault_handling_system.Controllers
{
    public class SchedulerController : Controller
    {
        private ILogger _logger;
        private IEmailSender _emailSender;
        private ApplicationDbContext _context;
        private readonly ISchedulerService _scheduler;
        enum Intervals { Hourly, Daily, Weekly };

        public SchedulerController(ApplicationDbContext context, IEmailSender emailSender,
            ILogger<SchedulerController> logger, ISchedulerService scheduler)
        {
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
            _scheduler = scheduler;
        }

        public IActionResult Index()
        {
            var list = new List<SelectListItem>
            {      
                new SelectListItem{ Text="Hourly", Value = "H" },
                new SelectListItem{ Text="Daily", Value = "D" },
                new SelectListItem{ Text="Weekly", Value = "W" },
                new SelectListItem{ Text="Cron", Value = "C" },
            };
            var daysOfWeek = new List<SelectListItem>
            {
                new SelectListItem{ Text="Monday", Value = "Monday"},
                new SelectListItem{ Text="Tuesday", Value = "Tuesday"},
                new SelectListItem{ Text="Wednesday", Value = "Wednesday"},
                new SelectListItem{ Text="Thursday", Value = "Thursday"},
                new SelectListItem{ Text="Friday", Value = "Friday"},
                new SelectListItem{ Text="Saturday", Value = "Saturday"},
                new SelectListItem{ Text="Sunday", Value = "Sunday"},
            };

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var filters = (from x in _context.ReportFilter
                                where x.UserId.Equals(userId)
                                select x);

            ViewData["IntervalOptionList"] = list;
            ViewData["FilterId"] = new SelectList(filters, "Id", "Name", null);
            ViewData["DaysOfWeek"] = daysOfWeek;
            return View();
        }

        [HttpPost]
        [Route("Start")]
        public async Task<IActionResult> Start(String IntervalDropDown, int FilterId, String Cron, String Hour, String DayOfWeek)
        {
            var filter = await (from x in _context.ReportFilter
                                where x.Id == FilterId
                                select x).SingleOrDefaultAsync();

            switch (IntervalDropDown)
            {
                case ("H"):
                    _scheduler.AddHourly(FilterId, Hour);
                    break;
                case ("D"):
                    _scheduler.AddDaily(FilterId, Hour);
                    break;
                case ("W"):
                    _scheduler.AddWeekly(FilterId, Hour, DayOfWeek);
                    break;
                case ("C"):
                    _scheduler.AddCron(FilterId, Cron);
                    break;
                default:
                    _logger.LogError("Wrong interval option");
                    break;
            }

            

            return RedirectToAction(nameof(Index));
        }

    }
}