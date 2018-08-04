using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeloceCars.Data;
using VeloceCars.Models;
using VeloceCars.Models.UserViewModels;
using VeloceCars.Models.DriverViewModels;
using VeloceCars.Models.PackageViewModels;
using VeloceCars.Models.ReportViewModels;

namespace VeloceCars.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Report
        public ActionResult ActiveSchedule()
        {

            ViewData["Title"] = "Veloce Cars.";
            ViewData["SubTitle"] = "Active Schedule";

            var activeSchedule = (from s in _context.Schedule
                            join r in _context.Reservation on s.ReservationId equals r.Id
                            join p in _context.Package on r.PackageId equals p.Id
                            join d in _context.Driver on s.DriverId equals d.Id
                            join du in _context.Users on d.ApplicationUserId equals du.Id
                            join uc in _context.Users on r.ApplicationUserId equals uc.Id
                            select new ActiveScheduleViewModel
                            {
                                Id = r.Id,
                                Package = p.PackageName,
                                Driver = du.Fullname,
                                Client = uc.Fullname,
                                StartDateTime = r.StartDateTime,
                                EndDateTime = r.EndDateTime,
                                EstimatedFare = (r.EndDateTime.Subtract(r.StartDateTime).Days + 1) * p.Price
                            });

            return View(activeSchedule);
        }


        // GET: Report
        public ActionResult UserList()
        {

            ViewData["Title"] = "Veloce Cars.";
            ViewData["SubTitle"] = "All user list";

            var userList = (from u in _context.Users
                            join ur in _context.UserRoles on u.Id equals ur.UserId
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new ApplicationUserListViewModel
                            {
                                Firstname = u.Firstname,
                                Lastname = u.Lastname,
                                Email = u.Email,
                                ContactHome = u.ContactHome,
                                ContactOffice = u.ContactOffice,
                                RoleType = r.Name,
                                CreatedBy = u.CreatedBy
                            }).ToList();

            return View(userList);
        }

        // GET: Report
        public ActionResult VehicleList()
        {

            ViewData["Title"] = "Veloce Cars.";
            ViewData["SubTitle"] = "Vehicle list";

            var vehicleList = from u in _context.Vehicle select u;


            return View(vehicleList);
        }

        // GET: Report
        public ActionResult DriverList()
        {

            ViewData["Title"] = "Veloce Cars.";
            ViewData["SubTitle"] = "Driver list";

            var driverList = (from d in _context.Driver
                            join u in _context.Users on d.ApplicationUserId equals u.Id
                            join v in _context.Vehicle on d.VehicleId equals v.Id
                            select new DriverListViewModel
                            {
                                ApplicationUser = u.Fullname,
                                Vehicle = v.Model
                            }).ToList();

            return View(driverList);
        }

        // GET: Report
        public ActionResult PackageList()
        {

            ViewData["Title"] = "Veloce Cars.";
            ViewData["SubTitle"] = "Package list";

            var packageList = (from p in _context.Package
                            select new PackageListViewModel
                            {
                                Id = p.Id,
                                PackageName = p.PackageName,
                                Route = p.Route,
                                Price = p.Price
                            }).ToList();

            return View(packageList);
        }
    }
}