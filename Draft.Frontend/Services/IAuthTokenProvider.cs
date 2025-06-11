namespace Draft.Frontend.Services;

public interface IAuthTokenProvider {
    Task<string> GetTokenAsync(CancellationToken cancellationToken = default);
    
    Task<bool> IsTokenStored(CancellationToken cancellationToken = default);
}
