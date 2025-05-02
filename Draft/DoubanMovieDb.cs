using System.Text.Encodings.Web;
using System.Text.Json;
using Draft.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Draft;

public class DoubanMovieDb : DbContext {

    private const string Server       = "localhost";
    private const string DatabaseName = "douban_movies";
    private const string User         = "root";
    private const string Password     = "123456";

    private const string ConnectionString = $"server={Server};user={User};password={Password};database={DatabaseName}";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public DbSet<DoubanMovie> Movies { get; set; }

    public ValueConverter<ICollection<string>, string> CollectionConverter { get; init; } = new(
        c => JsonSerializer.Serialize(c, JsonSerializerOptions),
        c => JsonSerializer.Deserialize<ICollection<string>>(c, JsonSerializerOptions) ?? new List<string>()
    );

    protected override void OnModelCreating(ModelBuilder model) {
        base.OnModelCreating(model);
        model.Entity<DoubanMovie>(entity => {
                entity.HasKey(e => e.Title);

                entity.Property(m => m.OtherTitles)
                      .HasConversion(CollectionConverter).HasColumnType("json");
                entity.Property(m => m.Tags)
                      .HasConversion(CollectionConverter).HasColumnType("json");
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        base.OnConfiguring(options);

        options.UseMySQL(ConnectionString)
               .EnableSensitiveDataLogging();
    }
}
