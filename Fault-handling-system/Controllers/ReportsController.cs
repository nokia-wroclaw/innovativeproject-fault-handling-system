﻿using System;
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
using PagedList;

namespace Fault_handling_system.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index(string sortOrder, string etrnumberS, string nokiacaseidS, string priorityS, string requestorS, string etrnumberC, string nokiacaseidC, string priorityC, string requestorC, string rfaidS, string rfaidC, string rfanameS, string rfanameC, string assignedtoS, string assignedtoC, string gradeS, string gradeC, string troubletypeS, string troubletypeC, string dateissuedS, string dateissuedC, string datesentS, string datesentC, string etrtodesS, string etrtodesC, string closingdateS, string closingdateC, string etrstatusS, string etrstatusC, string etrtypeS, string etrtypeC, string nsncoordS, string nsncoordC, string subconS, string subconC, string zoneS, string zoneC)
        {
            if (User.IsInRole("Admin")) //if user is admin they can view all reports
            {
                ViewBag.EtrNumSortParm = sortOrder == "etrnumber_desc" ? "etrnumber" : "etrnumber_desc";
                ViewBag.NokiaCaseIdSortParm = sortOrder == "nokiacaseid_desc" ? "nokiacaseid" : "nokiacaseid_desc";
                ViewBag.RfaIdSortParm = sortOrder == "rfaid_desc" ? "rfaid" : "rfaid_desc";
                ViewBag.RfaNameSortParm = sortOrder == "rfaname_desc" ? "rfaname" : "rfaname_desc";
                ViewBag.AssignedToSortParm = sortOrder == "assignedto_desc" ? "assignedto" : "assignedto_desc";
                ViewBag.PrioritySortParm = sortOrder == "priority_desc" ? "priority" : "priority_desc";
                ViewBag.GradeSortParm = sortOrder == "grade_desc" ? "grade" : "grade_desc";
                ViewBag.TroubleTypeSortParm = sortOrder == "troubletype_desc" ? "troubletype" : "troubletype_desc";
                ViewBag.DateIssuedSortParm = sortOrder == "dateissued_desc" ? "dateissued" : "dateissued_desc";
                ViewBag.DateSentSortParm = sortOrder == "datesent_desc" ? "datesent" : "datesent_desc";
                ViewBag.EtrToDesSortParm = sortOrder == "etrtodes_desc" ? "etrtodes" : "etrtodes_desc";
                ViewBag.ClosingDateSortParm = sortOrder == "closingdate_desc" ? "closingdate" : "closingdate_desc";
                ViewBag.EtrStatusSortParm = sortOrder == "etrstatus_desc" ? "etrstatus" : "etrstatus_desc";
                ViewBag.EtrTypeSortParm = sortOrder == "etrtype_desc" ? "etrtype" : "etrtype_desc";
                ViewBag.NsnCoordSortParm = sortOrder == "nsncoord_desc" ? "nsncoord" : "nsncoord_desc";
                ViewBag.RequestorSortParm = sortOrder == "requestor_desc" ? "requestor" : "requestor_desc";
                ViewBag.SubconSortParm = sortOrder == "subcon_desc" ? "subcon" : "subcon_desc";
                ViewBag.ZoneSortParm = sortOrder == "zone_desc" ? "zone" : "zone_desc";

                //restore current filters
                if (String.IsNullOrEmpty(etrnumberS))
                {
                    etrnumberS = etrnumberC;
                }
                if (String.IsNullOrEmpty(nokiacaseidS))
                {
                    nokiacaseidS = nokiacaseidC;
                }
                if (String.IsNullOrEmpty(rfaidS))
                {
                    rfaidS = rfaidC;
                }
                if (String.IsNullOrEmpty(rfanameS))
                {
                    rfanameS = rfanameC;
                }
                if (String.IsNullOrEmpty(assignedtoS))
                {
                    assignedtoS = assignedtoC;
                }
                if (String.IsNullOrEmpty(priorityS))
                {
                    priorityS = priorityC;
                }
                if (String.IsNullOrEmpty(gradeS))
                {
                    gradeS = gradeC;
                }
                if (String.IsNullOrEmpty(troubletypeS))
                {
                    troubletypeS = troubletypeC;
                }
                if (String.IsNullOrEmpty(dateissuedS))
                {
                    dateissuedS = dateissuedC;
                }
                if (String.IsNullOrEmpty(datesentS))
                {
                    datesentS = datesentC;
                }
                if (String.IsNullOrEmpty(etrtodesS))
                {
                    etrtodesS = etrtodesC;
                }
                if (String.IsNullOrEmpty(closingdateS))
                {
                    closingdateS = closingdateC;
                }
                if (String.IsNullOrEmpty(etrstatusS))
                {
                    etrstatusS = etrstatusC;
                }
                if (String.IsNullOrEmpty(etrtypeS))
                {
                    etrtypeS = etrtypeC;
                }
                if (String.IsNullOrEmpty(nsncoordS))
                {
                    nsncoordS = nsncoordC;
                }
                if (String.IsNullOrEmpty(requestorS))
                {
                    requestorS = requestorC;
                }
                if (String.IsNullOrEmpty(subconS))
                {
                    subconS = subconC;
                }
                if (String.IsNullOrEmpty(zoneS))
                {
                    zoneS = zoneC;
                }


                //restore current filters
                ViewBag.etrnumberC = etrnumberS;
                ViewBag.nokiacaseidC = nokiacaseidS;
                ViewBag.rfaidC = rfaidS;
                ViewBag.rfanameC = rfanameS;
                ViewBag.assignedtoC = assignedtoS;
                ViewBag.priorityC = priorityS;
                ViewBag.gradeC = gradeS;
                ViewBag.troubletypeC = troubletypeS;
                ViewBag.dateissuedC = dateissuedS;
                ViewBag.datesentC = datesentS;
                ViewBag.etrtodesC = etrtodesS;
                ViewBag.closingdateC = closingdateS;
                ViewBag.etrstatusC = etrstatusS;
                ViewBag.etrtypeC = etrtypeS;
                ViewBag.nsncoordC = nsncoordS;
                ViewBag.requestorC = requestorS;
                ViewBag.subconC = subconS;
                ViewBag.zoneC = zoneS;



                var applicationDbContext = _context.Report.Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone).AsQueryable();

                //filters
                if (!String.IsNullOrEmpty(etrnumberS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.EtrNumber.Contains(etrnumberS));
                }
                if (!String.IsNullOrEmpty(nokiacaseidS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.NokiaCaseId.ToString().Contains(nokiacaseidS));
                }
                if (!String.IsNullOrEmpty(rfaidS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.RfaId.ToString().Contains(rfaidS));
                }
                if (!String.IsNullOrEmpty(rfanameS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.RfaName.Contains(rfanameS));
                }
                if (!String.IsNullOrEmpty(assignedtoS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.AssignedTo.Contains(assignedtoS));
                }
                if (!String.IsNullOrEmpty(priorityS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.Priority.Contains(priorityS));
                }
                if (!String.IsNullOrEmpty(gradeS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.Grade.ToString().Contains(gradeS));
                }
                if (!String.IsNullOrEmpty(troubletypeS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.TroubleType.Contains(troubletypeS));
                }
                if (!String.IsNullOrEmpty(dateissuedS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.DateIssued.ToString().Contains(dateissuedS));
                }
                if (!String.IsNullOrEmpty(datesentS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.DateSent.ToString().Contains(datesentS));
                }
                if (!String.IsNullOrEmpty(etrtodesS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.EtrToDes.ToString().Contains(etrtodesS));
                }
                if (!String.IsNullOrEmpty(closingdateS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.ClosingDate.ToString().Contains(closingdateS));
                }
                if (!String.IsNullOrEmpty(etrstatusS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.EtrStatus.Status.Contains(etrstatusS));
                }
                if (!String.IsNullOrEmpty(etrtypeS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.EtrType.Type.Contains(etrtypeS));
                }
                if (!String.IsNullOrEmpty(nsncoordS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.NsnCoordinator.UserName.Contains(nsncoordS));
                }
                if (!String.IsNullOrEmpty(requestorS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.Requestor.UserName.Contains(requestorS));
                }
                if (!String.IsNullOrEmpty(subconS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.Subcontractor.UserName.Contains(subconS));
                }
                if (!String.IsNullOrEmpty(zoneS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.Zone.ZoneName.Contains(zoneS));
                }

                //sorting order
                switch (sortOrder)
                {
                    case "etrnumber":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.EtrNumber);
                        break;
                    case "etrnumber_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.EtrNumber);
                        break;
                    case "nokiacaseid":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.NokiaCaseId);
                        //applicationDbContext = _context.Report.OrderBy(r => r.NokiaCaseId).Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone);
                        break;
                    case "nokiacaseid_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.NokiaCaseId);
                        //applicationDbContext = _context.Report.OrderByDescending(r => r.NokiaCaseId).Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone);
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
                    case "assignedto":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.AssignedTo);
                        break;
                    case "assignedto_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.AssignedTo);
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
                    case "etrtodes":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.EtrToDes);
                        break;
                    case "etrtodes_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.EtrToDes);
                        break;
                    case "closingdate":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.ClosingDate);
                        break;
                    case "closingdate_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.ClosingDate);
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
                    case "requestor":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.Requestor);
                        break;
                    case "requestor_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.Requestor);
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
                return View(await applicationDbContext.ToListAsync());
            }
            else //otherwise they can view only those thery are enrolled in in any way
            {
                ViewBag.EtrNumSortParm = sortOrder == "etrnumber_desc" ? "etrnumber" : "etrnumber_desc";
                ViewBag.NokiaCaseIdSortParm = sortOrder == "nokiacaseid_desc" ? "nokiacaseid" : "nokiacaseid_desc";
                ViewBag.RfaIdSortParm = sortOrder == "rfaid_desc" ? "rfaid" : "rfaid_desc";
                ViewBag.RfaNameSortParm = sortOrder == "rfaname_desc" ? "rfaname" : "rfaname_desc";
                ViewBag.AssignedToSortParm = sortOrder == "assignedto_desc" ? "assignedto" : "assignedto_desc";
                ViewBag.PrioritySortParm = sortOrder == "priority_desc" ? "priority" : "priority_desc";
                ViewBag.GradeSortParm = sortOrder == "grade_desc" ? "grade" : "grade_desc";
                ViewBag.TroubleTypeSortParm = sortOrder == "troubletype_desc" ? "troubletype" : "troubletype_desc";
                ViewBag.DateIssuedSortParm = sortOrder == "dateissued_desc" ? "dateissued" : "dateissued_desc";
                ViewBag.DateSentSortParm = sortOrder == "datesent_desc" ? "datesent" : "datesent_desc";
                ViewBag.EtrToDesSortParm = sortOrder == "etrtodes_desc" ? "etrtodes" : "etrtodes_desc";
                ViewBag.ClosingDateSortParm = sortOrder == "closingdate_desc" ? "closingdate" : "closingdate_desc";
                ViewBag.EtrStatusSortParm = sortOrder == "etrstatus_desc" ? "etrstatus" : "etrstatus_desc";
                ViewBag.EtrTypeSortParm = sortOrder == "etrtype_desc" ? "etrtype" : "etrtype_desc";
                ViewBag.NsnCoordSortParm = sortOrder == "nsncoord_desc" ? "nsncoord" : "nsncoord_desc";
                ViewBag.RequestorSortParm = sortOrder == "requestor_desc" ? "requestor" : "requestor_desc";
                ViewBag.SubconSortParm = sortOrder == "subcon_desc" ? "subcon" : "subcon_desc";
                ViewBag.ZoneSortParm = sortOrder == "zone_desc" ? "zone" : "zone_desc";

                //restore current filters
                if (String.IsNullOrEmpty(etrnumberS))
                {
                    etrnumberS = etrnumberC;
                }
                if (String.IsNullOrEmpty(nokiacaseidS))
                {
                    nokiacaseidS = nokiacaseidC;
                }
                if (String.IsNullOrEmpty(rfaidS))
                {
                    rfaidS = rfaidC;
                }
                if (String.IsNullOrEmpty(rfanameS))
                {
                    rfanameS = rfanameC;
                }
                if (String.IsNullOrEmpty(assignedtoS))
                {
                    assignedtoS = assignedtoC;
                }
                if (String.IsNullOrEmpty(priorityS))
                {
                    priorityS = priorityC;
                }
                if (String.IsNullOrEmpty(gradeS))
                {
                    gradeS = gradeC;
                }
                if (String.IsNullOrEmpty(troubletypeS))
                {
                    troubletypeS = troubletypeC;
                }
                if (String.IsNullOrEmpty(dateissuedS))
                {
                    dateissuedS = dateissuedC;
                }
                if (String.IsNullOrEmpty(datesentS))
                {
                    datesentS = datesentC;
                }
                if (String.IsNullOrEmpty(etrtodesS))
                {
                    etrtodesS = etrtodesC;
                }
                if (String.IsNullOrEmpty(closingdateS))
                {
                    closingdateS = closingdateC;
                }
                if (String.IsNullOrEmpty(etrstatusS))
                {
                    etrstatusS = etrstatusC;
                }
                if (String.IsNullOrEmpty(etrtypeS))
                {
                    etrtypeS = etrtypeC;
                }
                if (String.IsNullOrEmpty(nsncoordS))
                {
                    nsncoordS = nsncoordC;
                }
                if (String.IsNullOrEmpty(requestorS))
                {
                    requestorS = requestorC;
                }
                if (String.IsNullOrEmpty(subconS))
                {
                    subconS = subconC;
                }
                if (String.IsNullOrEmpty(zoneS))
                {
                    zoneS = zoneC;
                }


                //restore current filters
                ViewBag.etrnumberC = etrnumberS;
                ViewBag.nokiacaseidC = nokiacaseidS;
                ViewBag.rfaidC = rfaidS;
                ViewBag.rfanameC = rfanameS;
                ViewBag.assignedtoC = assignedtoS;
                ViewBag.priorityC = priorityS;
                ViewBag.gradeC = gradeS;
                ViewBag.troubletypeC = troubletypeS;
                ViewBag.dateissuedC = dateissuedS;
                ViewBag.datesentC = datesentS;
                ViewBag.etrtodesC = etrtodesS;
                ViewBag.closingdateC = closingdateS;
                ViewBag.etrstatusC = etrstatusS;
                ViewBag.etrtypeC = etrtypeS;
                ViewBag.nsncoordC = nsncoordS;
                ViewBag.requestorC = requestorS;
                ViewBag.subconC = subconS;
                ViewBag.zoneC = zoneS;


                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var applicationDbContext = _context.Report.Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone).Where(r => r.NsnCoordinatorId == userId || r.SubcontractorId == userId || r.RequestorId == userId);//added where

                //filters
                if (!String.IsNullOrEmpty(etrnumberS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.EtrNumber.Contains(etrnumberS));
                }
                if (!String.IsNullOrEmpty(nokiacaseidS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.NokiaCaseId.ToString().Contains(nokiacaseidS));
                }
                if (!String.IsNullOrEmpty(rfaidS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.RfaId.ToString().Contains(rfaidS));
                }
                if (!String.IsNullOrEmpty(rfanameS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.RfaName.Contains(rfanameS));
                }
                if (!String.IsNullOrEmpty(assignedtoS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.AssignedTo.Contains(assignedtoS));
                }
                if (!String.IsNullOrEmpty(priorityS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.Priority.Contains(priorityS));
                }
                if (!String.IsNullOrEmpty(gradeS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.Grade.ToString().Contains(gradeS));
                }
                if (!String.IsNullOrEmpty(troubletypeS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.TroubleType.Contains(troubletypeS));
                }
                if (!String.IsNullOrEmpty(dateissuedS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.DateIssued.ToString().Contains(dateissuedS));
                }
                if (!String.IsNullOrEmpty(datesentS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.DateSent.ToString().Contains(datesentS));
                }
                if (!String.IsNullOrEmpty(etrtodesS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.EtrToDes.ToString().Contains(etrtodesS));
                }
                if (!String.IsNullOrEmpty(closingdateS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.ClosingDate.ToString().Contains(closingdateS));
                }
                if (!String.IsNullOrEmpty(etrstatusS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.EtrStatus.Status.Contains(etrstatusS));
                }
                if (!String.IsNullOrEmpty(etrtypeS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.EtrType.Type.Contains(etrtypeS));
                }
                if (!String.IsNullOrEmpty(nsncoordS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.NsnCoordinator.UserName.Contains(nsncoordS));
                }
                if (!String.IsNullOrEmpty(requestorS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.Requestor.UserName.Contains(requestorS));
                }
                if (!String.IsNullOrEmpty(subconS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.Subcontractor.UserName.Contains(subconS));
                }
                if (!String.IsNullOrEmpty(zoneS))
                {
                    applicationDbContext = applicationDbContext.Where(r => r.Zone.ZoneName.Contains(zoneS));
                }

                //sorting order
                switch (sortOrder)
                {
                    case "etrnumber":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.EtrNumber);
                        break;
                    case "etrnumber_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.EtrNumber);
                        break;
                    case "nokiacaseid":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.NokiaCaseId);
                        //applicationDbContext = _context.Report.OrderBy(r => r.NokiaCaseId).Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone);
                        break;
                    case "nokiacaseid_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.NokiaCaseId);
                        //applicationDbContext = _context.Report.OrderByDescending(r => r.NokiaCaseId).Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone);
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
                    case "assignedto":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.AssignedTo);
                        break;
                    case "assignedto_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.AssignedTo);
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
                    case "etrtodes":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.EtrToDes);
                        break;
                    case "etrtodes_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.EtrToDes);
                        break;
                    case "closingdate":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.ClosingDate);
                        break;
                    case "closingdate_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.ClosingDate);
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
                    case "requestor":
                        applicationDbContext = applicationDbContext.OrderBy(r => r.Requestor);
                        break;
                    case "requestor_desc":
                        applicationDbContext = applicationDbContext.OrderByDescending(r => r.Requestor);
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
