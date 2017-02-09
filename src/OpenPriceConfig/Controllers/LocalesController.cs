using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OpenPriceConfig.Data;
using OpenPriceConfig.Models;

namespace OpenPriceConfig.Controllers
{
    public class LocalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocalesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Locales
        public async Task<IActionResult> Index()
        {
            return View(await _context.Locale.ToListAsync());
        }

        // GET: Locales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locale = await _context.Locale.SingleOrDefaultAsync(m => m.ID == id);
            if (locale == null)
            {
                return NotFound();
            }

            return View(locale);
        }

        // GET: Locales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Tag,Text")] Locale locale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locale);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(locale);
        }

        // GET: Locales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locale = await _context.Locale.SingleOrDefaultAsync(m => m.ID == id);
            if (locale == null)
            {
                return NotFound();
            }
            return View(locale);
        }

        // POST: Locales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Tag,Text")] Locale locale)
        {
            if (id != locale.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocaleExists(locale.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(locale);
        }

        // GET: Locales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locale = await _context.Locale.SingleOrDefaultAsync(m => m.ID == id);
            if (locale == null)
            {
                return NotFound();
            }

            return View(locale);
        }

        // POST: Locales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locale = await _context.Locale.SingleOrDefaultAsync(m => m.ID == id);
            _context.Locale.Remove(locale);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LocaleExists(int id)
        {
            return _context.Locale.Any(e => e.ID == id);
        }
    }
}
