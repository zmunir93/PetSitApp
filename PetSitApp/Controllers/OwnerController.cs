using Microsoft.AspNetCore.Mvc;
using PetSitApp.Data;

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
            var objOwnerList = _db.Owners.ToList();
            return View();
        }
    }
}
