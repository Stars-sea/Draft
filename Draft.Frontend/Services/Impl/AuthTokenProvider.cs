using Blazored.LocalStorage;
using Draft.Models.Dto.Authentication;
using Microsoft.Extensions.Options;

namespace Draft.Frontend.Services.Impl;

internal class AuthTokenProvider(
    IOptions<ApiSettings> settings,
    ILocalStorageService localStorage
) : IAuthTokenProvider {
    
    private ApiSettings Settings => settings.Value;

    public async Task<string> GetTokenAsync(CancellationToken cancellationToken = default) {
        AuthenticationResponse? auth = await localStorage.GetItemAsync<AuthenticationResponse>(Settings.TokenStorageKey, cancellationToken);
        if (auth != null && auth.Expires > DateTime.Now) return auth.Token;
        
        await localStorage.RemoveItemAsync(Settings.TokenStorageKey, cancellationToken);
        return "";
    }

    public async Task<bool> IsTokenStored(CancellationToken cancellationToken = default) 
        => await localStorage.GetItemAsync<AuthenticationResponse>(Settings.TokenStorageKey, cancellationToken) != null;
}
