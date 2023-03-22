using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    public class manage_databaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
