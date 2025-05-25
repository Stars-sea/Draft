namespace Draft.Server.Services;

public interface IJwtTokenGenerator {
    string Generate(int userId, string nickname);
}
