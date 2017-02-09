using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OpenPriceConfig.Models
{
    public static class Roles
    {

        public static readonly IdentityRole Admin = new IdentityRole() { Name = "Admin", NormalizedName = "ADMINISTRATOR" };

        public static IEnumerable<IdentityRole> AllRoles = new List<IdentityRole> {
            Admin,
        };

    }
}
