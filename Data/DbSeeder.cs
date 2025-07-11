using KulinarstvoASP.Constants;
using KulinarstvoASP.Models;
using Microsoft.AspNetCore.Identity;

namespace KulinarstvoASP.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            var user = new ApplicationUser
            {
                UserName = "admin@kulinarstvo.rs",
                Email = "admin@kulinarstvo.rs",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if(userInDb == null)
            {
                await userManager.CreateAsync(user, "Admin123$");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
        }
    }
}
