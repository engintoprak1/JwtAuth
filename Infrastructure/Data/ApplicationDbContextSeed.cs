using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public class ApplicationDbContextSeed
{
    public static async Task SeedEssentialAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Enum.GetNames(typeof(Authorization.Roles)))
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var defaultUser = await userManager.FindByEmailAsync(Authorization.default_email);

        if (defaultUser == null)
        {
            defaultUser = new ApplicationUser
            {
                UserName = Authorization.default_username,
                FirstName = Authorization.default_firstname,
                LastName = Authorization.default_lastname,
                Email = Authorization.default_email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(defaultUser, Authorization.default_password);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create default user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        var isInRole = await userManager.IsInRoleAsync(defaultUser, Authorization.default_role.ToString());
        if (!isInRole)
        {
            await userManager.AddToRoleAsync(defaultUser, Authorization.default_role.ToString());
        }
    }
}
