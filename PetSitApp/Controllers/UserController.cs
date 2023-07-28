using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PetSitApp.Data;
using PetSitApp.Models;

namespace PetSitApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _db;
        public UserController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        } 

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                // checks to see if user exist in database
                var existingUser = _db.Users.FirstOrDefault(u => u.Username.ToLower().Equals(model.Username.ToLower()));

                // if they do exist, error is thrown
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username already exist");
                    return View(model);
                }

                // model.Password is encrypted
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // newUserObj created to pass info to be added to database
                var newUserObj = new User
                {
                    Username = model.Username,
                    Password = passwordHash
                };

                // info is added and then saved. Redirected to index page
                _db.Users.Add(newUserObj);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            else
            {
                return View(model);
            }
        }
    }
}
