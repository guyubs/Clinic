using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWeb.Data;
using ClinicWeb.Models;
using System.Net;
using System.Xml.Linq;

namespace ClinicWeb.Controllers
{
    public class SpecialistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpecialistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Specialists
        public async Task<IActionResult> Index()
        {
            // 登录验证，若cookies中没有用户，则显示登录页面
            if (!HttpContext.Request.Cookies.TryGetValue("UserId", out string userId))
            {
                return RedirectToAction("Index", "Login");
            }

            return _context.Specialists != null ? 
                          View(await _context.Specialists.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Specialists'  is null.");
        }

        // GET: Specialists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Specialists == null)
            {
                return NotFound();
            }

            var specialist = await _context.Specialists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialist == null)
            {
                return NotFound();
            }

            return View(specialist);
        }

        // GET: Specialists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SpecialityName,Note,Deleted,CreatedBy,ModifiedBy,CreateDateTime,ModifiedDateTime")] Specialist specialist)
        {
            if (ModelState.IsValid)
            {
                // 设置 ModifiedDateTime 属性为当前时间
                specialist.CreateDateTime = DateTime.Now;

                TempData["success"] = "Specialist created successfully!";
                _context.Add(specialist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialist);
        }

        // GET: Specialists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Specialists == null)
            {
                return NotFound();
            }

            var specialist = await _context.Specialists.FindAsync(id);
            if (specialist == null)
            {
                return NotFound();
            }
            return View(specialist);
        }

        // POST: Specialists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SpecialityName,Note,Deleted,CreatedBy,ModifiedBy,CreateDateTime,ModifiedDateTime")] Specialist specialist)
        {
            if (id != specialist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 设置 ModifiedDateTime 属性为当前时间
                    specialist.ModifiedDateTime = DateTime.Now;

                    TempData["success"] = "Specialist edited successfully!";
                    _context.Update(specialist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialistExists(specialist.Id))
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
            return View(specialist);
        }

        // GET: Specialists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Specialists == null)
            {
                return NotFound();
            }

            var specialist = await _context.Specialists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialist == null)
            {
                return NotFound();
            }

            return View(specialist);
        }

        // POST: Specialists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Specialists == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Specialists'  is null.");
            }
            var specialist = await _context.Specialists.FindAsync(id);
            if (specialist != null)
            {
                _context.Specialists.Remove(specialist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialistExists(int id)
        {
          return (_context.Specialists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
