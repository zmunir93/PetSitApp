using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSitApp.Data;
using System.Security.Claims;

namespace PetSitApp.Controllers
{
    public class SitterController : Controller
    {
        private readonly ApplicationDBContext _db;
        public SitterController(ApplicationDBContext db) 
        {
            _db = db;
        }

        [Authorize(Roles = "Sitter")]
        public async Task<IActionResult> SitterDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ownerInfo = await _db.Sitters.FirstOrDefaultAsync(s => s.UserId.Equals(int.Parse(userId)));

            return View(ownerInfo);
        }
    }
}
