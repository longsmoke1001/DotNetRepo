using Microsoft.EntityFrameworkCore;
using PersonalNotesApi.Models;
namespace PersonalNotesApi.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Note> Notes { get; set; }
    public DbSet<User> Users { get; set; }
}