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
        public OwnerController(ApplicationDBContext db, ILogger<OwnerController> logger)
        {
            _db = db;
        }

        // GET
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ownerInfo = await _db.Owners.FirstOrDefaultAsync(o => o.UserId.Equals(int.Parse(userId)));

            return View(ownerInfo);
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
        public async Task<IActionResult> Create(Owner model)
        {

            //User of the login session's Id #
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            if(userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            model.UserId = int.Parse(userId);
            _db.Owners.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Dashboard");



        }

        // GET
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var ownerFromDb = _db.Owners.Find(id);

            if(ownerFromDb==null)
            { 
                return NotFound(); 
            }
            return View(ownerFromDb);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Owner obj)
        {
            //if (obj.Name == obj.Email)
            //{
            //    ModelState.AddModelError("Name", "Name cant be the same as the email.");
            //}
            if (ModelState.IsValid)
            {
                _db.Owners.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
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
            
            return View(obj);
        }
    }
}
