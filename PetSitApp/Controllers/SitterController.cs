using Microsoft.AspNetCore.Mvc;

namespace PetSitApp.Controllers
{
    public class SitterController : Controller
    {
        public IActionResult SitterDashboard()
        {
            return View();
        }
    }
}
