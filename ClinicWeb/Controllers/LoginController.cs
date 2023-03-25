using ClinicWeb.Data;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);
                if (user != null)
                {
                    // 登录成功
                    return RedirectToAction("Index", "Panel");
                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码不正确");
                    return View("Index", model);
                }
            }

            // 登录失败
            return View("Index", model);
        }
    }
}

