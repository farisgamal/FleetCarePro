using Microsoft.AspNetCore.Identity;
using MVCDemo.Models;

namespace MVCDemo.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "FleetManager", "Driver" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            await CreateUserIfNotExists(userManager, "admin@fleetcare.com", "Admin@123", "Admin", "System Admin", "EMP-001");
            await CreateUserIfNotExists(userManager, "manager@fleetcare.com", "Manager@123", "FleetManager", "Fleet Manager", "EMP-002");
            await CreateUserIfNotExists(userManager, "driver@fleetcare.com", "Driver@123", "Driver", "Default Driver", "EMP-003");
        }

        private static async Task CreateUserIfNotExists(
            UserManager<ApplicationUser> userManager, string email, string password,
            string role, string fullName, string employeeId)
        {
            if (await userManager.FindByEmailAsync(email) is not null) return;

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FullName = fullName,
                EmployeeId = employeeId
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, role);
        }
    }
}
