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
using System.Globalization;

namespace OpenPriceConfig.Controllers
{
    [Authorize]
    public class OptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OptionsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Options
        public async Task<IActionResult> Index(int? id/*configuratorID*/)
        {
            if (id == null)
                return NotFound();

            var options = from o in _context.Option
                          where o.Configurator.ID == id
                          orderby o.Order
                          select o;

            var optionsq = options
                .Include(o => o.BracketPricing)
                .Include(o => o.DescriptionLocale)
                .Include(o => o.Configurator);

            ViewData["Configurator"] = await _context.Configurator.Where(c => c.ID == id).FirstAsync();

            //var applicationDbContext = _context.Option.Include(o => o.DescriptionLocale);
            return View(await optionsq.ToListAsync());
        }

        // GET: Options/Create
        public async Task<IActionResult> Create(int? id/*configurator id*/)
        {
            if(id == null)
            {
                return NotFound();
            }

            ViewData["Configurator"] = await _context.Configurator.Where(c => c.ID == id).FirstAsync();
            ViewData["ConfiguratorID"] = new SelectList(_context.Configurator, "ID", "Name");
            ViewData["DescriptionLocaleID"] = new SelectList(_context.Locale, "ID", "Tag");
            PopulateInputTypeDropDownList(null);
            PopulateBracketPricingTypeDropDownList(null);
            return View();
        }

        // POST: Options/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id/*Configurator ID*/, [Bind("DescriptionLocaleID,Name,OptionTag,Order,Price,InputType,BracketPricingType")] Option option)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var configuratorQuery = from c in _context.Configurator where (c.ID == id) select c;

                var configurator = await configuratorQuery.FirstOrDefaultAsync();
                option.Configurator = configurator;
                option.UpdateBracketPricings();
                _context.Add(option);
                //option.GenerateBracketPricings(configurator.FloorsNumber);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = configurator.ID });
            }
            ViewData["DescriptionLocaleID"] = new SelectList(_context.Locale, "ID", "ID", option.DescriptionLocaleID);
            return View(option);
        }

        // GET: Options/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var option = await _context.Option
                .Include(o => o.Configurator)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (option == null)
            {
                return NotFound();
            }


            PopulateInputTypeDropDownList(option);
            PopulateBracketPricingTypeDropDownList(option);

            ViewData["DescriptionLocaleID"] = new SelectList(_context.Locale, "ID", "Tag", option.DescriptionLocaleID);
            
            return View(option);
        }

        // POST: Options/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DescriptionLocaleID,Name,OptionTag,Order,Price,InputType,BracketPricingType")] Option option)
        {
            if (id != option.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var configurator = await (from o in _context.Option
                                          where o.ID == option.ID
                                          select o.Configurator).FirstAsync();

                try
                {
                    _context.Update(option);
                    await _context.SaveChangesAsync();

                    option = await _context.Option.Include(o => o.Configurator).Include(o => o.BracketPricing).SingleAsync(o => o.ID == id);
                    option.UpdateBracketPricings();
                    //Need to call again so bracket pricings are updated
                    _context.Update(option);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OptionExists(option.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = configurator.ID });
            }
            ViewData["DescriptionLocaleID"] = new SelectList(_context.Locale, "ID", "ID", option.DescriptionLocaleID);
            return View(option);
        }

        // GET: Options/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var option = await _context.Option.Include(o => o.Configurator).SingleOrDefaultAsync(m => m.ID == id);
            if (option == null)
            {
                return NotFound();
            }

            return View(option);
        }

        // POST: Options/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var option = await _context.Option.Include(o => o.Configurator).SingleOrDefaultAsync(m => m.ID == id);
            var configurator = option.Configurator;
            option.UpdateBracketPricings();
            _context.Option.Remove(option);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = configurator.ID });
        }

        // GET: BracketPricings/Edit/5
        public async Task<IActionResult> EditBracketPricings(int? id/*optionID*/)
        {
            if (id == null)
            {
                return NotFound();
            }

            var option = await _context.Option
                .Where(o => o.ID == id)
                .Include(o => o.Configurator)
                .FirstAsync();

            var bracketPricing = await _context.BracketPricing.Where(b => b.OptionID == id).OrderBy(b => b.Level).ToListAsync();

            //Generate a list of bracket pricing if not existing
            if (bracketPricing == null || bracketPricing.Count == 0)
            {
                option.UpdateBracketPricings();
                _context.Update(option);
                await _context.SaveChangesAsync();
            }

            return View(option);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBracketPricings(int? id, ICollection<BracketPricing> bracketPricing)
        {
            if (id == null)
            {
                return NotFound();
            }

            var option = await _context.Option
                .Where(o => o.ID == id)
                .Include(o => o.Configurator)
                .Include(o => o.BracketPricing)
                .FirstAsync();

            var bpList = bracketPricing.ToList();

            for (int i = 0; i < option.BracketPricing.Count; i++)
            {
                option.BracketPricing[i].Price = bpList[i].Price;
                _context.Update(option.BracketPricing[i]);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = option.Configurator.ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ParseBracketPricingString(int? id, string inputText)
        {
            if (id == null)
            {
                return NotFound();
            }

            char[] validchars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ',' };

            var inputSplit = inputText.Split('\t');
            List<decimal> decimals = new List<decimal>();
            foreach(var inp in inputSplit)
            {
                var chararr = inp.Where(c => validchars.Contains(c)).ToArray();
                var str = new string(chararr).Replace(',', '.');
                decimal dec;
                var ok = decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out dec);

                if (string.IsNullOrEmpty(str) || !ok)
                    continue;

                decimals.Add(dec);
            }

            var option = await _context.Option
                .Where(o => o.ID == id)
                .Include(o => o.Configurator)
                .Include(o => o.BracketPricing)
                .FirstAsync();

            if(option.BracketPricing.Count != decimals.Count)
            {
                return BadRequest();
            }

            var orderedBp = option.BracketPricing.OrderBy(o => o.Level).ToArray();
            for(int i = 0; i < decimals.Count; i++ )
            {
                orderedBp[i].Price = decimals[i];
                _context.Update(orderedBp[i]);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = option.Configurator.ID });
        }

        private bool OptionExists(int id)
        {
            return _context.Option.Any(e => e.ID == id);
        }

        void PopulateInputTypeDropDownList(Option option)
        {
            List<object> inputTypes = new List<object>() { };
            var inputTypesNames = Enum.GetNames(typeof(Option.InputTypes));
            var inputTypesValues = Enum.GetValues(typeof(Option.InputTypes));
            for (int i = 0; i < inputTypesNames.Length; i++)
            {
                inputTypes.Add(new { Value = inputTypesValues.GetValue(i), Name = inputTypesNames[i] });
            }
            ViewData["InputTypeID"] = new SelectList(inputTypes, "Value", "Name", option?.InputType);
        }

        void PopulateBracketPricingTypeDropDownList(Option option)
        {
            List<object> inputTypes = new List<object>() { };
            var bracketPricingTypesNames = Enum.GetNames(typeof(Option.BracketPricingTypes));
            var bracketPricingTypesValues = Enum.GetValues(typeof(Option.BracketPricingTypes));
            for (int i = 0; i < bracketPricingTypesNames.Length; i++)
            {
                inputTypes.Add(new { Value = bracketPricingTypesValues.GetValue(i), Name = bracketPricingTypesNames[i] });
            }
            ViewData["BracketPricingTypeID"] = new SelectList(inputTypes, "Value", "Name", option?.InputType);
        }

    }
}
