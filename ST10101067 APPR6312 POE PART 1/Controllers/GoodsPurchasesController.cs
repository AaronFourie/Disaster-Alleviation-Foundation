using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2;
using ST10101067_APPR6312_POE_PART_2.Models;

namespace ST10101067_APPR6312_POE_PART_2.Controllers
{
    public class GoodsPurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GoodsPurchasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Retrieve the first record from the Money table
            Money firstMoneyRecord = _context.Money.FirstOrDefault();

            // Calculate available money from the first record
            decimal availableMoney = firstMoneyRecord != null ? firstMoneyRecord.RemainingMoney : 0;
            ViewBag.AvailableMoney = availableMoney;

            return View(await _context.GoodsPurchase.ToListAsync());
        }

        public IActionResult Create()
        {
            // Retrieve the first record from the Money table
            Money firstMoneyRecord = _context.Money.FirstOrDefault();

            // Calculate available money from the first record
            decimal availableMoney = firstMoneyRecord != null ? firstMoneyRecord.RemainingMoney : 0;
            ViewBag.AvailableMoney = availableMoney;

            // Prepare the list of available categories
            var categories = _context.GoodsDonation.Select(g => g.CATEGORY).Distinct();
            ViewBag.Categories = new SelectList(categories);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoodsPurchaseID, GoodsPurchasePrice, ITEM_COUNT, GoodsTotalPrice, CATEGORY")] GoodsPurchase goodsPurchase, string category)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the first record from the Money table
                Money firstMoneyRecord = _context.Money.FirstOrDefault();

                if (firstMoneyRecord != null)
                {
                    decimal availableMoney = firstMoneyRecord.RemainingMoney;
                    decimal totalCost = goodsPurchase.GoodsPurchasePrice * goodsPurchase.ITEM_COUNT;

                    if (totalCost <= availableMoney)
                    {
                        // Check if the category already exists in the GoodsInventory
                        var inventoryItem = _context.GoodsInventory.FirstOrDefault(g => g.CATGEORY == goodsPurchase.CATEGORY);

                        if (inventoryItem != null)
                        {
                            // Update the item count in the existing record
                            inventoryItem.ITEM_COUNT += goodsPurchase.ITEM_COUNT;
                        }
                        else
                        {
                            // Create a new record in the GoodsInventory
                            _context.GoodsInventory.Add(new GoodsInventory
                            {
                                CATGEORY = goodsPurchase.CATEGORY,
                                ITEM_COUNT = goodsPurchase.ITEM_COUNT
                            });
                        }

                        // Subtract the totalCost from available money
                        availableMoney -= totalCost;

                        // Calculate the GoodsTotalPrice
                        goodsPurchase.GoodsTotalPrice = totalCost;

                        // Update the remaining money in the Money table
                        firstMoneyRecord.RemainingMoney = availableMoney;

                        // Add the purchase details to the database
                        _context.GoodsPurchase.Add(goodsPurchase);

                        // After saving the purchase, update the GoodsAllocation list
                        var goodsAllocation = _context.GoodsAllocation.ToList();
                        ViewBag.GoodsAllocationList = new SelectList(goodsAllocation, "CATEGORY", "ITEM_COUNT");

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Construct the custom error message with the total cost and available money
                        string errorMessage = $"The calculated total cost {totalCost:C} exceeds the available money {availableMoney:C}.";

                        // Add the error message to the ModelState
                        ModelState.AddModelError("ITEM_COUNT", errorMessage);
                    }
                }
            }

            // If the model is invalid, reload the list of categories
            var categories = _context.GoodsDonation.Select(g => g.CATEGORY).Distinct();
            ViewBag.Categories = new SelectList(categories);

            return View(goodsPurchase);
        }
    }
}

