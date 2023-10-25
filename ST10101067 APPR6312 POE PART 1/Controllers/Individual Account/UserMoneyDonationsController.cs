using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2;
using Microsoft.AspNetCore.Authorization;
using ST10101067_APPR6312_POE_PART_2.Models;

namespace ST10101067_APPR6312_POE_PART_2.Controllers
{
    
    public class UserMoneyDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserMoneyDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserMoneyDonations
        [Authorize]
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
            var userGoodsDonations = await _context.MoneyDonation
                .Where(d => d.USERNAME == currentUsername)
                .ToListAsync();

            return View(userGoodsDonations);
        }

        // GET: UserMoneyDonations/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MoneyDonation == null)
            {
                return NotFound();
            }

            var moneyDonation = await _context.MoneyDonation
                .FirstOrDefaultAsync(m => m.MONEY_DONATION_ID == id);
            if (moneyDonation == null)
            {
                return NotFound();
            }

            return View(moneyDonation);
        }

        // GET: UserMoneyDonations/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserMoneyDonations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("MONEY_DONATION_ID,USERNAME,DATE,AMOUNT,DONOR")] MoneyDonation moneyDonation)
        {
            if (ModelState.IsValid)
            {
                // Get the current logged-in username
                string currentUsername = @User.Identity.Name;

                // Set the username of the disaster to the current user's username
                moneyDonation.USERNAME = currentUsername;
                // Check if the user selected "Anonymous" as the donor
                if (moneyDonation.DONOR == "Anonymous")
                {
                    moneyDonation.DONOR = "Anonymous";
                }
                else
                {
                    // Set DONOR to the current logged-in user's username
                    var currentUser = User.Identity?.Name;
                    moneyDonation.DONOR = currentUser;
                }
                _context.Add(moneyDonation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(moneyDonation);
        }

        // GET: UserMoneyDonations/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MoneyDonation == null)
            {
                return NotFound();
            }

            var moneyDonation = await _context.MoneyDonation.FindAsync(id);
            if (moneyDonation == null || moneyDonation.USERNAME != @User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }
            return View(moneyDonation);
        }

        // POST: UserMoneyDonations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("MONEY_DONATION_ID,USERNAME,DATE,AMOUNT,DONOR")] MoneyDonation moneyDonation)
        {
            if (id != moneyDonation.MONEY_DONATION_ID)
            {
                return NotFound();
            }
            var existingDisaster = await _context.MoneyDonation.FindAsync(id);

            if (existingDisaster == null || existingDisaster.USERNAME != @User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moneyDonation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoneyDonationExists(moneyDonation.MONEY_DONATION_ID))
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
            return View(moneyDonation);
        }

        // GET: UserMoneyDonations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MoneyDonation == null)
            {
                return NotFound();
            }

            var moneyDonation = await _context.MoneyDonation
                .FirstOrDefaultAsync(m => m.MONEY_DONATION_ID == id);
            if (moneyDonation == null || moneyDonation.USERNAME != @User.Identity.Name)
            {
                return NotFound();
            }

            return View(moneyDonation);
        }

        // POST: UserMoneyDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MoneyDonation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MoneyDonation'  is null.");
            }
            var moneyDonation = await _context.MoneyDonation.FindAsync(id);
            if (moneyDonation != null || moneyDonation.USERNAME != @User.Identity.Name)
            {
                _context.MoneyDonation.Remove(moneyDonation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MoneyDonationExists(int id)
        {
            return _context.MoneyDonation.Any(e => e.MONEY_DONATION_ID == id && e.USERNAME == @User.Identity.Name);
        }
    }
}
