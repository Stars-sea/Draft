using System.Text.Encodings.Web;
using System.Text.Json;
using Draft.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Draft.Server.Database;

internal class ApplicationDb(DbContextOptions options) : IdentityDbContext<UserProfile>(options) {

    public DbSet<UserProfile> Profiles { get; set; }

    public DbSet<DoubanMovie> Movies { get; set; }

    public DbSet<Favorite> Favorites { get; set; }

    protected override void OnModelCreating(ModelBuilder model) {
        base.OnModelCreating(model);

        model.Entity<Favorite>().HasKey(f => new { f.UserId, f.MovieId });
        model.Entity<Favorite>()
             .HasOne(f => f.User)
             .WithMany(u => u.Favorites)
             .HasForeignKey(f => f.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        model.Entity<Favorite>()
             .HasOne(f => f.Movie)
             .WithMany(m => m.Favorites)
             .HasForeignKey(f => f.MovieId)
             .OnDelete(DeleteBehavior.Cascade);

        JsonSerializerOptions jsonSerializerOptions = new() {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        ValueConverter<ICollection<string>, string> collectionConverter = new(
            c => JsonSerializer.Serialize(c, jsonSerializerOptions),
            c => JsonSerializer.Deserialize<ICollection<string>>(c, jsonSerializerOptions) ?? new List<string>()
        );

        model.Entity<DoubanMovie>(entity => {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.OtherTitles).HasConversion(collectionConverter);
                entity.Property(m => m.Tags).HasConversion(collectionConverter);
            }
        );
    }
}
