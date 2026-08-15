using Microsoft.EntityFrameworkCore;
using GameApi.Data;
using GameApi.Middlewares;
var builder = WebApplication.CreateBuilder(args);

// 加入 Controller 服務
builder.Services.AddControllers();

// 加入 Swagger 服務（代替 OpenAPI）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=gameapi.db"));

var app = builder.Build();

// 啟用 Swagger（任何環境都著，方便你學習）
app.UseSwagger();
app.UseSwaggerUI();

// 如果你仲想保留 HTTPS 轉址
app.UseHttpsRedirection();

// 將 Controller 嘅路由加入應用程式
app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();  // 如果 Database 唔存在，就建立佢
}

app.Run();