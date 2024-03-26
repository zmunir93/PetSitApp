using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetSitApp.Data;

namespace PetSitApp.Controllers
{
    public class LoginController : Controller
    {
        //private readonly ApplicationDBContext _db;
        //public LoginController(ApplicationDBContext db)
        //{
        //    _db = db;
        //}

        //[HttpPost]
        //public async Task<IActionResult> Index(string username, string password)
        //{
        //    var user = _db.Users.FirstOrDefault(user => user.Username.ToLower().Equals(username));

        //    //if (result.Succeeded)
        //    //{
        //    //    return RedirectToAction("index", "home");
        //    //}

        //    return View();
        //}
        public IActionResult Index()
        {
            return View();
        }

        
    }


}
