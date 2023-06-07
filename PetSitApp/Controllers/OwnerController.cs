using Microsoft.AspNetCore.Mvc;
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
            return View(obj);
        }
    }
}
