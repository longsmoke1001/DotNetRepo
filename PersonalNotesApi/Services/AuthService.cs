using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalNotesApi.Models;

namespace PersonalNotesApi.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthService(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    // 示範用 Hardcode 用戶，真實應用會查 Database
    public User? Authenticate(string username, string password)
    {
        // 示範：只係 hardcode 一個用戶
        if (username == "admin" && password == "password")
        {
            return new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
        }
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return user;
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

    public async Task<User?> Register(string username, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            return null;
        }
        // 真實應用會將用戶資料存入 Database
        // 這裡示範直接返回一個新用戶
        var user = new User { Username = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}