using Application.Interfaces;
using Domain.DTOs.Auth;
using Domain.DTOs.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthAPI.Controllers;

[Route("[controller]/")]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto request)
    {
        try
        {
            var message = await _authService.RegisterAsync(request);
            return Ok(message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto request)
    {
        try
        {
            var token = await _authService.LoginAsync(request);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-role")]
    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDto request)
    {
        try
        {
            var message = await _authService.AddRoleAsync(request);
            return Ok(message);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
}

