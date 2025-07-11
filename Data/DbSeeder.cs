using KulinarstvoASP.Constants;
using KulinarstvoASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                Naziv = "Admin",
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

        public static async Task SeedKategorijeAsync(ApplicationDbContext context)
        {
            if (!context.Kategorije.Any())
            {
                var kategorije = new List<Kategorija>()
                {
                    new Kategorija() {Naziv = "Nekategorizovano", Opis = "Recept nema kategoriju"},
                    new Kategorija() {Naziv = "Dezerti", Opis = "Slatki recepti poput kolača, torti, pudinga, pita i drugih poslastica."},
                    new Kategorija() {Naziv = "Glavna jela", Opis = "Glavni deo obroka, često sa mesom, povrćem, prilogom ili testeninom."},
                    new Kategorija() {Naziv = "Supe i čorbe", Opis = "Tečna jela, od klasičnih bistrih supa do gustih variva i čorbi."},
                    new Kategorija() {Naziv = "Peciva i testa", Opis = "Razne vrste domaćeg hleba, kiflica, pogača, lisnatog i dizanog testa."},
                    new Kategorija() {Naziv = "Vegeterijansko", Opis = "Jela bez mesa, ali sa mlečnim proizvodima i/ili jajima."}
                };

                await context.AddRangeAsync(kategorije);
                await context.SaveChangesAsync();
            }
        }
    }
}
