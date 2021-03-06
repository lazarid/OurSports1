﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OurSports1.Data;
using OurSports1.Models;
using Microsoft.AspNetCore.Authorization;

namespace OurSports1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StadiumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StadiumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stadiums.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Stadiums
                .SingleOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,State,Street,City,NumHouse")] Stadiums s)
        {
            if (ModelState.IsValid)
            {
                _context.Add(s);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(s);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var s = await _context.Category.SingleOrDefaultAsync(m => m.ID == id);
            if (s == null)
            {
                return NotFound();
            }
            return View(s);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,State,Street,City,NumHouse")] Stadiums s)
        {
            if (id != s.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(s);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(s.ID))
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
            return View(s);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var s = await _context.Stadiums
                .SingleOrDefaultAsync(m => m.ID == id);
            if (s == null)
            {
                return NotFound();
            }

            return View(s);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Stadiums.SingleOrDefaultAsync(m => m.ID == id);
            _context.Stadiums.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Stadiums.Any(e => e.ID == id);
        }
    }
}
