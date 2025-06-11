using Microsoft.Extensions.Options;
using Refit;

namespace Draft.Frontend.Services.Api;

public static class ApiFactory {
    public static T Create<T>(IServiceProvider services) where T : class {
        ApiSettings apiSettings = services.GetService<IOptions<ApiSettings>>()!.Value;
        IAuthTokenProvider authTokenProvider = services.GetService<IAuthTokenProvider>()!;

        RefitSettings settings = new() {
            AuthorizationHeaderValueGetter = async (_, token) => await authTokenProvider.GetTokenAsync(token)
        };
        return RestService.For<T>(apiSettings.BaseUrl, settings);
    }
}
