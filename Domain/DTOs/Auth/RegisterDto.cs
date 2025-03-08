using Domain.Abstract;

namespace Domain.DTOs.Auth;

public class RegisterDto : IDto
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}
