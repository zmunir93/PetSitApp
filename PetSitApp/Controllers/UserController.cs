using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetSitApp.Data;
using PetSitApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetSitApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly PetSitApp.Models.PetSitAppContext _db;
        public UserController(PetSitApp.Models.PetSitAppContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
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


    }
}
