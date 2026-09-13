using System.Text;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalNotesApi.Services;
using PersonalNotesApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Configure services and cross-origin requests.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Personal Notes API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the token in the format: Bearer {your-token}"
    });

    // Add Bearer authentication to Swagger UI.
    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=personalnotes.db"));
builder.Services.AddScoped<IAuthService, AuthService>();

// Configure JWT authentication and token validation.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "PersonalNotesApi",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "PersonalNotesApiUsers",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]
                    ?? "your-super-secret-key-at-least-32-chars-long"))
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();


// Ensure the SQLite database exists before the application starts.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    // if (!db.Notes.Any())
    // {
    //     Console.WriteLine("✅ 正在加入 100 筆測試數據...");

    //     var categories = new[] { "一般", "日記", "密碼" };
    //     var random = new Random();

    //     for (int i = 0; i < 100; i++)
    //     {
    //         var note = new Note
    //         {
    //             Title = $"測試備忘錄 {i + 1}",
    //             Content = $"呢個係第 {i + 1} 個測試備忘錄嘅內容。",
    //             Category = (NoteCategory)random.Next(0, 3),
    //             CreatedAt = DateTime.Now.AddDays(-random.Next(0, 30))
    //         };
    //         db.Notes.Add(note);
    //     }
    //     db.SaveChanges();
    //     Console.WriteLine("✅ 成功加入 100 筆測試數據！");
    // }
    // else
    // {
    //     Console.WriteLine($"ℹ️ Database 已有數據，跳過 Seed。現有數據量：{db.Notes.Count()}");
    // }
}
app.Run();