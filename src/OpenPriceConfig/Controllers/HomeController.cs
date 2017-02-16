using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenPriceConfig.Data;
using Microsoft.EntityFrameworkCore;
using OpenPriceConfig.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace OpenPriceConfig.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Empty()
        {
            return View();
        }
        
        public async Task<IActionResult> Index(int? id /*configuratorID*/)
        {
            Configurator configurator = null;
            var query = from c in _context.Configurator select c;
            query = query.Include(c => c.Options);

            try
            {
                configurator = await query.FirstAsync();
                //configurator = await query.SingleAsync(c => c.ID == id);
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Empty));
            }

            //if (id == null)
            //{
            //    configurator = await query.FirstAsync();
            //}
            //else
            //{
            //    configurator = await query.SingleAsync(c => c.ID == id);
            //}

            //if(configurator == null)
            //{
            //    return RedirectToAction(nameof(HomeController.Empty));
            //}

            return View(configurator);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePrice(int? id)
        {
            if (id == null)
                NotFound();

            //Gather all form keys and values in a dictionary
            var dict = FormRequest2Dict();

            var vm = new OfferViewModel();

            var configurator = await _context.Configurator
                .Where(c => c.ID == id)
                .Include(c => c.Options).ThenInclude(o => o.BracketPricing)
                .Include(c => c.Options).ThenInclude(o => o.DescriptionLocale)
                .SingleAsync();

            vm.Name = configurator.Name;

            var numberOfFloors = int.Parse(dict["NUMBER_OF_FLOORS"].ToString());
            var numberOfWires = int.Parse(dict["NUMBER_OF_WIRES"].ToString());

            foreach (var kvp in dict)
            {
                Option option = null;

                if (kvp.Key.StartsWith("ITEM_"))
                {
                    var inputId = int.Parse(kvp.Key.Replace("ITEM_", ""));
                    option = configurator.Options.Where(o => o.ID == inputId).Single();
                }
                else if(kvp.Key.StartsWith("OPTION_"))
                {
                    int inputId = int.Parse(kvp.Value.ToString());
                    option = configurator.Options.Where(o => o.ID == inputId).Single();
                }

                if(option != null)
                {
                    var oi = new OfferViewModel.OfferItem()
                    {

                        Name = option.Name,
                        Description = option.DescriptionLocale.Text,
                        Price = option.GetPrice(numberOfFloors, numberOfWires),
                        TextValue = option.InputType == Option.InputTypes.Numeric ||
                                    option.InputType == Option.InputTypes.Text ?
                                    kvp.Value.ToString() : "",
                        HasPrice = option.InputType == Option.InputTypes.Checkbox ||
                                    option.InputType == Option.InputTypes.Option
                    };

                    if (oi.HasPrice && oi.Price == 0M)
                        continue;

                    vm.Items.Add(oi);

                }
            }

            return View(vm);
        }
        

        public IActionResult Error()
        {
            return View();
        }



        Dictionary<string, object> FormRequest2Dict()
        {
            var dict = new Dictionary<string, object>();
            var keys = Request.Form.Keys;
            
            foreach (var key in keys)
            {
                dict.Add(key, Request.Form[key]);
            }

            return dict;
        }

    }
}

