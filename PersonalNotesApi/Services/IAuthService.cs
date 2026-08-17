using PersonalNotesApi.Models;

namespace PersonalNotesApi.Services;

public interface IAuthService
{
    User? Authenticate(string username, string password);
    string GenerateToken(User user);
}