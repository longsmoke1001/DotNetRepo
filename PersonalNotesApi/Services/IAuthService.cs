using PersonalNotesApi.Models;

namespace PersonalNotesApi.Services;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(string username, string password);
    string GenerateToken(User user);
    Task<User?> RegisterAsync(string username, string password);
    Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
}