using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2.Models;

namespace ST10101067_APPR6312_POE_PART_2.Controllers
{
    
    public class MoneyAllocationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MoneyAllocationsController> _logger;

        public MoneyAllocationsController(ApplicationDbContext context, ILogger<MoneyAllocationsController> logger)
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var activeDisasters = _context.Disatser
                .Where(d => d.IsActive == 1)
                .Select(d => new SelectListItem
                {
                    Value = d.DISTATER_ID.ToString(),
                    Text = d.AID_TYPE
                })
                .ToList();

            ViewBag.DisasterTypes = activeDisasters;

            var money = _context.Money.FirstOrDefault();
            ViewBag.RemainingMoney = money?.RemainingMoney ?? 0.0m;

            // Calculate the total amount of money allocated
            decimal totalAllocated = _context.MoneyAllocation.Sum(m => m.AllocationAmount);
            ViewBag.Total = totalAllocated;

            return _context.MoneyAllocation != null ?
                  View(await _context.MoneyAllocation.ToListAsync()) :
                  Problem("Entity set 'ApplicationDbContext.GoodsDonation' is null.");
        }

        public IActionResult Create()
        {
            var activeDisasters = _context.Disatser
                .Where(d => d.IsActive == 1)
                .Select(d => new SelectListItem
                {
                    Value = d.DISTATER_ID.ToString(),
                    Text = d.AID_TYPE
                })
                .ToList();

            ViewBag.DisasterTypes = activeDisasters;

            var money = _context.Money.FirstOrDefault();
            ViewBag.RemainingMoney = money?.RemainingMoney ?? 0.0m;

            // Calculate the total amount of money allocated
            decimal totalAllocated = _context.MoneyAllocation.Sum(m => m.AllocationAmount);
            ViewBag.Total = totalAllocated;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AllocationAmount, AllocationDate, AidType")] MoneyAllocation moneyAllocation, int DisasterId)
        {
            var money = _context.Money.FirstOrDefault();
            decimal totalAllocated;

            if (ModelState.IsValid)
            {
                if (money != null)
                {
                    if (moneyAllocation.AllocationAmount <= money.RemainingMoney && moneyAllocation.AllocationAmount >= 0)
                    {
                        // Set AllocationDate to the current date
                        moneyAllocation.AllocationDate = DateTime.UtcNow.Date;

                        // Set DisasterId from the parameter
                        moneyAllocation.DisasterId = DisasterId;

                        // Retrieve the corresponding Disaster entity
                        var selectedDisaster = _context.Disatser.FirstOrDefault(d => d.DISTATER_ID == DisasterId);

                        if (selectedDisaster != null)
                        {
                            // Set the AidType property from the selected Disaster entity
                            moneyAllocation.AidType = selectedDisaster.AID_TYPE.ToString();

                            // Deduct the allocation amount from remaining money
                            money.RemainingMoney -= moneyAllocation.AllocationAmount;

                            // Update the total money allocated
                            totalAllocated = _context.MoneyAllocation.Sum(m => m.AllocationAmount);
                            ViewBag.Total = totalAllocated;

                            _context.Update(money);

                            // Save the money allocation record to the database
                            _context.MoneyAllocation.Add(moneyAllocation);
                            await _context.SaveChangesAsync();

                            // Update the remaining money
                            ViewBag.RemainingMoney = money.RemainingMoney;

                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("DisasterId", "Selected Disaster not found.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("AllocationAmount", "Invalid allocation amount or insufficient funds.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Money is null");
                }
            }
            else
            {
                // Log model state errors using ILogger
                if (!ModelState.IsValid)
                {
                    foreach (var key in ModelState.Keys)
                    {
                        var modelStateEntry = ModelState[key];
                        if (modelStateEntry.Errors.Any())
                        {
                            foreach (var error in modelStateEntry.Errors)
                            {
                                _logger.LogError($"Key: {key}, Error: {error.ErrorMessage}");
                            }
                        }
                    }
                }
            }

            // Repopulate the dropdown list and remaining money for the view
            var disasterTypes = _context.Disatser
                .Where(d => d.IsActive == 1)
                .Select(d => new SelectListItem
                {
                    Value = d.DISTATER_ID.ToString(),
                    Text = d.AID_TYPE
                })
                .ToList();
            ViewBag.DisasterTypes = disasterTypes;

            // Get the current remaining money
            ViewBag.RemainingMoney = money?.RemainingMoney ?? 0.0m;

            // Calculate the total amount of money allocated
            totalAllocated = _context.MoneyAllocation.Sum(m => m.AllocationAmount);
            ViewBag.Total = totalAllocated;

            return View(moneyAllocation);
        }


   

       
    }
}