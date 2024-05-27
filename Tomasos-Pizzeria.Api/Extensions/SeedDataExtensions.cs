using Microsoft.AspNetCore.Identity;

namespace Tomasos_Pizzeria.Api.Extensions
{
    public static class SeedDataExtensions
    {
        public static async Task InitializeRolesAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "RegularUser", "PremiumUser" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
        }
    }
}
