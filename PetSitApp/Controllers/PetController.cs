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

        // GET
        public IActionResult CreatePet()
        {
            
            return View();
        }
        // GET
        public async Task<IActionResult> EditPet(int? id) 
        {
            var petFromDb = await _db.Pets.FindAsync(id);

            if (petFromDb == null)
            {
                return NotFound();
            }
          
            return View(petFromDb);
        }
        // GET
        public async Task<IActionResult> DeletePet(int? id)
        {
            var petFromDb = await _db.Pets.FindAsync(id);
            if (petFromDb == null)
            {
                return NotFound();
            }
            return View(petFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPet(int id, Pet model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Owner");
            if (ModelState.IsValid)
            {
                _db.Update(model);
                await _db.SaveChangesAsync();
                TempData["success"] = "Successfully updated pet";
                return RedirectToAction("Dashboard", "Owner");
            }
            return View(model);  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePet(int id, Pet model)
        {
            var pet = await _db.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            _db.Remove(pet);
            await _db.SaveChangesAsync();
            TempData["success"] = "Successfully deleted Pet";
            return RedirectToAction("Dashboard", "Owner");

        }
    }
}
