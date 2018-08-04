using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeloceCars.Data;
using VeloceCars.Models;
using VeloceCars.Models.ScheduleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace VeloceCars.Controllers
{
    [Authorize(Policy = "DriverPolicy")]
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScheduleController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Schedule
        public async Task<IActionResult> Index()
        {
            var scheduleList = from s in _context.Schedule
                               join d in _context.Driver on s.DriverId equals d.Id
                               join r in _context.Reservation on s.ReservationId equals r.Id
                               join u in _context.Users on r.ApplicationUserId equals u.Id
                               join p in _context.Package on r.PackageId equals p.Id
                               where s.Status != 1 && d.ApplicationUserId == _userManager.GetUserId(User)
                               select new JourneySchedueViewModel
                               {
                                   Id = s.Id,
                                   ReservationId = s.ReservationId,
                                   Client = u.Fullname,
                                   StartDateTime = r.StartDateTime,
                                   EndDateTime = r.EndDateTime,
                                   Package = p.PackageName,
                                   Status = s.Status
                               };

            return View(await scheduleList.ToListAsync());
        }


        // GET: Schedule
        public async Task<IActionResult> All()
        {

            ViewData["ReturnUrl"] = "All";

            var scheduleList = from s in _context.Schedule
                               join d in _context.Driver on s.DriverId equals d.Id
                               join r in _context.Reservation on s.ReservationId equals r.Id
                               join u in _context.Users on r.ApplicationUserId equals u.Id
                               join p in _context.Package on r.PackageId equals p.Id
                               where d.ApplicationUserId == _userManager.GetUserId(User)
                               select new JourneySchedueViewModel
                               {
                                   Id = s.Id,
                                   ReservationId = s.ReservationId,
                                   Client = u.Fullname,
                                   StartDateTime = r.StartDateTime,
                                   EndDateTime = r.EndDateTime,
                                   Package = p.PackageName,
                                   Status = s.Status
                               };

            return View(await scheduleList.ToListAsync());
        }

        //Not being used
        // GET: Schedule/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var schedule = await _context.Schedule.SingleOrDefaultAsync(m => m.Id == id);
        //    if (schedule == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(schedule);
        //}


        // GET: Schedule/Journey/5
        public async Task<IActionResult> Journey(int? id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (id == null)
            {
                return NotFound();
            }

            var schedule = await (from s in _context.Schedule
                                  join d in _context.Driver on s.DriverId equals d.Id
                                  join r in _context.Reservation on s.ReservationId equals r.Id
                                  join u in _context.Users on r.ApplicationUserId equals u.Id
                                  join p in _context.Package on r.PackageId equals p.Id
                                  where s.Id == id && d.ApplicationUserId == _userManager.GetUserId(User)
                                  select new JourneySchedueViewModel
                                  {
                                      Id = s.Id,
                                      ReservationId = s.ReservationId,
                                      Client = u.Fullname,
                                      StartDateTime = r.StartDateTime,
                                      EndDateTime = r.EndDateTime,
                                      Package = p.PackageName,
                                      Status = s.Status,
                                      JourneyStartDateTime = s.StartDateTime,
                                      JourneyEndDateTime = s.EndDateTime,
                                      NetTotal = s.NetTotal
                                  }).SingleOrDefaultAsync();

            if (schedule == null)
            {
                return NotFound();
            }

            if (schedule.Status == null)
            {
                ViewData["CheckButton"] = "Check In";
            }
            else if (schedule.Status == 0)
            {
                ViewData["CheckButton"] = "Check Out";
            }

            return View(schedule);

        }

        // POST: Schedule/Journey/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Journey(int? id, JourneySchedueViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var schedule = await (from s in _context.Schedule
                                      where s.Id == id
                                      select s).SingleOrDefaultAsync();

                try
                {


                    if (schedule.Status == null)
                    {

                        schedule.StartDateTime = DateTime.Now;
                        schedule.Status = 0;
                        _context.Update(schedule);

                        model.JourneyStartDateTime = DateTime.Now;
                    }
                    else if (schedule.Status == 0)
                    {

                        double netTotal = 0;
                        DateTime startDate = Convert.ToDateTime(schedule.StartDateTime);
                        DateTime endDate = DateTime.Now;

                        try
                        {

                            int days = endDate.Subtract(startDate).Days + 1;

                            double rate = Convert.ToDouble((from s in _context.Schedule
                                                            join r in _context.Reservation on s.ReservationId equals r.Id
                                                            join p in _context.Package on r.PackageId equals p.Id
                                                            where s.Id == id
                                                            select p.Price).SingleOrDefault());

                            netTotal = days * rate;

                        }
                        catch
                        {
                            throw;
                        }

                        schedule.EndDateTime = endDate;
                        schedule.NetTotal = netTotal;
                        schedule.Status = 1;
                        _context.Update(schedule);
                        
                        model.NetTotal = netTotal;
                        model.JourneyEndDateTime = endDate;
                        model.Status = 1;
                    }

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (schedule.Status == null)
                {
                    ViewData["CheckButton"] = "Check In";
                }
                else if (schedule.Status == 0)
                {
                    ViewData["CheckButton"] = "Check Out";
                }

                ModelState.Clear();
                return View(model);

            }

            return View(model);
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedule.Any(e => e.Id == id);
        }
    }
}
