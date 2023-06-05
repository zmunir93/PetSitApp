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
    }
}
