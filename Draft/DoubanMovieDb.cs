using Draft.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Draft;

public class DoubanMovieDb : DbContext {

    private const string Server       = "localhost";
    private const string DatabaseName = "douban_movies";
    private const string User         = "root";
    private const string Password     = "123456";

    private const string ConnectionString = $"server={Server};user={User};password={Password};database={DatabaseName}";

    public DbSet<DoubanMovie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder model) {
        base.OnModelCreating(model);
        model.Entity<DoubanMovie>().HasKey(d => d.Title);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        base.OnConfiguring(options);

        options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString))
               .LogTo(Console.WriteLine, LogLevel.Information)
               .EnableSensitiveDataLogging()
               .EnableDetailedErrors();
    }
}
