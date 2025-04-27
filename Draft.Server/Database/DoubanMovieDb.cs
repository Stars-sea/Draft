using Draft.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Draft.Server.Database;

public class DoubanMovieDb(DbContextOptions options) : DbContext(options) {
    public DbSet<DoubanMovie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder model) {
        base.OnModelCreating(model);
        model.Entity<DoubanMovie>().HasKey(d => d.Title);
    }
}
