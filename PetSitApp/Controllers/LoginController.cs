using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetSitApp.Data;

namespace PetSitApp.Controllers
{
    public class LoginController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        
    }


}
