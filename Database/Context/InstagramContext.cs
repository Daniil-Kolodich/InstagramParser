using Database.Configurations;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Context;

public class InstagramContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public InstagramContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("server=localhost;database=instagram;user=instagram;password=1234");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}