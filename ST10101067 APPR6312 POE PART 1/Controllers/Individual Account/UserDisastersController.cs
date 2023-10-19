using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
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

        public UserDisastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserDisasters
        public async Task<IActionResult> Index()
        {
            if (!@User.Identity.IsAuthenticated)
            {
                // User is not logged in, handle this case as needed (e.g., redirect to login)
                return RedirectToAction("Login", "Account");
            }

            // Get the current logged-in username
            string currentUsername = @User.Identity.Name;

            // Query the data for the current username
            var userGoodsDonations = await _context.Disatser
                .Where(d => d.USERNAME == currentUsername)
                .ToListAsync();

            return View(userGoodsDonations);
        }

        // GET: UserDisasters/Details/5
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

        // GET: UserDisasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserDisasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DISTATER_ID,USERNAME,STARTDATE,ENDDATE,LOCATION,AID_TYPE")] Disaster disaster)
        {
            if (ModelState.IsValid)
            {
                // Get the current logged-in username
                string currentUsername = @User.Identity.Name;

                // Set the username of the disaster to the current user's username
                disaster.USERNAME = currentUsername;

                _context.Add(disaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disaster);
        }

        // GET: UserDisasters/Edit/5
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
