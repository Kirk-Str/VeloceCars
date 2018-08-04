using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VeloceCars.Data;
using VeloceCars.Models.PackageViewModels;
using Microsoft.EntityFrameworkCore;

namespace VeloceCars.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var packageList = _context.Package.Select(p => new PackageListViewModel
            {
                Id = p.Id,
                PackageName = p.PackageName,
                Route = p.Route,
                Price = p.Price,
                Image = p.Image
            }).Take(3);

            return View(await packageList.ToListAsync());
        }

        public IActionResult ThisProject()
        {
             return View();
        }

        public IActionResult StudentInfo()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "We couldn't be more happier to serve our customer";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Veloce Cars - Italy";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
