using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWeb.Data;
using ClinicWeb.Models;
using System.Xml.Linq;

namespace ClinicWeb.Controllers
{
    public class DrAddressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DrAddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DrAddresses
        public async Task<IActionResult> Index()
        {

            // 登录验证，若cookies中没有用户，则显示登录页面
            if (!HttpContext.Request.Cookies.TryGetValue("UserId", out string userId))
            {
                return RedirectToAction("Index", "Login");
            }

            return _context.DrAddresses != null ? 
                          View(await _context.DrAddresses.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DrAddresses'  is null.");
        }

        // GET: DrAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DrAddresses == null)
            {
                return NotFound();
            }

            var drAddress = await _context.DrAddresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drAddress == null)
            {
                return NotFound();
            }

            return View(drAddress);
        }

        // GET: DrAddresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DrAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Street1,Street2,City,State,Zip,Tel,Fax,Deleted,CreatedBy,ModifiedBy,CreateDateTime,ModifiedDateTime")] DrAddress drAddress)
        {
            if (ModelState.IsValid)
            {
                // 设置 CreateDateTime 属性为当前时间
                drAddress.CreateDateTime = DateTime.Now;

                // 设置 CreatedBy
                drAddress.CreatedBy = HttpContext.Request.Cookies["Username"];

                TempData["success"] = "Address created successfully!";
                _context.Add(drAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(drAddress);
        }

        // GET: DrAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DrAddresses == null)
            {
                return NotFound();
            }

            var drAddress = await _context.DrAddresses.FindAsync(id);
            if (drAddress == null)
            {
                return NotFound();
            }
            return View(drAddress);
        }

        // POST: DrAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Street1,Street2,City,State,Zip,Tel,Fax,Deleted,CreatedBy,ModifiedBy,CreateDateTime,ModifiedDateTime")] DrAddress drAddress)
        {
            if (id != drAddress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 设置 ModifiedDateTime 属性为当前时间
                    drAddress.ModifiedDateTime = DateTime.Now;

                    // 设置 ModifiedBy
                    drAddress.ModifiedBy = HttpContext.Request.Cookies["Username"];

                    TempData["success"] = "Address edited successfully!";
                    _context.Update(drAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrAddressExists(drAddress.Id))
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
            return View(drAddress);
        }

        // GET: DrAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DrAddresses == null)
            {
                return NotFound();
            }

            var drAddress = await _context.DrAddresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drAddress == null)
            {
                return NotFound();
            }

            return View(drAddress);
        }

        // POST: DrAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DrAddresses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DrAddresses'  is null.");
            }
            var drAddress = await _context.DrAddresses.FindAsync(id);
            if (drAddress != null)
            {
                _context.DrAddresses.Remove(drAddress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrAddressExists(int id)
        {
          return (_context.DrAddresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
