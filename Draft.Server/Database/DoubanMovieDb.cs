using System.Text.Encodings.Web;
using System.Text.Json;
using Draft.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Draft.Server.Database;

public class DoubanMovieDb(DbContextOptions options) : DbContext(options) {
    public DbSet<DoubanMovie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder model) {
        base.OnModelCreating(model);

        JsonSerializerOptions jsonSerializerOptions = new() {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        ValueConverter<ICollection<string>, string> collectionConverter = new(
            c => JsonSerializer.Serialize(c, jsonSerializerOptions),
            c => JsonSerializer.Deserialize<ICollection<string>>(c, jsonSerializerOptions) ?? new List<string>()
        );

        model.Entity<DoubanMovie>(entity => {
                entity.HasKey(m => m.Title);
                entity.Property(m => m.OtherTitles).HasConversion(collectionConverter);
                entity.Property(m => m.Tags).HasConversion(collectionConverter);
            }
        );
    }
}
