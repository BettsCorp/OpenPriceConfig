using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenPriceConfig.Data;
using Microsoft.EntityFrameworkCore;
using OpenPriceConfig.Models;

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
            if (id == null)
            {
                return RedirectToAction(nameof(HomeController.Empty));
            }

            var configurator = await _context.Configurator.Where(c => c.ID == id)
                .Include(c => c.Options)
                .SingleAsync();

            return View(configurator);
        }

        [HttpPost]
        public async Task<string> GeneratePrice(int? id)
        {
            if (id == null)
                NotFound();

            //Gather all form keys and values in a dictionary
            var dict = new Dictionary<string, object>();
            var keys = Request.Form.Keys;
            string output = "";
            foreach (var key in keys)
            {
                dict.Add(key, Request.Form[key]);
                output += $"{key} = {Request.Form[key]}\n";
            }

            var configurator = await _context.Configurator
                .Where(c => c.ID == id)
                .Include(c => c.Options)
                    .ThenInclude(o => o.BracketPricing)
                .SingleAsync();

            var numberOfFloors = int.Parse(dict["NUMBER_OF_FLOORS"].ToString());
            decimal price = 0M;

            foreach(var kvp in dict)
            {
                decimal itemPrice = 0M;
                if (kvp.Key.StartsWith("ITEM_"))
                {
                    var inputId = int.Parse(kvp.Key.Replace("ITEM_", ""));
                    var option = configurator.Options.Where(o => o.ID == inputId).Single();
                    if(option.BracketPricing == null || option.BracketPricing.Count == 0)
                    {
                        itemPrice = option.Price;
                    }
                    else
                    {
                        itemPrice = option.BracketPricing.Where(b => b.ForFloorNumber == numberOfFloors).Single().Price;
                    }

                    output += Environment.NewLine + $"{option.Name} : {itemPrice}";
                }
                else if(kvp.Key.StartsWith("OPTION_"))
                {

                }

                price += itemPrice;
            }


            output += Environment.NewLine + $"Price SUM: {price}";


            
            return output;
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
