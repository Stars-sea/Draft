namespace Draft.Server.Services.Authentication;

internal interface IJwtTokenGenerator {
    AuthenticationToken Generate(string userId, string email, string username);
}
