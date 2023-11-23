using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2;
using ST10101067_APPR6312_POE_PART_2.Data;
using ST10101067_APPR6312_POE_PART_2.Models;

namespace ST10101067_APPR6312_POE_PART_2.Controllers
{
    public class GoodsDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public GoodsDonationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: GoodsDonations
        public async Task<IActionResult> Index()
        {
            return _context.GoodsDonation != null ?
                        View(await _context.GoodsDonation.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.GoodsDonation'  is null.");
        }

        // GET: GoodsDonations/Details/5
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
        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            // Get the current logged-in username
            // Get the current logged-in user
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                // Redirect to login if user not found or not authenticated
                return RedirectToAction("Login", "Account");
            }
            // Get the unique categories for the logged-in user, excluding "Cloths" and "Non-Perishable Foods"
            var existingCategories = _context.GoodsDonation
                .Where(d => d.USERNAME == currentUser.UserName && d.CATEGORY != "Cloths" && d.CATEGORY != "Non-Perishable Foods")
                .Select(d => d.CATEGORY)
                .Distinct()
                .ToList();

            ViewBag.CategoryList = existingCategories;

            return View();
        }

        // POST: UserGoodsDonations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: UserGoodsDonations/Create
        // POST: UserGoodsDonations/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GOODS_DONATION_ID, USERNAME, DATE, ITEM_COUNT, CATEGORY, DESCRIPTION, DONOR")] GoodsDonation goodsDonation)
        {
            if (ModelState.IsValid)
            {
                if (goodsDonation.DATE < DateTime.Now.Date)
                {
                    ModelState.AddModelError("DATE", "Date cannot be earlier than today.");
                    return View(goodsDonation);
                }

                // Check if the user selected "Anonymous" as the donor
                if (goodsDonation.DONOR == "Anonymous")
                {
                    goodsDonation.DONOR = "Anonymous";
                }
                else
                {
                    // Get the current logged-in username
                    // Get the current logged-in user
                    var currentUser = await _userManager.GetUserAsync(User);

                    if (currentUser == null)
                    {
                        // Redirect to login if user not found or not authenticated
                        return RedirectToAction("Login", "Account");
                    }
                    // Set DONOR to the current logged-in user's username
                    goodsDonation.DONOR = currentUser.UserName;
                    
                }

                // Check if the category exists in the GoodsInventory
                var inventoryItem = _context.GoodsInventory.FirstOrDefault(g => g.CATGEORY == goodsDonation.CATEGORY);

                if (inventoryItem != null)
                {
                    // Update the item count in the existing record
                    inventoryItem.ITEM_COUNT += goodsDonation.ITEM_COUNT;
                }
                else
                {
                    // Create a new record in the GoodsInventory
                    _context.GoodsInventory.Add(new GoodsInventory
                    {
                        CATGEORY = goodsDonation.CATEGORY, // Corrected the property name
                        ITEM_COUNT = goodsDonation.ITEM_COUNT
                    });
                }

                // Check if the category already exists in the user's previous donations
                var existingCategories = _context.GoodsDonation
                    .Where(d => d.USERNAME == goodsDonation.USERNAME && d.CATEGORY != "Cloths" && d.CATEGORY != "Non-Perishable Foods")
                    .Select(d => d.CATEGORY)
                    .Distinct()
                    .ToList();

                if (!existingCategories.Contains(goodsDonation.CATEGORY))
                {
                    existingCategories.Add(goodsDonation.CATEGORY);
                }

                ViewBag.CategoryList = existingCategories;

                _context.Add(goodsDonation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If the model is not valid, we need to repopulate the category dropdown with the unique categories.
            var existingUserCategories = _context.GoodsDonation
                .Where(d => d.USERNAME == goodsDonation.USERNAME && d.CATEGORY != "Cloths" && d.CATEGORY != "Non-perishable foods")
                .Select(d => d.CATEGORY)
                .Distinct()
                .ToList();

            ViewBag.CategoryList = new SelectList(existingUserCategories);

            return View(goodsDonation);
        }

        // GET: UserGoodsDonations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GoodsDonation == null)
            {
                return NotFound();
            }

            var goodsDonation = await _context.GoodsDonation
                .FirstOrDefaultAsync(m => m.GOODS_DONATION_ID == id);
            if (goodsDonation == null)
            {
                // Either the goods donation doesn't exist or it doesn't belong to the current user
                return NotFound();
            }
            return View(goodsDonation);
        }

        // POST: UserGoodsDonations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GOODS_DONATION_ID,USERNAME,DATE,ITEM_COUNT,CATEGORY,DESCRIPTION,DONOR")] GoodsDonation updatedGoodsDonation)
        {
            if (id != updatedGoodsDonation.GOODS_DONATION_ID)
            {
                return NotFound();
            }

            var existingGoodsDonation = await _context.GoodsDonation.FindAsync(id);

            if (existingGoodsDonation == null)
            {
                return NotFound();
            }
            int existingcount = existingGoodsDonation.ITEM_COUNT;
            if (ModelState.IsValid)
            {
                try
                {
                    existingGoodsDonation.USERNAME = updatedGoodsDonation.USERNAME;
                    existingGoodsDonation.DATE = updatedGoodsDonation.DATE;
                    existingGoodsDonation.ITEM_COUNT = updatedGoodsDonation.ITEM_COUNT;
                    existingGoodsDonation.CATEGORY = updatedGoodsDonation.CATEGORY;
                    existingGoodsDonation.DESCRIPTION = updatedGoodsDonation.DESCRIPTION;
                    existingGoodsDonation.DONOR = updatedGoodsDonation.DONOR;

                    // Calculate the difference in item count
                    int itemCountDifference = existingGoodsDonation.ITEM_COUNT -existingcount;

                    // Update the existing entity
                    _context.Update(existingGoodsDonation);

                    // Check if the category exists in the GoodsInventory
                    var inventoryItem = _context.GoodsInventory.FirstOrDefault(g => g.CATGEORY == existingGoodsDonation.CATEGORY);

                    if (inventoryItem != null)
                    {
                        // Update the item count in the existing record
                        inventoryItem.ITEM_COUNT += itemCountDifference;
                    }
                    else
                    {
                        // Create a new record in the GoodsInventory
                        _context.GoodsInventory.Add(new GoodsInventory
                        {
                            CATGEORY = existingGoodsDonation.CATEGORY, // Corrected the property name
                            ITEM_COUNT = existingGoodsDonation.ITEM_COUNT
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsDonationExists(existingGoodsDonation.GOODS_DONATION_ID))
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
            return View(updatedGoodsDonation);
        }



        // GET: UserGoodsDonations/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: UserGoodsDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GoodsDonation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GoodsDonation'  is null.");
            }
            var goodsDonation = await _context.GoodsDonation.FindAsync(id);
            if (goodsDonation != null)
            {
                // Get the category of the goods donation
                var category = goodsDonation.CATEGORY;

                // Update the GoodsInventory item for the category
                var inventoryItem = _context.GoodsInventory.FirstOrDefault(g => g.CATGEORY == category);
                if (inventoryItem != null)
                {
                    inventoryItem.ITEM_COUNT -= goodsDonation.ITEM_COUNT;

                    // If the item count becomes zero, remove the category from GoodsInventory
                    if (inventoryItem.ITEM_COUNT <= 0)
                    {
                        _context.GoodsInventory.Remove(inventoryItem);
                    }
                }

                _context.GoodsDonation.Remove(goodsDonation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool GoodsDonationExists(int id)
        {
            return _context.GoodsDonation.Any(e => e.GOODS_DONATION_ID == id);
        }
    }
}
