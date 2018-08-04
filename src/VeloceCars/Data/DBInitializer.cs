using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeloceCars.Models;

namespace VeloceCars.Data
{
    public static class DBInitializer
    {

        public async static void Initializes(ApplicationDbContext context, IServiceProvider provider)
        {

            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            List<IdentityRole> roles = new List<IdentityRole>();
            roles.Add(new IdentityRole { Name = "Administrator", NormalizedName = "ADMINISTRATOR" });
            roles.Add(new IdentityRole { Name = "Headquarters User", NormalizedName = "HEADQUARTERS" });
            roles.Add(new IdentityRole { Name = "Branch User", NormalizedName = "BRANCH" });
            roles.Add(new IdentityRole { Name = "Driver", NormalizedName = "DRIVER" });
            roles.Add(new IdentityRole { Name = "Client", NormalizedName = "CLIENT" });


            IdentityResult result;

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role.Name);
                if (!roleExist)
                {
                    result = await roleManager.CreateAsync(new IdentityRole(role.Name));
                }
            }



            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add( new ApplicationUser
            {
                Firstname = "Veloce",
                Lastname = "Admin",
                Email = "admin@velocecars.com",
                UserName = "admin@velocecars.com",
                AccessFailedCount = 0,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false
            });

            users.Add(new ApplicationUser
            {
                Firstname = "Driver",
                Lastname = "Bratt",
                Email = "bratt@velocecars.com",
                UserName = "bratt@velocecars.com",
                AccessFailedCount = 0,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false
            });

            users.Add(new ApplicationUser
            {
                Firstname = "Matt",
                Lastname = "Damon",
                Email = "matt@velocecars.com",
                UserName = "matt@velocecars.com",
                AccessFailedCount = 0,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false
            });

            users.Add(new ApplicationUser
            {
                Firstname = "Branch",
                Lastname = "Mia",
                Email = "mia@velocecars.com",
                UserName = "mia@velocecars.com",
                AccessFailedCount = 0,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false
            });

            users.Add(new ApplicationUser
            {
                Firstname = "HQ",
                Lastname = "Jason",
                Email = "Jason@velocecars.com",
                UserName = "Jason@velocecars.com",
                AccessFailedCount = 0,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false
            });

            users.Add(new ApplicationUser
            {
                Firstname = "Client",
                Lastname = "Robert",
                Email = "robert@velocecars.com",
                UserName = "robert@velocecars.com",
                AccessFailedCount = 0,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false
            });


            foreach (var user in users)
            {
                var userExist = await userManager.FindByNameAsync(user.UserName);
                if (userExist == null)
                {
                    result = await userManager.CreateAsync(user);
                    user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "_Veloce123");
                }
            }
     

            var newUser =  await userManager.FindByEmailAsync("admin@velocecars.com");
            await userManager.AddToRoleAsync(newUser, "Administrator");

            newUser = await userManager.FindByEmailAsync("bratt@velocecars.com");
            await userManager.AddToRoleAsync(newUser, "Driver");

            newUser = await userManager.FindByEmailAsync("matt@velocecars.com");
            await userManager.AddToRoleAsync(newUser, "Driver");

            newUser = await userManager.FindByEmailAsync("mia@velocecars.com");
            await userManager.AddToRoleAsync(newUser, "Branch User");

            newUser = await userManager.FindByEmailAsync("Jason@velocecars.com");
            await userManager.AddToRoleAsync(newUser, "Headquarters User");

            newUser = await userManager.FindByEmailAsync("robert@velocecars.com");
            await userManager.AddToRoleAsync(newUser, "Client");

        }
    }
}
