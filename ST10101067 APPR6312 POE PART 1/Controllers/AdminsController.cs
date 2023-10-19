using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10101067_APPR6312_POE_PART_2;
using ST10101067_APPR6312_POE_PART_2.Data.Migrations;
using ST10101067_APPR6312_POE_PART_2.Models;

namespace ST10101067_APPR6312_POE_PART_2.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;


        public AdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
              return _context.Admin != null ? 
                          View(await _context.Admin.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Admin'  is null.");
        }

        public async Task<IActionResult> Crud()
        {

            return View();

        }
        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin
                .FirstOrDefaultAsync(m => m.ADMIN_ID == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ADMIN_ID,EMAIL,PASSWORD")] Admin admin)
        {

            if (ModelState.IsValid)
            {
                if (admin.EMAIL != "aaronfourie@gmail.com" && admin.PASSWORD != "password1")
                {
                    return RedirectToAction("Create", "Admins");
                }
                else
                {
                    return RedirectToAction("Crud", "Admins");
                }

            }
            return View(admin);
        }

    }
}
