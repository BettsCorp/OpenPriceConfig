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
        public async Task<string> GeneratePrice()
        {
            var dict = new Dictionary<string, object>();
            var keys = Request.Form.Keys;
            string output = "";
            foreach (var key in keys)
            {
                dict.Add(key, Request.Form[key]);
                output += $"{key} = {Request.Form[key]}\n";
            }

            
            await Task.Delay(1);
            return output;
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
