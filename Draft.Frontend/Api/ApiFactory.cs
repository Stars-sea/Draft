using Blazored.LocalStorage;
using Draft.Models.Dto.Authentication;
using Refit;

namespace Draft.Frontend.Api;

public sealed class ApiFactory(string baseUrl) {
    public const string TokenStorageKey = "auth_token";
    
    private static async Task<string> GetTokenFromStorageAsync(ILocalStorageService storage, CancellationToken token) {
        AuthenticationResponse? auth = await storage.GetItemAsync<AuthenticationResponse>(TokenStorageKey, token);
        if (auth != null && auth.Expires > DateTime.Now) return auth.Token;
        
        await storage.RemoveItemAsync(TokenStorageKey, token);
        return "";
    }
    
    public T Create<T>(IServiceProvider services) where T : class {
        ILocalStorageService storageService = services.GetRequiredService<ILocalStorageService>();

        RefitSettings settings = new() {
            AuthorizationHeaderValueGetter = async (_, token) => await GetTokenFromStorageAsync(storageService, token)
        };
        return RestService.For<T>(baseUrl, settings);
    }
}
