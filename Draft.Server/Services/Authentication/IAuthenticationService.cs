namespace Draft.Server.Services.Authentication;

public interface IAuthenticationService {
    Task<AuthenticationResult> RegisterAsync(string email, string username, string password);

    Task<AuthenticationResult> LoginAsync(string email, string password);
}
