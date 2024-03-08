using Akalaat.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Data
{
    public class DataSeeding
    {
        public static async Task SeedDataAsync(AkalaatDbContext context, ILoggerFactory loggerFactory
            , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                if (!context.Roles.Any())
                {
                    List<string> roles = new List<string>() { "Admin", "Customer", "Vendor" };
                    foreach (var item in roles)
                    {
                        var R = new IdentityRole() { Name = item };
                        var result = await roleManager.CreateAsync(R);
                        if (!result.Succeeded)
                        {
                            throw new Exception(message: "Cannot add this role");
                        }
                    }
                }

                if (!context.Admins.Any())
                {
                    var adminPass = "P@ssw0rd";
                    var Admin = new ApplicationUser() {UserName = "admin", Email = "admin@gmail.com" };
                    var result = await userManager.CreateAsync(Admin, adminPass);

                    var role = await roleManager.FindByNameAsync("Admin");
                    var added = await userManager.AddToRoleAsync(Admin, role.Name);

                }
                await context.SaveChangesAsync();
            }

            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<DataSeeding>();
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
