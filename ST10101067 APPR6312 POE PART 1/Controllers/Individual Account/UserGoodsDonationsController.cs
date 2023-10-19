using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_1;
using ST10101067_APPR6312_POE_PART_1.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ST10101067_APPR6312_POE_PART_1.Controllers
{
    public class UserGoodsDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserGoodsDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserGoodsDonations
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
            var userGoodsDonations = await _context.GoodsDonation
                .Where(d => d.USERNAME == currentUsername)
                .ToListAsync();

            return View(userGoodsDonations);
        }

        // GET: UserGoodsDonations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GoodsDonation == null)
            {
                return NotFound();
            }

            var goodsDonation = await _context.GoodsDonation
                .FirstOrDefaultAsync(m => m.GOODS_DONATION_ID == id);
            if (goodsDonation == null)
            {
                return NotFound();
            }

            return View(goodsDonation);
        }

        // GET: UserGoodsDonations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserGoodsDonations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GOODS_DONATION_ID, USERNAME, DATE,ITEM_COUNT,CATEGORY,DESCRIPTION,DONOR")] GoodsDonation goodsDonation)
        {
            if (ModelState.IsValid)
            {

                // Check if the user selected "Anonymous" as the donor
                if (goodsDonation.DONOR == "Anonymous")
                {
                    goodsDonation.DONOR = "Anonymous";
                }
                else
                {
                    // Set DONOR to the current logged-in user's username
                    var currentUser = User.Identity?.Name;
                    goodsDonation.DONOR = currentUser;
                }
                _context.Add(goodsDonation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(goodsDonation);
        }

        // GET: UserGoodsDonations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GoodsDonation == null)
            {
                return NotFound();
            }

            var goodsDonation = await _context.GoodsDonation.FindAsync(id);
            if (goodsDonation == null || goodsDonation.USERNAME != @User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }
            return View(goodsDonation);
        }

        // POST: UserGoodsDonations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GOODS_DONATION_ID,USERNAME,DATE,ITEM_COUNT,CATEGORY,DESCRIPTION,DONOR")] GoodsDonation goodsDonation)
        {
            if (id != goodsDonation.GOODS_DONATION_ID)
            {
                return NotFound();
            }
            var existingDisaster = await _context.GoodsDonation.FindAsync(id);

            if (existingDisaster == null || existingDisaster.USERNAME != @User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goodsDonation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsDonationExists(goodsDonation.GOODS_DONATION_ID))
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
            return View(goodsDonation);
        }

        // GET: UserGoodsDonations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GoodsDonation == null)
            {
                return NotFound();
            }

            var goodsDonation = await _context.GoodsDonation
                .FirstOrDefaultAsync(m => m.GOODS_DONATION_ID == id);
            if (goodsDonation == null || goodsDonation.USERNAME != @User.Identity.Name)
            {
                return NotFound();
            }

            return View(goodsDonation);
        }

        // POST: UserGoodsDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GoodsDonation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GoodsDonation'  is null.");
            }
            var goodsDonation = await _context.GoodsDonation.FindAsync(id);
            if (goodsDonation != null || goodsDonation.USERNAME != @User.Identity.Name)
            {
                _context.GoodsDonation.Remove(goodsDonation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsDonationExists(int id)
        {
            return _context.GoodsDonation.Any(e => e.GOODS_DONATION_ID == id && e.USERNAME == @User.Identity.Name);
        }
    }
}
