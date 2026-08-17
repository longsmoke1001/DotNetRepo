using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PersonalNotesApi.Models;

namespace PersonalNotesApi.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // 示範用 Hardcode 用戶，真實應用會查 Database
    public User? Authenticate(string username, string password)
    {
        // 示範：只係 hardcode 一個用戶
        if (username == "admin" && password == "password")
        {
            return new User { Id = 1, Username = "admin" };
        }
        return null;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Secret"] ?? "your-super-secret-key-at-least-32-chars-long"));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "PersonalNotesApi",
            audience: _configuration["Jwt:Audience"] ?? "PersonalNotesApiUsers",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}