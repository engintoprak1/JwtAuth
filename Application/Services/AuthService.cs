using Application.Interfaces;
using Domain.DTOs.Auth;
using Domain.DTOs.Role;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService) : IAuthService
{
    public async Task<string> RegisterAsync(RegisterDto request)
    {
        var user = new ApplicationUser { UserName = request.Email, FirstName = request.Firstname, LastName = request.Lastname, Email = request.Email, PhoneNumber = request.PhoneNumber, EmailConfirmed = true };
        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, Authorization.default_role.ToString());
            return "User has been created is successfully.";
        }
            

        throw new Exception("Registration failed: " + string.Join(" ", result.Errors.Select(x=>x.Description).ToList()));
    }

    public async Task<string> LoginAsync(LoginDto request)
    {
        var user = await userManager.FindByEmailAsync(request.Email) ?? throw new Exception("Invalid username or password.");

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            throw new Exception("Invalid username or password.");

        return await tokenService.CreateToken(user);
    }

    public async Task<string> AddRoleAsync(AddRoleDto request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return $"No Accounts Registered with {request.Email}.";
        }

        if (await userManager.CheckPasswordAsync(user, request.Password))
        {
            var roleExists = Enum.GetNames(typeof(Authorization.Roles))
                .Any(x => x.ToLower() == request.Role.ToLower());

            if (roleExists)
            {
                var validRole = Enum.GetValues(typeof(Authorization.Roles)).Cast<Authorization.Roles>()
                    .Where(x => x.ToString().ToLower() == request.Role.ToLower())
                    .FirstOrDefault();

                await userManager.AddToRoleAsync(user, validRole.ToString());
                return $"Added {request.Role} to user {request.Email}.";
            }

            return $"Role {request.Role} not found.";
        }
        return $"Incorrect Credentials for user {user.Email}.";
    }

    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Enum.GetNames(typeof(Authorization.Roles)))
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

}
