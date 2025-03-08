using Domain.Entities.User;

namespace Application.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(ApplicationUser user);
}
