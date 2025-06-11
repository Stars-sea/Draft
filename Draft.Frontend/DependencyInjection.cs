using Draft.Frontend.Services;
using Draft.Frontend.Services.Api;
using Draft.Frontend.Services.Impl;

namespace Draft.Frontend;

internal static class DependencyInjection {
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration) {
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
        return services;
    }
    
    public static IServiceCollection AddRefit(this IServiceCollection services) {
        services.AddScoped<IAuthTokenProvider, AuthTokenProvider>();
        
        services.AddScoped(ApiFactory.Create<IDoubanMovieApi>);
        services.AddScoped(ApiFactory.Create<IAuthApi>);
        services.AddScoped(ApiFactory.Create<IProfileApi>);
        
        return services;
    }
}
