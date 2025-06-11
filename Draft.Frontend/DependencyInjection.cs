using Draft.Frontend.Api;

namespace Draft.Frontend;

internal static class DependencyInjection {
    public static IServiceCollection AddRefit(this IServiceCollection services, IConfiguration configuration) {
        ApiFactory factory = new(configuration["ApiUrl"]!);
        services.AddScoped(factory.Create<IDoubanMovieApi>);
        services.AddScoped(factory.Create<IAuthApi>);
        services.AddScoped(factory.Create<IProfileApi>);
        
        return services;
    }
}
