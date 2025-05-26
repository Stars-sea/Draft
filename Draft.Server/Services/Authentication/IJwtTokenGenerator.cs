namespace Draft.Server.Services.Authentication;

public interface IJwtTokenGenerator {
    AuthenticationToken Generate(int userId, string email, string username);
}
