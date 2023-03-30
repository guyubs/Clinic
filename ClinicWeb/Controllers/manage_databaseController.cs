using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    public class manage_databaseController : Controller
    {
        public IActionResult Index()
        {
            // 登录验证，若cookies中没有用户，则显示登录页面
            if (!HttpContext.Request.Cookies.TryGetValue("UserId", out string userId))
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
    }
}
