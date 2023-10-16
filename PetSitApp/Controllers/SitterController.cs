﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSitApp.Data;
using PetSitApp.Models;
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

        // GET ///////////////////////////////////////////////

        [Authorize(Roles = "Sitter")]
        public async Task<IActionResult> SitterDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ownerInfo = await _db.Sitters.FirstOrDefaultAsync(s => s.UserId.Equals(int.Parse(userId)));

            


            return View(ownerInfo);
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
            return View();
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
    }
}