using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSitApp.Data;
using PetSitApp.Models;
using System.Security.Claims;

namespace PetSitApp.Controllers
{
    public class OwnerController : Controller
    {
        private readonly ApplicationDBContext _db;
        public OwnerController(ApplicationDBContext db)
        {
            _db = db;
        }

        // GET
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ownerInfo = await _db.Owners
                .Include(o => o.Pets)
                .ThenInclude(p => p.PetPictures)
                .FirstOrDefaultAsync(o => o.UserId.Equals(int.Parse(userId)));

            
            return View(ownerInfo);
        }

        public async Task<FileStreamResult> GetOwnerImage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var owner = await _db.Owners.FirstOrDefaultAsync(o => o.UserId.Equals(int.Parse(userId)));


            var memStream = new MemoryStream(owner.ProfilePicture);
            memStream.Position = 0;

            return File(memStream, "image/jpeg");
        }

        [HttpGet("GetPetImage/{petId:int}")]
        public async Task<IActionResult> GetPetImage(int petId)
        {
            var petPicture = await _db.PetPictures.FirstOrDefaultAsync(pp => pp.PetId == petId);

            if(petPicture != null && petPicture.Picture != null)
            {
                var memStream = new MemoryStream(petPicture.Picture);
                memStream.Position = 0;
                return File(memStream, "image/jpeg");
            }

            
            return Redirect("https://placehold.jp/3d4070/ffffff/200x200.jpg?text=No%20Image");
        }
        
        public IActionResult Index()
        {
            IEnumerable<Owner> objOwnerList = _db.Owners;
            return View(objOwnerList);
        }

        // GET
        public IActionResult Create()
        {
           
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(2621440)] // Limit to 2.5MB
        public async Task<IActionResult> Create(Owner model, IFormFile ProfilePicture)
        {

            //User of the login session's Id #
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            if(userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ProfilePicture.CopyToAsync(memoryStream);
                    model.ProfilePicture = memoryStream.ToArray();
                }
            };

            model.UserId = int.Parse(userId);
            _db.Owners.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Dashboard");

        }

        // GET
        [Authorize(Roles = "Owner")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ownerFromDb = await _db.Owners.FindAsync(id);

            if (ownerFromDb == null) 
            {
                return NotFound();
            }
            return View(ownerFromDb);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Owner model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                _db.Owners.Update(model);
                await _db.SaveChangesAsync();
                TempData["success"] = "Info Successfully Updated";
                return RedirectToAction("Dashboard");
            }
            
            return View(model);
                
        }

        // GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ownerFromDb = _db.Owners.Find(id);

            if (ownerFromDb == null)
            {
                return NotFound();
            }
            return View(ownerFromDb);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Owners.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Owners.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            
        }
    }
}
