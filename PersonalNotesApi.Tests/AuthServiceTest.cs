using Microsoft.EntityFrameworkCore;
using PersonalNotesApi.Data;
using PersonalNotesApi.Models;
using PersonalNotesApi.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace PersonalNotesApi.Tests;

public class AuthServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private IConfiguration GetConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Secret"] = "test-secret-key-at-least-32-characters-long",
                ["Jwt:Issuer"] = "PersonalNotesApi",
                ["Jwt:Audience"] = "PersonalNotesApiUsers"
            })
            .Build();

        return config;
    }

    [Fact]
    public async Task Register_ShouldCreateUser()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var config = GetConfiguration();
        var service = new AuthService(config, context);

        // Act
        var user = await service.RegisterAsync("leo", "password123");

        // Assert
        Assert.NotNull(user);
        Assert.Equal("leo", user.Username);
        Assert.Single(context.Users);
    }

    [Fact]
    public async Task Register_ShouldReturnNull_IfUsernameExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var config = GetConfiguration();
        var service = new AuthService(config, context);

        await service.RegisterAsync("leo", "password123");

        // Act
        var user = await service.RegisterAsync("leo", "password456");

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnUser_WithCorrectPassword()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var config = GetConfiguration();
        var service = new AuthService(config, context);

        await service.RegisterAsync("leo", "password123");

        // Act
        var user = await service.AuthenticateAsync("leo", "password123");

        // Assert
        Assert.NotNull(user);
        Assert.Equal("leo", user.Username);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnNull_WithWrongPassword()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var config = GetConfiguration();
        var service = new AuthService(config, context);

        await service.RegisterAsync("leo", "password123");

        // Act
        var user = await service.AuthenticateAsync("leo", "wrongpassword");

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var config = GetConfiguration();
        var service = new AuthService(config, context);

        var user = new User { Id = 1, Username = "leo", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123") };

        // Act
        var token = service.GenerateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.Contains(".", token);   // JWT 有三個部分
    }
}