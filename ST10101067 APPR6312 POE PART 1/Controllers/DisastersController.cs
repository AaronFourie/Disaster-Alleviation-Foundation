using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2;
using ST10101067_APPR6312_POE_PART_2.Models;

namespace ST10101067_APPR6312_POE_PART_2.Controllers
{
    public class DisastersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public DisastersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Disasters
        public async Task<IActionResult> Index()
        {
              return _context.Disatser != null ? 
                          View(await _context.Disatser.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Disatser'  is null.");
        }

        // GET: Disasters/Details/5
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
        [Authorize]
        public async Task<IActionResult> Create([Bind("DISTATER_ID,USERNAME,STARTDATE,ENDDATE,LOCATION,AID_TYPE,IsActive")] Disaster disaster)
        {
            if (ModelState.IsValid)
            {
                // Get the current logged-in username
                string currentUsername = User.Identity.Name;

                // Set the username of the disaster to the current user's username
                disaster.USERNAME = currentUsername;

                // Check if the start date is before the current date
                if (disaster.STARTDATE < DateTime.Now.Date)
                {
                    ModelState.AddModelError("STARTDATE", "Start date cannot be earlier than today.");
                    return View(disaster);
                }

                // Check if the current date is in between the start and end dates
                if (disaster.STARTDATE <= DateTime.Now.Date && DateTime.Now.Date <= disaster.ENDDATE)
                {
                    disaster.IsActive = 1; // 1 represents true in your integer-based flag
                }
                else
                {
                    disaster.IsActive = 0; // 0 represents false in your integer-based flag
                }

                if (disaster.ENDDATE < disaster.STARTDATE.Value.Date.AddDays(1))
                {
                    ModelState.AddModelError("ENDDATE", "End date must be at least one day after the start date.");
                    return View(disaster);
                }

                // Now add the disaster with MoneyAllocation to the context.
                _context.Add(disaster);

                // Save changes to the database.
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(disaster);
        }

        // GET: UserDisasters/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Disatser == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disatser.FindAsync(id);

            if (disaster == null)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }

            return View(disaster);
        }

        // POST: UserDisasters/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DISTATER_ID,USERNAME,STARTDATE,ENDDATE,LOCATION,AID_TYPE")] Disaster disaster)
        {
            if (id != disaster.DISTATER_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the start date is before the current date
                    if (disaster.STARTDATE < DateTime.Now.Date)
                    {
                        ModelState.AddModelError("STARTDATE", "Start date cannot be earlier than today.");
                        return View(disaster);
                    }

                    // Check if the current date is in between the start and end dates
                    if (disaster.STARTDATE <= DateTime.Now.Date && DateTime.Now.Date <= disaster.ENDDATE)
                    {
                        disaster.IsActive = 1; // 1 represents true in your integer-based flag
                    }
                    else
                    {
                        disaster.IsActive = 0; // 0 represents false in your integer-based flag
                    }

                    if (disaster.ENDDATE < disaster.STARTDATE.Value.Date.AddDays(1))
                    {
                        ModelState.AddModelError("ENDDATE", "End date must be at least one day after the start date.");
                        return View(disaster);
                    }
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

            var disaster = await _context.Disatser.FindAsync(id);

            if (disaster == null)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }

            return View(disaster);
        }

        // POST: UserDisasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Disatser == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Disatser'  is null.");
            }

            var disaster = await _context.Disatser.FindAsync(id);

            if (disaster == null)
            {
                // The disaster doesn't exist
                return NotFound();
            }

            _context.Disatser.Remove(disaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool DisasterExists(int id)
        {
            return _context.Disatser.Any(e => e.DISTATER_ID == id);
        }
    }
}
