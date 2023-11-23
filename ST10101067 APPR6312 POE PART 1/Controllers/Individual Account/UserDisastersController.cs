using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2;
using ST10101067_APPR6312_POE_PART_2.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ST10101067_APPR6312_POE_PART_2.Controllers
{
    public class UserDisastersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserDisastersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Disasters/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Disatser == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disatser
                .FirstOrDefaultAsync(m => m.DISTATER_ID == id);
            if (disaster == null)
            {
                return NotFound();
            }

            return View(disaster);
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Get the current logged-in user
            var currentUser = await _userManager.GetUserAsync(User);
            // Check if the user is authenticated
            if (currentUser == null)
            {
                // User is not logged in, handle this case as needed (e.g., redirect to login)
                return RedirectToAction("Login", "Account");
            }

            // Get the current logged-in username
            string currentUsername = currentUser.UserName;

            // Query the data for the current username
            var userDisasters = await _context.Disatser
                .Where(d => d.USERNAME == currentUsername)
                .ToListAsync();

            return View(userDisasters);
        }

        // GET: UserDisasters/Create
        [Authorize]
        public IActionResult Create()
        {
            var currentDate = DateTime.Now.Date;
            var tomorrow = currentDate.AddDays(1);

            var disaster = new Disaster
            {
                STARTDATE = currentDate,
                ENDDATE = tomorrow
            };

            return View(disaster);
        }
    [HttpPost]
[ValidateAntiForgeryToken]
[Authorize] // Requires any logged-in user
public async Task<IActionResult> Create([Bind("DISTATER_ID,USERNAME,STARTDATE,ENDDATE,LOCATION,AID_TYPE,IsActive")] Disaster disaster)
{
    if (ModelState.IsValid)
    {
        // Get the current logged-in user
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            // Redirect to login if user not found or not authenticated
            return RedirectToAction("Login", "Account");
        }

        // Check if the start date is before the current date
        if (disaster.STARTDATE < DateTime.Now.Date)
        {
            ModelState.AddModelError("STARTDATE", "Start date cannot be earlier than today.");
            return View(disaster);
        }

        // Check if the current date is in between the start and end dates
        if (disaster.STARTDATE <= DateTime.Now.Date && DateTime.Now.Date <= disaster.ENDDATE)
        {
            disaster.IsActive = 1; // Set IsActive to true (1)
        }
        else
        {
            disaster.IsActive = 0; // Set IsActive to false (0)
        }

        if (disaster.ENDDATE < disaster.STARTDATE.Value.AddDays(1))
        {
            ModelState.AddModelError("ENDDATE", "End date must be at least one day after the start date.");
            return View(disaster);
        }

        // Set the disaster's username to the current user's username
        disaster.USERNAME = currentUser.UserName;

        // Add the disaster to the context and save changes to the database
        _context.Add(disaster);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    return View(disaster);
}
        // GET: UserDisasters/Edit/5
        [Authorize(Roles = "Admin")] //only admin
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Disatser == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disatser.FindAsync(id);

            if (disaster == null || disaster.USERNAME != @User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }

            return View(disaster);
        }

        // POST: UserDisasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DISTATER_ID,USERNAME,STARTDATE,ENDDATE,LOCATION,AID_TYPE")] Disaster disaster)
        {
            if (id != disaster.DISTATER_ID)
            {
                return NotFound();
            }

            var existingDisaster = await _context.Disatser.FindAsync(id);

            if (existingDisaster == null || existingDisaster.USERNAME != @User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisasterExists(disaster.DISTATER_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(disaster);
        }

        // GET: UserDisasters/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Disatser == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disatser
                .FirstOrDefaultAsync(m => m.DISTATER_ID == id);

            if (disaster == null || disaster.USERNAME != User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }

            return View(disaster);
        }

        // POST: UserDisasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize (Roles ="Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Disatser == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Disatser'  is null.");
            }

            var disaster = await _context.Disatser.FindAsync(id);

            if (disaster == null || disaster.USERNAME != User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }

            _context.Disatser.Remove(disaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool DisasterExists(int id)
        {
            return _context.Disatser.Any(e => e.DISTATER_ID == id && e.USERNAME == @User.Identity.Name);
        }
    }
}
