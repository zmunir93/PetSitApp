using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetSitApp.Data;
using PetSitApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PetSitApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly PetSitAppContext _db;
        private readonly HttpClient _client;
        public UserController(PetSitAppContext db, IConfiguration configuration, HttpClient client)
        {
            _db = db;
            _configuration = configuration;
            _client = client;
        }

        public static double CalculateDistance(double lat1, double long1, double lat2, double long2)
        {
            const double EarthRadiusMiles = 3958.8; // Earth's radius in miles

            double lat1Rad = ToRadians(lat1);
            double long1Rad = ToRadians(long1);
            double lat2Rad = ToRadians(lat2);
            double long2Rad = ToRadians(long2);

            double dLat = lat2Rad - lat1Rad;
            double dLon = long2Rad - long1Rad;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusMiles * c;
        }

        public static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }



        // GET //////////////////////////////////////
        [Authorize(Roles="Owner")]
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RegisterSitter()
        {
            return View();
        }
        public IActionResult SitterSearch()
        {
            return View();
        }



        // POST //////////////////////////////////////
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool rememberMe)
        {
            // Check to make sure username and password are not null
           if (username == null || password == null)
            {
                TempData["error"] = "Username or password cannot be empty";
                return View();
            }

            // Fetch User from database
            var userFromDb = await _db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

            if (userFromDb == null || !BCrypt.Net.BCrypt.Verify(password, userFromDb.Password))
            {
                TempData["error"] = "Invalid username or password";
                return View();
            }
            //// Generate JWT token
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userFromDb.Username) }),
            //    Expires = DateTime.UtcNow.AddDays(1),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //var tokenString = tokenHandler.WriteToken(token);

            //Response.Cookies.Append("jwt", tokenString, new CookieOptions { HttpOnly = true });

            //return RedirectToAction("Index");

            //// Cookie method
            //var options = new CookieOptions
            //{
            //    Expires = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(1),
            //    HttpOnly = true,
            //    Secure = true // Only set this to true if using HTTPS
            //};
            //string authenticationToken = Guid.NewGuid().ToString();
            //Response.Cookies.Append("PetSitAuthToken", authenticationToken, options);
            //return RedirectToAction("Index");


            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userFromDb.Username),
            new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
            new Claim(ClaimTypes.Role, "Owner")
        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.
            };

            //HttpContext.Session.SetString("UserId", userFromDb.Id.ToString());
            //return RedirectToAction("Index");

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
            return RedirectToAction("Dashboard", "Owner");
        }

        public async Task<IActionResult> LoginSitter(string username, string password, bool rememberMe)
        {
            // Check to make sure username and password are not null
            if (username == null || password == null)
            {
                TempData["error"] = "Username or password cannot be empty";
                return View();
            }

            // Fetch User from database
            var userFromDb = await _db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

            if (userFromDb == null || !BCrypt.Net.BCrypt.Verify(password, userFromDb.Password))
            {
                TempData["error"] = "Invalid username or password";
                return View();
            }
            //// Generate JWT token
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userFromDb.Username) }),
            //    Expires = DateTime.UtcNow.AddDays(1),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //var tokenString = tokenHandler.WriteToken(token);

            //Response.Cookies.Append("jwt", tokenString, new CookieOptions { HttpOnly = true });

            //return RedirectToAction("Index");

            //// Cookie method
            //var options = new CookieOptions
            //{
            //    Expires = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(1),
            //    HttpOnly = true,
            //    Secure = true // Only set this to true if using HTTPS
            //};
            //string authenticationToken = Guid.NewGuid().ToString();
            //Response.Cookies.Append("PetSitAuthToken", authenticationToken, options);
            //return RedirectToAction("Index");


            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userFromDb.Username),
            new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
            new Claim(ClaimTypes.Role, "Sitter")
        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.
            };

            //HttpContext.Session.SetString("UserId", userFromDb.Id.ToString());
            //return RedirectToAction("Index");

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
            return RedirectToAction("SitterDashboard", "Sitter");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSitter(User model)
        {
            if (!ModelState.IsValid)
            {
                return View();
                
            }

            var existingUser = await _db.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Username.ToLower().Equals(model.Username.ToLower()));



            if (existingUser != null && existingUser.Permissions.Any(p => p.Role == "Sitter"))
            {
                TempData["error"] = "Account already exist. Please login.";
                return RedirectToAction("Login"); 
            }
            else if (existingUser != null && existingUser.Permissions.Any(p => p.Role == "Owner") && BCrypt.Net.BCrypt.Verify(model.Password, existingUser.Password)) // Already have an account as an Sitter
            {
                var newPermission = new Permission
                {
                    Role = "Sitter",
                    User = existingUser,
                    UserId = existingUser.Id
                };

                // Permission is added, then saved
                _db.Permissions.Add(newPermission);
                await _db.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.Username),
                    new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                    new Claim(ClaimTypes.Role, newPermission.Role)
                };

                var claimsIndentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIndentity), authProperties);

                TempData["success"] = "Successful creation of account";
                return RedirectToAction("SitterDashboard", "Sitter");
            } 
            else if (existingUser == null)
            {
                // model.Password is encrypted
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // newUserObj created to pass info to be added to database
                var newUserObj = new User
                {
                    Username = model.Username,
                    Password = passwordHash
                };

                // Info is added and then saved. Redirected to index page
                _db.Users.Add(newUserObj);
                await _db.SaveChangesAsync();

                // Permission is created by setting Role explicitly to User that is being created
                var newPermission = new Permission
                {
                    Role = "Sitter",
                    User = newUserObj,
                    UserId = newUserObj.Id
                };

                // Permission is added, then saved
                _db.Permissions.Add(newPermission);
                await _db.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, newUserObj.Username),
                    new Claim(ClaimTypes.NameIdentifier, newUserObj.Id.ToString()),
                    new Claim(ClaimTypes.Role, newPermission.Role)
                };

                var claimsIndentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIndentity), authProperties);

                TempData["success"] = "Successful creation of account";
                return RedirectToAction("SitterDashboard", "Sitter");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            ModelState.Remove("Owner");
            if (ModelState.IsValid)
            {
                // checks to see if user exist in database
                var existingUser = _db.Users.FirstOrDefault(u => u.Username.ToLower().Equals(model.Username.ToLower()));

                // if they do exist, error is thrown
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username already exist. Please try a differnt one.");
                    TempData["error"] = "Username already exist. Please try a differnt name";
                    return View(model);
                }

                // model.Password is encrypted
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // newUserObj created to pass info to be added to database
                var newUserObj = new User
                {
                    Username = model.Username,
                    Password = passwordHash
                };

                // Info is added and then saved. Redirected to index page
                _db.Users.Add(newUserObj);
                await _db.SaveChangesAsync();

                // Permission is created by setting Role explicitly to User that is being created
                var newPermission = new Permission
                {
                    Role = "Owner",
                    User = newUserObj,
                    UserId = newUserObj.Id
                };

                // Permission is added, then saved
                _db.Permissions.Add(newPermission);
                await _db.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, newUserObj.Username),
                    new Claim(ClaimTypes.NameIdentifier, newUserObj.Id.ToString()),
                    new Claim(ClaimTypes.Role, newPermission.Role)
                };

                var claimsIndentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIndentity), authProperties);

                TempData["success"] = "Successful creation of account";
                return RedirectToAction("Dashboard", "Owner");
            }

            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SitterSearch(string dogOrCat, string serviceType, string zipCode, DateTime? startDate, DateTime? endDate)
        {
            var query= _db.Sitters.AsQueryable();

            if (dogOrCat == "IsDog") query = query.Where(s => s.Service.PetType == 1 || s.Service.PetType == 3);
            else if (dogOrCat == "IsCat") query = query.Where(s => s.Service.PetType == 2 || s.Service.PetType == 3);

            if (!string.IsNullOrWhiteSpace(serviceType))
            {
                query = query.Where(s => s.Service.ServiceTypes.Any(st => st.ServiceOffered.Equals(serviceType)));
            }

            double zipLat = 0.0;
            double zipLng = 0.0;

            var zipCodeApiKey = _configuration["GoogleGeocodingAPI:ApiKey"];
            var zipApiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={zipCode}&key={zipCodeApiKey}";
            var zipResponse = await _client.GetAsync(zipApiUrl);
            if (zipResponse.IsSuccessStatusCode)
            {
                var zipJsonResult = await zipResponse.Content.ReadAsStringAsync();
                var zipDynamicResult = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(zipJsonResult);
                zipLat = zipDynamicResult.results[0].geometry.location.lat;
                zipLng = zipDynamicResult.results[0].geometry.location.lng;
            }


            var sittersInRange = new List<Sitter>();
            foreach (var sitter in query) // or however you are fetching the sitters
            {
                double nonNullableLat = sitter.Latitude.Value;
                double nonNullableLng = sitter.Longitude.Value;

                
                var distance = CalculateDistance(zipLat, zipLng, nonNullableLat, nonNullableLng);
                if (distance <= 25) // less than radius
                {
                    
                    sittersInRange.Add(sitter);
                }
            }

            var sittersWithAvailibility = new List<Sitter>();

            if (startDate.HasValue && endDate.HasValue)
            {
                foreach (var sitter in sittersInRange)
                {
                    if (sitter.DaysUnavailables.Any(du => du.Date >= startDate && du.Date <= endDate && !du.IsAvailable))
                    {
                        continue;
                    }

                    sittersWithAvailibility.Add(sitter);
                }
            }
            else
            {
                sittersWithAvailibility = sittersInRange;
            }


            var sitterForWeekUnavail = new List<Sitter>();

            foreach (var sitter in query)
            {

                var sitterWeekAvailability = _db.WeekAvailabilities.FirstOrDefault(avail => avail.SitterId == sitter.Id);

                if (sitterWeekAvailability != null)
                {
                    for (DateTime date = startDate.Value; date <= endDate.Value; date = date.AddDays(1))
                    {
                        DayOfWeek dayOfWeek = date.DayOfWeek; // Concert the date to a DayOfWeek enumeration

                        switch (dayOfWeek) // Check if the Sitter is unavailable on this day
                        {
                            case DayOfWeek.Monday:
                                if(sitterWeekAvailability.Monday)
                                {

                                }
                                break;
                                case DayOfWeek.Tuesday:
                                if(sitterWeekAvailability.Tuesday)
                                {

                                }
                                break;
                                case DayOfWeek.Wednesday:
                                if(sitterWeekAvailability.Wednesday)
                                {

                                }
                                break;
                                case DayOfWeek.Thursday:
                                if(sitterWeekAvailability.Thursday)
                                {

                                }
                                break;
                                case DayOfWeek.Friday:
                                if(sitterWeekAvailability.Friday)
                                {

                                }
                                break;
                                case DayOfWeek.Saturday:
                                if (sitterWeekAvailability.Saturday)
                                {

                                }
                                break;
                                case DayOfWeek.Sunday:
                                if (sitterWeekAvailability.Sunday)
                                {

                                }
                                break;
                        }
                    }
                }
                sitterForWeekUnavail.Add(sitter);

            }

            
            
            


            query = sittersWithAvailibility.AsQueryable();


            return View(query);


        }

    }
}
