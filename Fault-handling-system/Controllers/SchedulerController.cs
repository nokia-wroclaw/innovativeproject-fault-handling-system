using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fault_handling_system.Data;
using Fault_handling_system.Models;
using Fault_handling_system.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Fault_handling_system.Controllers
{
    /// <summary>
    /// The main controller for scheduler.
    /// Contains actions for create new job and start or stop created jobs
    /// </summary>
    [Authorize]
    public class SchedulerController : Controller
    {
        private ILogger _logger;
        private ApplicationDbContext _context;
        private readonly ISchedulerService _scheduler;

        /// <summary>
        /// SchedulerController constructor.
        /// </summary>
        /// <param name="context">Instance of <c>ApplicationDbContext</c> is responsible for communication with SQL server</param>
        /// <param name="logger">ILogger</param>
        /// <param name="scheduler">ISchedulerService</param>
        public SchedulerController(ApplicationDbContext context,
            ILogger<SchedulerController> logger, ISchedulerService scheduler)
        {
            _context = context;
            _logger = logger;
            _scheduler = scheduler;
            
        }
        // GET: Scheduler
        /// <summary>
        /// Action <c>Index</c> can render a view with list of jobs from database.
        /// </summary>
        /// <returns>
        /// ViewResult - list of created jobs
        /// </returns>
        public async Task<IActionResult> Index()
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
            _logger.LogDebug(userId);
            var filters = (from x in _context.ReportFilter
                                where x.UserId.Equals(userId)
                                select x);

            ViewData["IntervalOptionList"] = list;
            ViewData["FilterId"] = new SelectList(filters, "Id", "Name", null);
            ViewData["DaysOfWeek"] = daysOfWeek;

            _logger.LogDebug(userId);
            var SchedulerFilters = _context.ScheduleFilter
                .Include(r=>r.Filter)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r=>r.Active)
                .ThenByDescending(r=>r.Id);

            var SchedulerFiltersList = await SchedulerFilters.ToListAsync();

            return View(new ScheduleFilterViewModel(SchedulerFiltersList));
        }

        // POST: Scheduler/AddNew
        /// <summary>
        /// Add new job to scheduler
        /// </summary>
        /// <param name="IntervalDropDown">Job interval</param>
        /// <param name="FilterId">Filter Id</param>
        /// <param name="Cron">Unix Cron</param>
        /// <param name="Hour">Job hour</param>
        /// <param name="DayOfWeek">Day of week</param>
        /// <param name="MailingLists">List of mail</param>
        /// <returns>ViewResult with all jobs</returns>
        [HttpPost]
        [Route("AddNew")]
        public async Task<IActionResult> AddNew(String IntervalDropDown, int FilterId, String Cron, String Hour, String DayOfWeek, String MailingLists)
        {
            var filter = await (from x in _context.ReportFilter
                                where x.Id == FilterId
                                select x).SingleOrDefaultAsync();
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int MaxId = _context.ScheduleFilter.Max(x => (int?) x.Id) ?? 0;
            int NewSchedulerFilterId = MaxId + 1;
            bool status;
            switch (IntervalDropDown)
            {
                case ("H"):
                    status = _scheduler.AddHourly(NewSchedulerFilterId, FilterId, UserId, Hour, MailingLists);
                    break;
                case ("D"):
                    status = _scheduler.AddDaily(NewSchedulerFilterId, FilterId, UserId, Hour, MailingLists);
                    break;
                case ("W"):
                    status = _scheduler.AddWeekly(NewSchedulerFilterId, FilterId, UserId, Hour, DayOfWeek, MailingLists);
                    break;
                case ("C"):
                    status = AddCron(NewSchedulerFilterId, FilterId, UserId, Cron, MailingLists);
                    break;
                default:
                    status = false;
                    _logger.LogError("Wrong interval option");
                    break;
            }
            _logger.LogDebug(status.ToString());
            return RedirectToAction(nameof(Index));
        }

        // POST: Scheduler/Start
        /// <summary>
        /// Start job
        /// </summary>
        /// <returns>ViewResult with all jobs</returns>
        [HttpPost]
        [Route("Start")]
        public async Task<IActionResult> Start(int Id)
        {
            _scheduler.StartReportSendJob(Id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Scheduler/Stop
        /// <summary>
        /// Stop job
        /// </summary>
        /// <returns>ViewResult with all jobs</returns>
        [HttpPost]
        [Route("Stop")]
        public async Task<IActionResult> Stop(int Id)
        {
            _scheduler.StopReportSendJob(Id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Scheduler/Delete
        /// <summary>
        /// Permamently delete job from scheduler
        /// </summary>
        /// <returns>ViewResult with all jobs</returns>
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            _scheduler.DeleteReportSendJob(Id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Add Cron Job to Scheduler.
        /// </summary>
        /// <param name="NewSchedulerFilterId">Scheduled Filter ID</param>
        /// <param name="FilterId">Filter Id</param>
        /// <param name="UserId">Creator Id</param>
        /// <param name="Cron">Unix Cron</param>
        /// <param name="MailingLists">List of mail</param>
        /// <returns>True or false</returns>
        private bool AddCron(int NewSchedulerFilterId, int FilterId, String UserId, String Cron, String MailingLists)
        {
            if (ValidateCron(Cron))
            {
                _logger.LogInformation("Validated");
                return _scheduler.AddCron(NewSchedulerFilterId, FilterId, UserId, Cron, MailingLists);
            } else
            {
                _logger.LogInformation("Unvalidated");
                return false;
            }
            
        }

        /// <summary>
        /// Validate Unix Cron.
        /// </summary>
        /// <param name="Cron">Unix Cron</param>
        /// <returns>True or false</returns>
        private bool ValidateCron(String Cron)
        {
            Cron = Cron.Trim();
            String QuartzCron;


            if (Cron[Cron.Length - 1] == '*')
            {
                QuartzCron = "0 " + Cron.Substring(0, Cron.Length - 1) + "? *";
            }
            else
            {
                QuartzCron = "0 " + Cron + " *";
            }

            var valid = CronExpression.IsValidExpression(QuartzCron);
            // Some expressions are parsed as valid by the above method but they are not valid, like "* * * ? * *&54".
            //In order to avoid such invalid expressions an additional check is required, that is done using the below regex.

            var regex = @"^\s*($|#|\w+\s*=|(\?|\*|(?:[0-5]?\d)(?:(?:-|\/|\,)(?:[0-5]?\d))?(?:,(?:[0-5]?\d)(?:(?:-|\/|\,)(?:[0-5]?\d))?)*)\s+(\?|\*|(?:[0-5]?\d)(?:(?:-|\/|\,)(?:[0-5]?\d))?(?:,(?:[0-5]?\d)(?:(?:-|\/|\,)(?:[0-5]?\d))?)*)\s+(\?|\*|(?:[01]?\d|2[0-3])(?:(?:-|\/|\,)(?:[01]?\d|2[0-3]))?(?:,(?:[01]?\d|2[0-3])(?:(?:-|\/|\,)(?:[01]?\d|2[0-3]))?)*)\s+(\?|\*|(?:0?[1-9]|[12]\d|3[01])(?:(?:-|\/|\,)(?:0?[1-9]|[12]\d|3[01]))?(?:,(?:0?[1-9]|[12]\d|3[01])(?:(?:-|\/|\,)(?:0?[1-9]|[12]\d|3[01]))?)*)\s+(\?|\*|(?:[1-9]|1[012])(?:(?:-|\/|\,)(?:[1-9]|1[012]))?(?:L|W)?(?:,(?:[1-9]|1[012])(?:(?:-|\/|\,)(?:[1-9]|1[012]))?(?:L|W)?)*|\?|\*|(?:JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(?:(?:-)(?:JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))?(?:,(?:JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(?:(?:-)(?:JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))?)*)\s+(\?|\*|(?:[0-6])(?:(?:-|\/|\,|#)(?:[0-6]))?(?:L)?(?:,(?:[0-6])(?:(?:-|\/|\,|#)(?:[0-6]))?(?:L)?)*|\?|\*|(?:MON|TUE|WED|THU|FRI|SAT|SUN)(?:(?:-)(?:MON|TUE|WED|THU|FRI|SAT|SUN))?(?:,(?:MON|TUE|WED|THU|FRI|SAT|SUN)(?:(?:-)(?:MON|TUE|WED|THU|FRI|SAT|SUN))?)*)(|\s)+(\?|\*|(?:|\d{4})(?:(?:-|\/|\,)(?:|\d{4}))?(?:,(?:|\d{4})(?:(?:-|\/|\,)(?:|\d{4}))?)*))$";

            return valid && Regex.IsMatch(QuartzCron, regex);
        }

    }
}