using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeloceCars.Data;
using VeloceCars.Models;
using VeloceCars.Models.DriverViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace VeloceCars.Controllers
{
    [Authorize(Policy = "BranchPolicy")]
    public class DriverController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriverController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Driver
        public async Task<IActionResult> Index()
        {

           var driverList = (from d in _context.Driver
                              join u in _context.Users on d.ApplicationUserId equals u.Id
                              join v in _context.Vehicle on d.VehicleId equals v.Id
                              
                              select new DriverListViewModel
                              {
                                  Id = d.Id,
                                  ApplicationUser = u.Fullname,
                                  Vehicle = v.VehicleName,
                                  Status = (from s in _context.Schedule where s.DriverId == d.Id && s.Status !=1 select s).Count() > 0 ? 1 : 0
                              }).ToListAsync();


            return View(await driverList);
        }

        // GET: Driver/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = (from d in _context.Driver
                                join u in _context.Users on d.ApplicationUserId equals u.Id
                                join v in _context.Vehicle on d.VehicleId equals v.Id
                                select new DriverListViewModel {
                                    Id = d.Id,
                                    ApplicationUser = u.Fullname,
                                    Vehicle = v.VehicleName,
                                    Status = (from s in _context.Schedule where s.DriverId == d.Id && s.Status != 1 select s).Count() > 0 ? 1 : 0
                                }).SingleOrDefaultAsync(d => d.Id == id);

            if (driver == null)
            {
                return NotFound();
            }

            return View(await driver);
        }

        // GET: Driver/Add
        public IActionResult Add()
        {
            var vehicle = _context.Vehicle
                .Where(v => !_context.Driver.Any(d => d.VehicleId == v.Id))
                .Select(v => new { Id = v.Id, Value = v.Model });

            var applicationUser = from u in _context.Users
                                  join ur in _context.UserRoles on u.Id equals ur.UserId
                                  join r in _context.Roles on ur.RoleId equals r.Id
                                  where ((r.Name == "Driver") && !(from a in _context.Driver select a.ApplicationUserId).Contains(u.Id))
                                  orderby u.Id
                                  select new { Id = u.Id, Value = u.Fullname };

            var model = new DriverViewModel();
            model.VehicleList = new SelectList(vehicle, "Id", "Value");
            model.ApplicationUserList = new SelectList(applicationUser, "Id", "Value");

            return View(model);
        }

        // POST: Driver/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DriverViewModel model)
        {
            if (ModelState.IsValid)
            {
                var driver = new Driver { Id = model.Id, ApplicationUserId = model.ApplicationUserId, VehicleId = model.VehicleId };
                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Driver/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = (from d in _context.Driver
                where (d.Id == id)
                select new DriverViewModel
                {
                    Id = d.Id,
                    ApplicationUserId = d.ApplicationUserId,
                    VehicleId = d.VehicleId
                }).SingleOrDefaultAsync();

            if (driver == null)
            {
                return NotFound();
            }
            return View(await driver);
        }

        // POST: Driver/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DriverViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(model.Id))
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

        // GET: Driver/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = (from d in _context.Driver
                             join u in _context.Users on d.ApplicationUserId equals u.Id
                             join v in _context.Vehicle on d.VehicleId equals v.Id
                             select new DriverListViewModel { Id = d.Id, ApplicationUser = u.Fullname, Vehicle = v.VehicleName })
                             .Where(d => d.Id == id)
                             .SingleOrDefaultAsync();

            if (driver == null)
            {
                return NotFound();
            }

            return View(await driver);
        }

        // POST: Driver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Driver.SingleOrDefaultAsync(m => m.Id == id);
            _context.Driver.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DriverExists(int id)
        {
            return _context.Driver.Any(e => e.Id == id);
        }
    }
}
