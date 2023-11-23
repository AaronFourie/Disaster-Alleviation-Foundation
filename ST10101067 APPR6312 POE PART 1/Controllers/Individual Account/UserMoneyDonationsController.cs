using System;
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
    public class UserMoneyDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserMoneyDonationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string currentUsername = User.Identity.Name;
            var userMoneyDonations = await _context.MoneyDonation
                .Where(d => d.USERNAME == currentUsername)
                .ToListAsync();

            return View(userMoneyDonations);
        }

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

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("MONEY_DONATION_ID,USERNAME,DATE,AMOUNT,DONOR")] MoneyDonation moneyDonation)
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

                // Set the username of the donation to the current user's username
                moneyDonation.USERNAME = currentUser.UserName;

                if (moneyDonation.DATE < DateTime.Now.Date)
                {
                    ModelState.AddModelError("DATE", "Date cannot be earlier than today.");
                    return View(moneyDonation);
                }
                if(moneyDonation.AMOUNT == null || moneyDonation.AMOUNT == 0)
                {
                    ModelState.AddModelError("AMOUNT", "Amount must be more than 0.");
                    return View(moneyDonation);
                }

                // Check if the user selected "Anonymous" as the donor
                if (moneyDonation.DONOR == "Anonymous")
                {
                    moneyDonation.DONOR = "Anonymous";
                }
                else
                {
                    // Set DONOR to the current logged-in user's username
                    moneyDonation.DONOR = currentUser.UserName;
                }

                // Retrieve the existing Money entity or create a new one
                var money = _context.Money.FirstOrDefault();

                if (money == null)
                {
                    money = new Money
                    {
                        TotalMoney = moneyDonation.AMOUNT,
                        RemainingMoney = moneyDonation.AMOUNT
                    };
                    _context.Add(money);
                }
                else
                {
                    // Update the existing Money entity
                    money.TotalMoney += moneyDonation.AMOUNT;
                    money.RemainingMoney += moneyDonation.AMOUNT;
                }

                // Add the money donation to the context
                _context.Add(moneyDonation);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(moneyDonation);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MoneyDonation == null)
            {
                return NotFound();
            }

            var moneyDonation = await _context.MoneyDonation.FindAsync(id);

            if (moneyDonation == null || moneyDonation.USERNAME != User.Identity.Name)
            {
                return NotFound();
            }

            return View(moneyDonation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("MONEY_DONATION_ID, USERNAME, DATE, AMOUNT, DONOR")] MoneyDonation moneyDonation)
        {
            if (id != moneyDonation.MONEY_DONATION_ID)
            {
                return NotFound();
            }

            var existingMoneyDonation = await _context.MoneyDonation.FindAsync(id);

            if (existingMoneyDonation == null || existingMoneyDonation.USERNAME != User.Identity.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Calculate the difference in donation amount for updating TotalMoney and RemainingMoney
                decimal donationDifference = moneyDonation.AMOUNT - existingMoneyDonation.AMOUNT;

                // Update TotalMoney and RemainingMoney accordingly
                var money = _context.Money.First(); // Assuming you have a single record for Money

                money.TotalMoney += donationDifference;
                money.RemainingMoney += donationDifference;

                _context.Update(money);
                _context.Update(moneyDonation);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(moneyDonation);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MoneyDonation == null)
            {
                return NotFound();
            }

            var moneyDonation = await _context.MoneyDonation
                .FirstOrDefaultAsync(m => m.MONEY_DONATION_ID == id);

            if (moneyDonation == null || moneyDonation.USERNAME != User.Identity.Name)
            {
                return NotFound();
            }

            return View(moneyDonation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MoneyDonation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MoneyDonation'  is null.");
            }

            var moneyDonation = await _context.MoneyDonation.FindAsync(id);

            if (moneyDonation != null || moneyDonation.USERNAME != User.Identity.Name)
            {
                // Calculate the difference in donation amount for updating TotalMoney and RemainingMoney
                decimal donationDifference = -moneyDonation.AMOUNT;

                // Update TotalMoney and RemainingMoney accordingly
                var money = _context.Money.First(); // Assuming you have a single record for Money

                money.TotalMoney += donationDifference;
                money.RemainingMoney += donationDifference;

                _context.Update(money);
                _context.Remove(moneyDonation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MoneyDonationExists(int id)
        {
            return _context.MoneyDonation.Any(e => e.MONEY_DONATION_ID == id && e.USERNAME == User.Identity.Name);
        }
    }
}