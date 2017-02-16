using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OpenPriceConfig.Data;
using OpenPriceConfig.Models;
using Microsoft.AspNetCore.Authorization;

namespace OpenPriceConfig.Controllers
{
    [Authorize]
    public class ConfiguratorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguratorsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Configurators
        public async Task<IActionResult> Index()
        {
            return View(await _context.Configurator.ToListAsync());
        }

        // GET: Configurators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Configurators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Enabled,FloorsNumber,Name,WiresNumber")] Configurator configurator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(configurator);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(configurator);
        }

        // GET: Configurators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configurator = await _context.Configurator.SingleOrDefaultAsync(m => m.ID == id);
            if (configurator == null)
            {
                return NotFound();
            }
            return View(configurator);
        }

        // POST: Configurators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Enabled,FloorsNumber,Name,WiresNumber")] Configurator configurator)
        {
            if (id != configurator.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                try
                {
                    
                    var options = await _context.Option.Include(o => o.BracketPricing).ToListAsync();
                    foreach (var option in options)
                    {
                        option.Configurator = configurator;
                        option.UpdateBracketPricings();
                        _context.Update(option);
                    }

                    _context.Update(configurator);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConfiguratorExists(configurator.ID))
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
            return View(configurator);
        }

        // GET: Configurators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configurator = await _context.Configurator.SingleOrDefaultAsync(m => m.ID == id);
            if (configurator == null)
            {
                return NotFound();
            }

            return View(configurator);
        }

        // POST: Configurators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var configurator = await _context.Configurator.SingleOrDefaultAsync(m => m.ID == id);
            _context.Configurator.Remove(configurator);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ConfiguratorExists(int id)
        {
            return _context.Configurator.Any(e => e.ID == id);
        }
    }
}
