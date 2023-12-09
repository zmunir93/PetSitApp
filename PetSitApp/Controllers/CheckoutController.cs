using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSitApp.Models;
using PetSitApp.ViewModels;
using Stripe.Checkout;
using System.Globalization;
using System.Security.Claims;

namespace PetSitApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PetSitAppContext _db;
        public CheckoutController(PetSitAppContext db)
        {
            _db = db;
        }

        public IActionResult Confirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus == "paid")
            {
                return RedirectToAction("Success");
            }
            return View("Failure");
        }

        /////////////////////////// GET
        public async Task<IActionResult> Success(Reservation rerservation)
        {
            
            var startDate = DateTime.ParseExact(HttpContext.Session.GetString("StartDate"), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(HttpContext.Session.GetString("EndDate"), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            var sitterId = HttpContext.Session.GetInt32("SitterId") ?? 0;
            var sessionId = HttpContext.Session.GetString("SessionId");
            var jobtype = HttpContext.Session.GetString("JobType");
            
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var owner = await _db.Owners
                .Include(o => o.Pets)
                .FirstOrDefaultAsync(o => o.UserId.Equals(int.Parse(currentUser)));

            var savedReservation = new Reservation()
            {
                OwnerId = owner.Id,
                SitterId = sitterId,
                PetId = owner.Pets.First().Id,
                SessionId = sessionId,
                StartDate = startDate,
                EndDate = endDate,
                JobType = jobtype
            };

            var viewModel = new SuccessViewModel()
            {

                StartDate = startDate,
                EndDate = endDate,
                SitterId = sitterId

            };

            HttpContext.Session.Remove("StartDate");
            HttpContext.Session.Remove("EndDate");
            HttpContext.Session.Remove("SitterId");
            HttpContext.Session.Remove("SessionId");
            HttpContext.Session.Remove("JobType");

            _db.Reservations.Add(savedReservation);
            await _db.SaveChangesAsync();

            return View(viewModel);
        }

        public IActionResult Unsuccessful()
        {
            return View();
        }

        public IActionResult Checkout(int quantity, int rate, string jobType, DateTime startDate, DateTime endDate, Int32 sitterId)
        {

            var loggedOwner = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggedOwner == null)
            {
                TempData["error"] = "Please login as an Owner";
                return RedirectToAction("Login", "User");
                
            }

            var domain = "https://localhost:7112/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Checkout/Confirmation",
                CancelUrl = domain + $"Checkout/Unsuccessful",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = "email@email.com"
            };

            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = rate,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = jobType
                    }
                },
                Quantity = quantity,

            };

            


            




            options.LineItems.Add(sessionListItem);

            var service = new SessionService();
            Session session = service.Create(options);

            HttpContext.Session.SetString("StartDate", startDate.ToString("yyyy-MM-ddTHH:mm:ss"));
            HttpContext.Session.SetString("EndDate", endDate.ToString("yyyy-MM-ddTHH:mm:ss"));
            HttpContext.Session.SetInt32("SitterId", sitterId);
            HttpContext.Session.SetString("SessionId", session.Id);
            HttpContext.Session.SetString("JobType", jobType);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);


            
        }
    }
}
