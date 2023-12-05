using Microsoft.AspNetCore.Mvc;
using PetSitApp.ViewModels;
using Stripe.Checkout;
using System.Security.Claims;

namespace PetSitApp.Controllers
{
    public class CheckoutController : Controller
    {

        public IActionResult Confirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus == "paid")
            {
                return View("Success");
            }
            return View("Failure");
        }

        /////////////////////////// GET
        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Unsuccessful()
        {
            return View();
        }

        public IActionResult Checkout(int quantity, int rate, string jobType)
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

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);


            
        }
    }
}
