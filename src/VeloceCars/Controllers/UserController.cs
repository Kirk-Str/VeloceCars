using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeloceCars.Data;
using VeloceCars.Models.AccountViewModels;
using Microsoft.EntityFrameworkCore;
using VeloceCars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using VeloceCars.Models.UserViewModels;
using Microsoft.AspNetCore.Authorization;

namespace VeloceCars.Controllers
{
    [Authorize(Policy = "HQPolicy")]
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: User
        public async Task<IActionResult> Index()
        {
            var applicationUserList = from u in _context.Users
                                      join ur in _context.UserRoles on u.Id equals ur.UserId
                                      join r in _context.Roles on ur.RoleId equals r.Id
                                      orderby u.LockoutEnd, u.RoleId descending
                                      select new ApplicationUserListViewModel
                                      {
                                          Id = u.Id,
                                          Firstname = u.Firstname,
                                          Lastname = u.Lastname,
                                          Email = u.Email,
                                          CreatedBy = u.CreatedBy,
                                          LockoutEnd = u.LockoutEnd,
                                          RoleType = r.Name
                                      };

            return View(await applicationUserList.ToListAsync());
        }

        // GET: Account/Disable/5
        [HttpGet]
        public async Task<IActionResult> Deactivate(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await (from u in _context.Users
                                         join ur in _context.UserRoles on u.Id equals ur.UserId
                                         join r in _context.Roles on ur.RoleId equals r.Id
                                         where u.Id == id
                                         select new ApplicationUserListViewModel
                                         {
                                             Id = u.Id,
                                             Firstname = u.Firstname,
                                             Lastname = u.Lastname,
                                             Email = u.Email,
                                             CreatedBy = u.CreatedBy,
                                             RoleType = r.Name
                                         })
                                      .SingleOrDefaultAsync();

            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: Account/Disable/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(string id, ApplicationUserListViewModel model)
        {
            var applicationUser = await _userManager.FindByIdAsync(model.Id);
            await _userManager.SetLockoutEndDateAsync(applicationUser, DateTime.Today.AddYears(10));

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        //
        // GET: /Account/RegisterUser
        [HttpGet]
        public IActionResult RegisterUser(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var roleList = _context.Roles.
                Select(r => new {
                    Id = r.Id,
                    Value = r.Name
                });

            var model = new RegisterUserViewModel();
            model.RoleList = new SelectList(roleList, "Id", "Value");

            return View(model);
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Firstname = model.Firstname, Lastname = model.Lastname, Address = model.Address, ContactHome = model.ContactHome, ContactOffice = model.ContactOffice, CreatedBy = 1 };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, GetRoleNameById(model.RoleId));
                    return RedirectToAction("index");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private string GetRoleNameById(string id)
        {
            return _context.Roles
                .Where(r => r.Id == id)
                .Select(r => r.Name)
                .SingleOrDefault();
        }

    }
}