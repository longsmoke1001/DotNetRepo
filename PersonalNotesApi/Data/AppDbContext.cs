using Microsoft.EntityFrameworkCore;
using PersonalNotesApi.Models;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Note> Notes { get; set; }
}