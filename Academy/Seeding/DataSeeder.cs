using Academy.Models;
using Microsoft.AspNetCore.Identity;

namespace Academy.Seeding
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            // Get required services
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create the Admin role if it doesn't exist
            string adminRoleName = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            }

            // Create the Admin user if it doesn't exist
            string adminEmail = "admin@info.com";
            string adminPassword = "P@ssw0rd";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    EntityName = "Admin",
                };

                var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (createUserResult.Succeeded)
                {
                    // Assign the Admin role to the user
                    await userManager.AddToRoleAsync(adminUser, adminRoleName);
                }
            }
            //if(adminUser != null)
            //{
            //    if(!await userManager.IsInRoleAsync(adminUser, adminRoleName))
            //    {
            //        await userManager.AddToRoleAsync(adminUser, adminRoleName);
            //    }
              
            //}
        }
    }

}
