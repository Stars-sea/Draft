namespace Draft.Server.Services;

public interface IJwtTokenGenerator {
    (string, DateTime) Generate(int userId, string nickname);
}
