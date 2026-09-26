using Microsoft.AspNetCore.Mvc;
using PersonalNotesApi.Services;
using PersonalNotesApi.Models;
using System.Security.Claims;

namespace PersonalNotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.AuthenticateAsync(request.Username, request.Password);
        if (user == null)
            return Unauthorized("用戶名或密碼錯誤");

        var token = _authService.GenerateToken(user);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Implementation for user registration
        var user = await _authService.RegisterAsync(request.Username, request.Password);
        if (user == null)
            return BadRequest("format error or user already exists");
        var token = _authService.GenerateToken(user);
        return Ok(new { token });
    }

    [HttpPost("change-password")]
public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    var result = await _authService.ChangePasswordAsync(userId, request.OldPassword, request.NewPassword);
    if (!result)
        return BadRequest("舊密碼錯誤");

    return Ok("密碼已更改");
}
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}