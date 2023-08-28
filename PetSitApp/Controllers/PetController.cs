using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSitApp.Data;
using PetSitApp.Models;
using System.Security.Claims;

namespace PetSitApp.Controllers
{
    public class PetController : Controller
    {
        private readonly ApplicationDBContext _db;

        public PetController(ApplicationDBContext db)
        {
            _db = db;
        }

        
        public IActionResult CreatePet()
        {
            
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> CreatePet(Pet model)
        {
            ModelState.Remove("Owner");
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var owner = await _db.Owners.FirstOrDefaultAsync(o => o.UserId == int.Parse(userId));

                if (owner == null)
                {
                    return RedirectToAction("Error");
                }

                model.OwnerId = owner.Id;
                _db.Pets.Add(model);
                await _db.SaveChangesAsync();

                return RedirectToAction("Dashboard", "Owner");
            }
            return View(model);      
        }
    }
}
