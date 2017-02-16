using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenPriceConfig.Data;

namespace OpenPriceConfig.Models
{
    public static class SeedData
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                await InitializeRoles(context, serviceProvider);
                await InitializeUsers(context, serviceProvider);
                await InitializeLocale(context, serviceProvider);

                await FixBracketPricings(context, serviceProvider);
            }

        }

        static async Task InitializeRoles(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var changes = false;
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in Roles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(role.NormalizedName))
                {
                    context.Roles.Add(role);
                    changes |= true;
                }
            }

            if (changes)
                context.SaveChanges();

            roleManager.Dispose();
        }

        static async Task InitializeUsers(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Users.Any())
            {
                return;
            }


            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var admin = new ApplicationUser()
            {
                UserName = "admin@opc",
                Email = "admin@opc",
            };

            await userManager.CreateAsync(admin, "Opc123!");

            var result = await userManager.AddToRoleAsync(admin, Roles.Admin.NormalizedName);

            await context.SaveChangesAsync();

            userManager.Dispose();
        }

        static async Task InitializeLocale(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Locale.Any())
            {
                return;
            }

            context.Locale.Add(new Locale() { Tag = "-", Text = null });

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Temporary fix for moving (obsolete)ForFloorNumber to new parameter Level
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        static async Task FixBracketPricings(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var bracketPricings = await context.BracketPricing.ToListAsync();

            foreach(var bp in bracketPricings)
            {
                bp.Level = bp.ForFloorNumber;
                context.Update(bp);
            }

            await context.SaveChangesAsync();
        }
    }
}
