using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2;
using ST10101067_APPR6312_POE_PART_2.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class GoodsAllocationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public GoodsAllocationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Get a list of active disasters for the AidType dropdown
        var activeDisasters = _context.Disatser
            .Where(d => d.IsActive == 1)
            .Select(d => new SelectListItem
            {
                Value = d.AID_TYPE,
                Text = d.AID_TYPE
            })
            .ToList();
        ViewBag.DisasterTypes = activeDisasters;

        // Get a list of available categories for the Goods CATEGORY dropdown
        var availableCategories = _context.GoodsDonation
            .Select(g => g.CATEGORY)
            .Distinct()
            .ToList();
        ViewBag.Categories = availableCategories;

        return _context.GoodsAllocation != null ?
            View(await _context.GoodsAllocation.ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.GoodsAllocation' is null.");
    }

    public IActionResult Create()
    {
        // Get a list of available categories for the CATEGORY dropdown
        var availableCategories = _context.GoodsDonation
            .Select(g => g.CATEGORY)
            .Distinct()
            .ToList();
        ViewBag.Categories = availableCategories;

        // Get a list of active disasters for the AidType dropdown
        var activeDisasters = _context.Disatser
            .Where(d => d.IsActive == 1)
            .Select(d => new SelectListItem
            {
                Value = d.AID_TYPE,
                Text = d.AID_TYPE
            })
            .Distinct()
            .ToList();
        ViewBag.DisasterTypes = activeDisasters;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ITEM_COUNT, AllocationDate")] GoodsAllocation goodsAllocation, string category, string aidType)
    {
        if (ModelState.IsValid)
        {
            // Set the AllocationDate
            goodsAllocation.AllocationDate = DateTime.Now.Date;

            // Set the CATEGORY and AidType based on user selections
            goodsAllocation.CATEGORY = category;
            goodsAllocation.AidType = aidType;

            // Retrieve the selected good from the database
            var selectedGood = _context.GoodsInventory
                .FirstOrDefault(g => g.CATGEORY == category);

            if (selectedGood != null)
            {
                // Decrease the ITEM_COUNT of the selected good
                selectedGood.ITEM_COUNT -= goodsAllocation.ITEM_COUNT;

                // Add the goodsAllocation to the context
                _context.GoodsAllocation.Add(goodsAllocation);

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Add the purchased category and ITEM COUNT to the GoodsDonation list
                var goodsPurchased = _context.GoodsPurchase.FirstOrDefault(p => p.CATEGORY == category);
                if (goodsPurchased != null)
                {
                    selectedGood.ITEM_COUNT += goodsPurchased.ITEM_COUNT;
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle the case where the selected good is not found
                ModelState.AddModelError("CATEGORY", "Selected category not found.");
            }
        }

        return View(goodsAllocation);
    }

    [HttpPost]
    public IActionResult GetSumOfItemCount(string category)
    {
        // Calculate the sum of the ITEM_COUNT for the selected category
        var sumOfItemCount = _context.GoodsDonation
            .Where(g => g.CATEGORY == category)
            .Sum(g => g.ITEM_COUNT);

        return Json(sumOfItemCount);
    }
}