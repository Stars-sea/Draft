namespace Draft.Server.Services;

public interface IDateTimeProvider {
    DateTime UtcNow { get; }
}
