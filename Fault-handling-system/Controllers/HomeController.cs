using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fault_handling_system.Models;
using Microsoft.AspNetCore.Authorization;

namespace Fault_handling_system.Controllers
{
    /// <summary>
    /// Controller responsible for administering Home page.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Responsible for displaying Home page.
        /// </summary>
        /// <returns>
        /// Returns main view of the Home page
        /// </returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Responsible for displaying About page.
        /// </summary>
        /// <returns>
        /// Returns view of the About page
        /// </returns>
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        /// <summary>
        /// Responsible for displaying Contact page.
        /// </summary>
        /// <returns>
        /// Returns view of the Contact page
        /// </returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Responsible for displaying User guide.
        /// </summary>
        /// <returns>
        /// Returns view of the User guide
        /// </returns>
        public IActionResult UserGuide()
        {
            return View();
        }

        /// <summary>
        /// Responsible for displaying Error page.
        /// </summary>
        /// <returns>
        /// Returns view of the Error page
        /// </returns>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
