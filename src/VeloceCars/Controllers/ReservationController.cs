using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeloceCars.Data;
using VeloceCars.Models;
using VeloceCars.Models.ReservationModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace VeloceCars.Controllers
{
    [Authorize(Policy = "ClientPolicy")]
    public class ReservationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        IAuthorizationService _authorizationService;

        public ReservationController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _context = context;
            _authorizationService = authorizationService;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {

            if (!await _authorizationService.AuthorizeAsync(User, "BranchPolicy"))
            {
                ViewData["Title"] = "All Reservations";
                var reservationList = from r in _context.Reservation
                                      join p in _context.Package on r.PackageId equals p.Id
                                      join u in _context.Users on r.ApplicationUserId equals u.Id
                                      where u.Id == _userManager.GetUserId(User)
                select new ReservationListViewModel {
                                          Id = r.Id,
                                          Client = u.Fullname,
                                          Package = p.PackageName,
                                          StartDateTime = r.StartDateTime,
                                          EndDateTime = r.EndDateTime,
                                          Status = r.Status == 1 && (from s in _context.Schedule where s.ReservationId == r.Id && s.Status == 1 select s).Count() > 0 ? 2 : r.Status,
                                          Feedback = r.Feedback };

                return View(await reservationList.ToListAsync());
            }
            else
            {
                return RedirectToAction("All", "Reservation");
            }

        }


        // GET: AllReservation
        [Authorize(Policy = "BranchPolicy")]
        public async Task<IActionResult> All()
        {
            if (await _authorizationService.AuthorizeAsync(User, "HQPolicy"))
            {
                ViewData["Title"] = "All Reservations";
            }
            else
            {
                ViewData["Title"] = "All Reservations & Feedback";
            }
                

            var reservationList = (from r in _context.Reservation
                                   join p in _context.Package on r.PackageId equals p.Id
                                   join u in _context.Users on r.ApplicationUserId equals u.Id
                                   select new ReservationListViewModel { Id = r.Id,
                                       Client = u.Fullname,
                                       Package = p.PackageName,
                                       StartDateTime = r.StartDateTime,
                                       EndDateTime = r.EndDateTime,
                                       Status = r.Status,
                                       CreatedBy = r.CreatedBy,
                                       Feedback = r.Feedback
                                   });

            return View(await reservationList.ToListAsync());
        }


        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationList = (from r in _context.Reservation
                                   join p in _context.Package on r.PackageId equals p.Id
                                   join u in _context.Users on r.ApplicationUserId equals u.Id
                                   where u.Id == _userManager.GetUserId(User)
                                   select new ReservationListViewModel
                                   {
                                       Id = r.Id,
                                       Client = u.Fullname,
                                       Package = p.PackageName,
                                       StartDateTime = r.StartDateTime,
                                       EndDateTime = r.EndDateTime,
                                       Request = r.Request,
                                       Reply = r.Reply,
                                       Status = r.Status,
                                       Feedback = r.Feedback
                                   })
                                       .SingleOrDefaultAsync(r => r.Id == id);

            if (reservationList == null)
            {
                return NotFound();
            }

            return View(await reservationList);
        }


        // GET: Reservation/Add

        [HttpGet]
        public async Task<IActionResult> Add(string id = null)
        {
            var model = new ReservationViewModel();

            if (id == null)
            {
                model.ApplicationUserId = _userManager.GetUserId(User);
            }
            else
            {
                model.ApplicationUserId = id;
            }

            model.Id = 0;
            model.ApplicationUser = await _context.Users
                                .Where(u => u.Id == model.ApplicationUserId)
                                .Select(u => u.Fullname)
                                .SingleOrDefaultAsync();

            var package = _context.Package.OrderBy(p => p.Id).Select(p => new { Id = p.Id, Value = p.PackageName, Price = p.Price });
            model.PackageList = new SelectList(package, "Id", "Value");

            ModelState.Clear(); // Todo:Added to resolve ModelState bug that causing Id issue when saving from user master

            return View(model);
        }

        // POST: Reservation/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ReservationViewModel model)
        {
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                
                int? createdBy = model.ApplicationUserId == _userManager.GetUserId(User) ? 0 : 1;

                var reservation = new Reservation {
                    ApplicationUserId = model.ApplicationUserId,
                    PackageId = model.PackageId,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    Request = model.Request,
                    CreatedBy = createdBy
                };

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(model);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await (from r in _context.Reservation
                               join u in _context.Users on r.ApplicationUserId equals u.Id

                               where (r.Id == id && u.Id == _userManager.GetUserId(User))
                               select new ReservationViewModel
                               {
                                   Id = r.Id,
                                   PackageId = r.PackageId,
                                   ApplicationUser = u.Fullname,
                                   StartDateTime = r.StartDateTime,
                                   EndDateTime = r.EndDateTime,
                                   Request = r.Request
                               }).SingleOrDefaultAsync(m => m.Id == id);

            //var applicationUser = _context.Users.Where(c => c.RoleId == 3).OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Firstname + " " + x.Lastname });
            var package = _context.Package.OrderBy(p => p.Id).Select(p => new { Id = p.Id, Value = p.PackageName });
            //model.ApplicationUserList = new SelectList(applicationUser, "Id", "Value");
            model.PackageList = new SelectList(package, "Id", "Value");

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var reservation = _context.Reservation.SingleOrDefault(r => r.Id == id);
                    reservation.PackageId = model.PackageId;
                    reservation.StartDateTime = model.StartDateTime;
                    reservation.EndDateTime = model.EndDateTime;
                    reservation.Request = model.Request;

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }


        // GET: Reservation/Calculate
        [HttpGet]
        public IActionResult Calculate()
        {

            var model = new ReservationViewModel();

            model.ApplicationUserId = _userManager.GetUserId(User);

            var package = _context.Package.OrderBy(p => p.Id)
                .Select(p => new {
                    Id = p.Id,
                    Value = p.PackageName});

            model.PackageList = new SelectList(package, "Id", "Value");

            return View(model);
        }

        // POST: Reservation/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Calculate(ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {

                double price = await _context.Package.Where(p => p.Id == model.PackageId).Select(p => p.Price).SingleOrDefaultAsync();

                double tourCost = 0;
                DateTime startDate = Convert.ToDateTime(model.StartDateTime);
                DateTime endDate = Convert.ToDateTime(model.EndDateTime);

                int days = endDate.Subtract(startDate).Days + 1;

                try
                {
                    tourCost = days * price;
                }
                catch
                {
                    throw;
                }
                

                model.ApplicationUserId = _userManager.GetUserId(User);

                var package = _context.Package.OrderBy(p => p.Id)
                    .Select(p => new {
                        Id = p.Id,
                        Value = p.PackageName
                    });

                model.PackageList = new SelectList(package, "Id", "Value");

                model.NetTotal = tourCost.ToString("#.00 €");
                ModelState.Clear();

                return View(model);
            }
            return View(model);
        }


        // GET: Reservation/Review/5
        [Authorize(Policy = "BranchPolicy")]
        public async Task<IActionResult> Review(int? id)
        {
            ViewData["SubTitle"] = "Review & Feedback";

            if (id == null)
            {
                return NotFound();
            }

            var model = await (from r in _context.Reservation
                               join u in _context.Users on r.ApplicationUserId equals u.Id
                               join p in _context.Package on r.PackageId equals p.Id
                               where (r.Id == id)
                               select new ReservationReviewViewModel
                               {
                                   Id = r.Id,
                                   ApplicationUserId = r.ApplicationUserId,
                                   ApplicationUser = u.Fullname,
                                   PackageId = r.PackageId,
                                   Package = p.PackageName,
                                   StartDateTime = r.StartDateTime,
                                   EndDateTime = r.EndDateTime,
                                   Status = r.Status,
                                   Request = r.Request,
                                   Reply = r.Reply,
                                   CreatedBy = r.CreatedBy,
                                   Feedback = r.Feedback
                               }).SingleOrDefaultAsync(m => m.Id == id);



            var driver = from d in _context.Driver
                         join u in _context.Users on d.ApplicationUserId equals u.Id
                         select new { Id = d.Id, Value = u.Fullname };
            //select new { d.Id, u}).Where(s => _context.Schedule.Where(sx => sx.DriverId == cx.))


            model.DriverList = new SelectList(driver, "Id", "Value");


            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }


        // POST: Reservation/Review/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HQPolicy")]
        public async Task<IActionResult> Review(string submit, int id, ReservationReviewViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int? _status = null;
                    if (submit == "Accept")
                    {
                        _status = 1;
                    }
                    else if (submit == "Reject")
                    {
                        _status = 0;
                    }

                    var reservation = new Reservation
                    {
                        Id = model.Id,
                        ApplicationUserId = model.ApplicationUserId,
                        PackageId = model.PackageId,
                        StartDateTime = model.StartDateTime,
                        EndDateTime = model.EndDateTime,
                        Request = model.Request,
                        Reply = model.Reply,
                        CreatedBy = model.CreatedBy,
                        Status = _status
                    };

                    _context.Update(reservation);

                    if (_status == 1)
                    {
                        var schedule = new Schedule
                        {
                            ReservationId = reservation.Id,
                            DriverId = model.DriverId
                        };

                        _context.Add(schedule);
   
                    }

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("All");
            }
            return View(model);
        }


        // GET: Reservation/Feedback/5
        public IActionResult Feedback(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = _context.Reservation.SingleOrDefault(r => r.Id == id && r.ApplicationUserId == _userManager.GetUserId(User));

            if (model == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Feedback(int id, ReservationFeedbackViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var reservation = _context.Reservation.SingleOrDefault(r => r.Id == id);
                    reservation.Feedback = model.Feedback;
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }




        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.SingleOrDefaultAsync(m => m.Id == id && m.ApplicationUserId == _userManager.GetUserId(User));
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.SingleOrDefaultAsync(m => m.Id == id);
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
