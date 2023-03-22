using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    public class PanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
