using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetSitApp.Data;
using PetSitApp.Models;

namespace PetSitApp.Controllers
{
    public class OwnerController : Controller
    {
        private readonly ApplicationDBContext _db;
        public OwnerController(ApplicationDBContext db)
        {
            _db = db;
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
        public IActionResult Create(Owner obj)
        {
            if(obj.Name==obj.Email)
            {
                ModelState.AddModelError("Name", "Name cant be the same as the email.");
            }
            if (ModelState.IsValid)
            {
                _db.Owners.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Get the validation errors from ModelState
            var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage)
                                                     .ToList();

            // Optional: Log or display the validation errors for debugging purposes
            foreach (var error in validationErrors)
            {
                Console.WriteLine(error);
            }
            return View(obj);
            
        }

        // GET
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
    }
}
