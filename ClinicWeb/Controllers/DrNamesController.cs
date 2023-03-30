using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWeb.Data;
using ClinicWeb.Models;

namespace ClinicWeb.Controllers
{
    public class DrNamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DrNamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DrNames
        public async Task<IActionResult> Index()
        {
            // 登录验证，若cookies中没有用户，则显示登录页面
            if (!HttpContext.Request.Cookies.TryGetValue("UserId", out string userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var applicationDbContext = _context.DrNames.Include(d => d.DrAddr).Include(d => d.Speciality).Include(d => d.Title);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DrNames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DrNames == null)
            {
                return NotFound();
            }

            var drName = await _context.DrNames
                .Include(d => d.DrAddr)
                .Include(d => d.Speciality)
                .Include(d => d.Title)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drName == null)
            {
                return NotFound();
            }

            return View(drName);
        }

        // GET: DrNames/Create
        public IActionResult Create()
        {
            ViewData["DrAddrId"] = new SelectList(_context.DrAddresses, "Id", "Street1");
            ViewData["SpecialityId"] = new SelectList(_context.Specialists, "Id", "SpecialityName");
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "TitleName");
            return View();
        }

        // POST: DrNames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,SpecialityId,DrAddrId,TitleId,Note,CreatedBy,CreateDateTime")] DrName drName)
        {
            if (ModelState.IsValid)
            {
                // 设置 CreateDateTime 属性为当前时间
                drName.CreateDateTime = DateTime.Now;

                // get selected values from dropdowns （检查是否能被解析，比如外键为空。此处双保险，因为在前端已经确定外键不为空）
                int.TryParse(Request.Form["SpecialityId"], out int selectedSpecialityId);
                int.TryParse(Request.Form["DrAddrId"], out int selectedAddrId);
                int.TryParse(Request.Form["TitleId"], out int selectedTitleId);

                // 解析，存入表中
                drName.SpecialityId = selectedSpecialityId;
                drName.DrAddrId = selectedAddrId;
                drName.TitleId = selectedTitleId;

                _context.Add(drName);
                await _context.SaveChangesAsync();
                TempData["success"] = "Doctor created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // 用户在表单验证失败时显示。
            ViewData["DrAddrId"] = new SelectList(_context.DrAddresses, "Id", "Street1", drName.DrAddrId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialists, "Id", "SpecialityName", drName.SpecialityId);
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "TitleName", drName.TitleId);
            return View(drName);
        }

        // GET: DrNames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DrNames == null)
            {
                return NotFound();
            }

            var drName = await _context.DrNames.FindAsync(id);
            if (drName == null)
            {
                return NotFound();
            }

            ViewData["DrAddrId"] = new SelectList(_context.DrAddresses, "Id", "Street1", drName.DrAddrId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialists, "Id", "SpecialityName", drName.SpecialityId);
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "TitleName", drName.TitleId);
            return View(drName);
        }

        // POST: DrNames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,SpecialityId,DrAddrId,TitleId,Note,Deleted,CreateDateTime,ModifiedBy,ModifiedDateTime")] DrName drName)
        {
            if (id != drName.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 设置 ModifiedDateTime 属性为当前时间
                    drName.ModifiedDateTime = DateTime.Now;

                    // get selected values from dropdowns
                    var selectedSpecialityId = Request.Form["SpecialityId"];
                    var selectedAddrId = Request.Form["DrAddrId"];
                    var selectedTitleId = Request.Form["TitleId"];


                    // 解析，存入表中（不需要检查是否能被解析，因为修改时外键不会出现空值）
                    drName.SpecialityId = int.Parse(selectedSpecialityId);
                    drName.DrAddrId = int.Parse(selectedAddrId);
                    drName.TitleId = int.Parse(selectedTitleId);

                    _context.Update(drName);
                    TempData["success"] = "Doctor edited successfully!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrNameExists(drName.Id))
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
            ViewData["DrAddrId"] = new SelectList(_context.DrAddresses, "Id", "Street1", drName.DrAddrId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialists, "Id", "SpecialityName", drName.SpecialityId);
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "TitleName", drName.TitleId);
            return View(drName);
        }

        // GET: DrNames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DrNames == null)
            {
                return NotFound();
            }

            var drName = await _context.DrNames
                .Include(d => d.DrAddr)
                .Include(d => d.Speciality)
                .Include(d => d.Title)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drName == null)
            {
                return NotFound();
            }

            return View(drName);
        }

        // POST: DrNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DrNames == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DrNames'  is null.");
            }
            var drName = await _context.DrNames.FindAsync(id);
            if (drName != null)
            {
                _context.DrNames.Remove(drName);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrNameExists(int id)
        {
          return (_context.DrNames?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
