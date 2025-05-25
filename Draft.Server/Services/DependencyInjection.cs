using System.Text;
using Draft.Server.Database;
using Draft.Server.Services.Impl;
using Draft.Server.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Draft.Server.Services;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services.AddDatabase(configuration)
                   .AddSingleton<IDateTimeProvider, DateTimeProvider>()
                   .AddSingleton<IRandomProvider, RandomProvider>()
                   .AddAuth(configuration);

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<DoubanMovieDb>(database => {
                string doubanMoviesConnectionString = configuration.GetConnectionString("APPLICATION_DATABASE")!;
                database.UseMySQL(doubanMoviesConnectionString);
            }
        );

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        // services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));
        JwtSettings settings = new();
        configuration.Bind(JwtSettings.Section, settings);
        services.AddSingleton(Options.Create(settings));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = settings.ValidIssuer,
                ValidAudience            = settings.ValidAudience,
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret!))
            }
        );

        return services;
    }
}
