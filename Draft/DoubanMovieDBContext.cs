using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Draft;

public class DoubanMovieDbContext : DbContext
{
    public DbSet<DoubanMovie> Movies { get; set; }
    
    private const string Server = "localhost";
    private const string DatabaseName = "douban_movies";
    private const string User = "root";
    private const string Password = "123456";
    
    private const string ConnectionString = $"server={Server};user={User};password={Password};database={DatabaseName}";

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);
        model.Entity<DoubanMovie>().HasKey(d => d.Title);
        // model.Entity<DoubanMovie>().HasMany(d => d.Descriptions);
        // model.Entity<DoubanMovie>().HasMany(d => d.OtherTitles);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
        
        options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString))
               .LogTo(Console.WriteLine, LogLevel.Information)
               .EnableSensitiveDataLogging()
               .EnableDetailedErrors();
    }
}
