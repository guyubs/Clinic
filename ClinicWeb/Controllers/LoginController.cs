using ClinicWeb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
            // 如果用户已登录，直接跳转到 Panel 页面
            if (HttpContext.Request.Cookies.TryGetValue("UserId", out string userId))
            {
                return RedirectToAction("Index", "Panel");
            }
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
                    // 登录成功，设置 Cookie
                    var options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    };
                    Response.Cookies.Append("UserId", user.Id.ToString(), options);

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

        public IActionResult Logout()
        {
            // 清除 Cookie
            Response.Cookies.Delete("UserId");

            // 设置成功消息
            TempData["Message"] = "Logout successful.";

            return RedirectToAction("Index", "Home");
        }
    }
}
