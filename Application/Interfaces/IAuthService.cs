using Domain.DTOs.Auth;
using Domain.DTOs.Role;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto request);
    Task<string> LoginAsync(LoginDto request);
    Task<string> AddRoleAsync(AddRoleDto request);
}
