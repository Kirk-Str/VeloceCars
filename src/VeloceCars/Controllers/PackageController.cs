using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeloceCars.Data;
using VeloceCars.Models;
using Microsoft.AspNetCore.Authorization;
using VeloceCars.Models.PackageViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace VeloceCars.Controllers
{
    [Authorize(Policy = "HQPolicy")]
    public class PackageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PackageController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Package
        public async Task<IActionResult> Index()
        {
            var packageList = _context.Package.Select(p => new PackageListViewModel
            {
                Id = p.Id,
                PackageName = p.PackageName,
                Route = p.Route,
                Price = p.Price
            });

            return View(await packageList.ToListAsync());
        }

        // GET: Package/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageList = _context.Package
                .Where(p => p.Id == id)
                .Select(p => new PackageListViewModel
            {
                Id = p.Id,
                PackageName = p.PackageName,
                Route = p.Route,
                Price = p.Price
            });
            

            if (packageList == null)
            {
                return NotFound();
            }

            return View(await packageList.SingleOrDefaultAsync());
        }

        // GET: Offers
        [AllowAnonymous]
        public async Task<IActionResult> Offers()
        {
            
            var packageList = _context.Package.Select(p => new PackageListViewModel
            {
                Id = p.Id,
                PackageName = p.PackageName,
                Route = p.Route,
                Price = p.Price,
                Image = p.Image
            });

            return View(await packageList.ToListAsync());
        }


        // GET: Package/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: Package/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Package model, IFormFile image)
        {
            if (ModelState.IsValid)
            {

                MemoryStream ms = new MemoryStream();
                await image.OpenReadStream().CopyToAsync(ms);

                var package = new Package
                {
                    Id = model.Id,
                    PackageName = model.PackageName,
                    Route = model.Route,
                    Price = model.Price,
                    Image = ms.ToArray()
                };

                _context.Add(package);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }


        //private byte[] ConvertToBytes(IFormFile file)
        //{
        //    Stream stream = file.OpenReadStream();
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        stream.CopyTo(memoryStream);
        //        return memoryStream.ToArray();
        //    }
        //}

        // GET: Package/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var package = await _context.Package.SingleOrDefaultAsync(m => m.Id == id);
            if (package == null)
            {
                return NotFound();
            }
            return View(package);
        }

        // POST: Package/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Package model, IFormFile image)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    await image.OpenReadStream().CopyToAsync(ms);

                    var package = await _context.Package.SingleOrDefaultAsync(m => m.Id == id);
                    package.PackageName = model.PackageName;
                    package.Route = model.Route;
                    package.Price = model.Price ;
                    package.Image = ms.ToArray();

                    _context.Update(package);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageExists(model.Id))
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

        // GET: Package/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageList = _context.Package
                .Where(p => p.Id == id)
                .Select(p => new PackageListViewModel
                {
                    Id = p.Id,
                    PackageName = p.PackageName,
                    Route = p.Route,
                    Price = p.Price
                });

            if (packageList == null)
            {
                return NotFound();
            }

            return View(await packageList.SingleOrDefaultAsync());
        }

        // POST: Package/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var package = await _context.Package.SingleOrDefaultAsync(m => m.Id == id);
            _context.Package.Remove(package);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PackageExists(int id)
        {
            return _context.Package.Any(e => e.Id == id);
        }
    }
}
