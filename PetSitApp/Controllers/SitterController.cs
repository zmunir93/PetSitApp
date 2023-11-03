using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSitApp.DTOs.GoogleGeocoding;
using PetSitApp.Models;
using PetSitApp.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace PetSitApp.Controllers
{
    public class SitterController : Controller
    {
        private readonly PetSitAppContext _db;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        public SitterController(PetSitAppContext db, IConfiguration configuration, HttpClient client) 
        {
            _db = db;
            _configuration = configuration;
            _client = client;
        }
        protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        

        // GET ///////////////////////////////////////////////

        [Authorize(Roles = "Sitter")]
        public async Task<IActionResult> SitterDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ownerInfo = await _db.Sitters.FirstOrDefaultAsync(s => s.UserId.Equals(int.Parse(userId)));

            


            return View(ownerInfo);
        }
        public async Task<Sitter> GetCurrentSitter()
        {
            var userId = CurrentUserId;
            return await _db.Sitters.FirstOrDefaultAsync(s => s.UserId.Equals(int.Parse(userId)));
        }

        public IActionResult CreateSitter()
        {
            return View();
        }

        public async Task<IActionResult> EditSitter(int id)
        {
            var sitter = await _db.Sitters.FindAsync(id);

            if (sitter == null)
            {
                return NotFound();
            }
            return View(sitter);
        }

        public IActionResult EditAvailability()
        {
            var viewModel = new AvailabilityViewModel();
            
            return View(viewModel);
        }

        public IActionResult Services()
        {
            var viewModel = new ServicesViewModel();
            return View(viewModel);
        }



        // POST //////////////////////////////////////////////

        public async Task<IActionResult> GetOwnerImage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentUser = await _db.Sitters.FirstOrDefaultAsync(s => s.UserId.Equals(int.Parse(userId)));

            var memStream = new MemoryStream(currentUser.ProfilePicture);
            memStream.Position = 0;

            return File(memStream, "image/jpeg");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSitter(int id, Sitter model, IFormFile ProfilePicture)
        {


            if (id != model.Id)
            {
                return NotFound();
            }
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                var existingSitter = await _db.Sitters.FindAsync(id);

                if (existingSitter == null)
                {
                    return NotFound();
                }

                var apiKey = _configuration["GoogleGeocodingAPI:ApiKey"];
                var address = $"{model.Address}, {model.City}, {model.State}, {model.Zip}";
                var apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={apiKey}";

                var response = await _client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(jsonResult);
                    var dynamicResult = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResult);
                    string status = dynamicResult.status; // This should give "OK"
                    var results = dynamicResult.results; // This gives the "results" array.

                    var firstResult = results[0];
                    string formattedAddress = firstResult.formatted_address;
                    double lat = firstResult.geometry.location.lat;
                    double lng = firstResult.geometry.location.lng;



                    //var geoResult = JsonSerializer.Deserialize<GeocodeResponse>(jsonResult);
                    //var location = geoResult.Results.ToArray()[0].Geometry.Location;
                    existingSitter.Longitude = lng;
                    existingSitter.Latitude = lat;
                }

                existingSitter.FirstName = model.FirstName;
                existingSitter.LastName = model.LastName;
                existingSitter.Age = model.Age;
                existingSitter.Address = model.Address;
                existingSitter.City = model.City;
                existingSitter.State = model.State;
                existingSitter.Zip = model.Zip;

                if (ProfilePicture != null && ProfilePicture.Length > 1)
                {
                    using (var memStream = new MemoryStream())
                    {
                        await ProfilePicture.CopyToAsync(memStream);
                        existingSitter.ProfilePicture = memStream.ToArray();
                    }
                }

                _db.Sitters.Update(existingSitter);
                await _db.SaveChangesAsync();
                TempData["success"] = "Profile Successfully updated.";
                return RedirectToAction("SitterDashboard");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(2621440)] // Limit to 2.5MB
        public async Task<IActionResult> CreateSitter(Sitter model, IFormFile ProfilePicture)
        {
            ModelState.Remove("User");

            //foreach (var modelStateKey in ModelState.Keys)
            //{
            //    var modelStateVal = ModelState[modelStateKey];
            //    foreach (var error in modelStateVal.Errors)
            //    {
            //        // Log or print the error to get an understanding of what's causing the validation failure
            //        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
            //    }
            //}

            if (ModelState.IsValid)
            {
                //User of login session id # but as a string. needs to be converted to int later
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ProfilePicture.CopyToAsync(memoryStream);
                        model.ProfilePicture = memoryStream.ToArray();
                    }
                }

                model.UserId = int.Parse(userId);
                _db.Sitters.Add(model);
                await _db.SaveChangesAsync();

                TempData["success"] = "Account successfully created";
                return RedirectToAction("SitterDashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAvailability(AvailabilityViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sitter = await _db.Sitters.FirstOrDefaultAsync(s => s.UserId.Equals(int.Parse(userId)));

            var weekAvail = await _db.WeekAvailabilities.FirstOrDefaultAsync(wa => wa.SitterId.Equals(sitter.Id));

            /// if table doesnt exist, creates new one
            if (weekAvail == null)
            {
                weekAvail = new WeekAvailability
                {
                    SitterId = sitter.Id,
                    Monday = viewModel.Monday,
                    Tuesday = viewModel.Tuesday,
                    Wednesday = viewModel.Wednesday,
                    Thursday = viewModel.Thursday,
                    Friday = viewModel.Friday,
                    Saturday = viewModel.Saturday,
                    Sunday = viewModel.Sunday
                };

                _db.WeekAvailabilities.Add(weekAvail);

            } else // if table does exist, updates instead
            {
                weekAvail.Monday = viewModel.Monday;
                weekAvail.Tuesday = viewModel.Tuesday;
                weekAvail.Wednesday = viewModel.Wednesday;
                weekAvail.Thursday = viewModel.Thursday;
                weekAvail.Friday = viewModel.Friday;
                weekAvail.Saturday = viewModel.Saturday;
                weekAvail.Sunday = viewModel.Sunday;
            }

            if (viewModel.SelectedDate.HasValue)
            {
                var daysUnavail = new DaysUnavailable
                {
                    SitterId = sitter.Id,
                    IsAvailable = false,
                    Date = viewModel.SelectedDate.Value
                };

                _db.DaysUnavailables.Add(daysUnavail);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("SitterDashboard");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Services(ServicesViewModel viewModel)
        {
            var sitter = await GetCurrentSitter();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (sitter == null)
            {
                return RedirectToAction("Login", "User");
            }
            int serviceNumber = 0;
            if (viewModel.IsDog) serviceNumber += 1;
            if (viewModel.IsCat) serviceNumber += 2;

            var service = new Service
            {
                PetType = serviceNumber,
                SitterId = sitter.Id
            };

            _db.Services.Add(service);
            await _db.SaveChangesAsync();

            if (viewModel.OffersBoarding)
            {
                _db.ServiceTypes.Add(new ServiceType
                {
                    ServiceId = service.Id,
                    ServiceOffered = "Boarding",
                    Rate = viewModel.BoardingRate

                });
            }

            if (viewModel.OffersHomeSits)
            {
                _db.ServiceTypes.Add(new ServiceType
                {
                    ServiceId = service.Id,
                    ServiceOffered = "Home Sits",
                    Rate = viewModel.HomeSitRate
                });
            }

            if (viewModel.OffersWalking)
            {
                _db.ServiceTypes.Add(new ServiceType
                {
                    ServiceId = service.Id,
                    ServiceOffered = "Walking",
                    Rate = viewModel.WalkingRate
                });
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("SitterDashboard");
        }
    }
}
