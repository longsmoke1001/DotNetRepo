using Microsoft.AspNetCore.Mvc;
using PersonalNotesApi.Services;
using PersonalNotesApi.Models;

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
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _authService.Authenticate(request.Username, request.Password);
        if (user == null)
            return Unauthorized("用戶名或密碼錯誤");

        var token = _authService.GenerateToken(user);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Implementation for user registration
        var user = await _authService.Register(request.Username, request.Password);
        if (user == null)
            return BadRequest("format error or user already exists");
        var token = _authService.GenerateToken(user);
        return Ok(new { token });
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}