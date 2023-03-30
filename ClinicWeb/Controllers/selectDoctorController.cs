using ClinicWeb.Data;
using ClinicWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClinicWeb.Controllers
{
    public class selectDoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public selectDoctorController(ApplicationDbContext context)
        {
            _context = context;
        }


        // 注意！此处一定要把specialities返回给View， 这样就能将 specialities 列表传递给视图，并在视图中使用 @model List<Doctors.Models.Specialist> 来接收这个列表。
        public IActionResult Index()
        {
            // 登录验证，若cookies中没有用户，则显示登录页面
            if (!HttpContext.Request.Cookies.TryGetValue("UserId", out string userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var specialities = _context.Specialists
                .Where(s => s.Deleted == null || s.Deleted == false)
                .OrderBy(s => s.SpecialityName)
                .ToList();

            ViewBag.Specialities = specialities;

            return View("~/Views/Panel/selectDoctor/Index.cshtml");
        }


        // 处理搜索操作并返回DrInfo视图的代码
        public IActionResult DrInfo(string specialityName)
        {
            var doctors = (from dn in _context.DrNames
                           join sp in _context.Specialists on dn.SpecialityId equals sp.Id
                           join da in _context.DrAddresses on dn.DrAddrId equals da.Id
                           where (sp.Deleted == false) && sp.SpecialityName == specialityName
                           select new DoctorViewModel
                           {
                               LastName = dn.LastName,
                               FirstName = dn.FirstName,
                               Street1 = da.Street1,
                               Street2 = da.Street2,
                               City = da.City,
                               State = da.State,
                               Zip = da.Zip,
                               Tel = da.Tel
                           }).ToList();

            // 检查 doctors 是否为空
            if (doctors == null)
            {
                return NotFound();
            }

            // 传递 doctors 到视图
            return View("~/Views/Panel/selectDoctor/DrInfo.cshtml", doctors);
        }



    }
}
