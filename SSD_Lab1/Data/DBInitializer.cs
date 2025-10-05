using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSD_Lab1.Models;

namespace SSD_Lab1.Data
{
    public static class DbInitializer
    {
        public static appSecrets appSecrets { get; set; }
        public static async Task<int> SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            // make sure db is made
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // if roles already exis then skip
            if (roleManager.Roles.Any())
                return 1;

            // seed roles
            var roleResult = await SeedRoles(roleManager);
            if (roleResult != 0)
                return 2;

            // if users already exist then skip
            if (userManager.Users.Any())
                return 3;

            // seed users
            var userResult = await SeedUsers(userManager);
            if (userResult != 0)
                return 4;

            return 0;
        }

        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var result = await roleManager.CreateAsync(new IdentityRole("Supervisor"));
            if (!result.Succeeded) return 1;

            result = await roleManager.CreateAsync(new IdentityRole("Employee"));
            if (!result.Succeeded) return 2;

            return 0;
        }

        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Supervisor user
            var supervisor = new ApplicationUser
            {
                UserName = "supervisor@gmail.com",
                Email = "supervisor@gmail.com",
                FirstName = "Super",
                LastName = "Visor",
                City = "Hamilton",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(supervisor, appSecrets.SupervisorPassword);
            if (!result.Succeeded) return 1;

            result = await userManager.AddToRoleAsync(supervisor, "Supervisor");
            if (!result.Succeeded) return 2;

            // Employee user
            var employee = new ApplicationUser
            {
                UserName = "employee@gmail.com",
                Email = "employee@gmail.com",
                FirstName = "Em",
                LastName = "Ployee",
                City = "Hamilton",
                EmailConfirmed = true
            };

            result = await userManager.CreateAsync(employee, appSecrets.EmployeePassword);
            if (!result.Succeeded) return 3;

            result = await userManager.AddToRoleAsync(employee, "Employee");
            if (!result.Succeeded) return 4;

            return 0;
        }
    }
}
