using Microsoft.AspNetCore.Mvc;
using PersonalNotesApi.Services;

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
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}