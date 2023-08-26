using Microsoft.AspNetCore.Mvc;

namespace PetSitApp.Controllers
{
    public class PetController : Controller
    {
        public IActionResult CreatePet()
        {
            return View();
        }
    }
}
