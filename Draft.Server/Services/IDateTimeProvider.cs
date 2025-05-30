namespace Draft.Server.Services;

internal interface IDateTimeProvider {
    DateTime UtcNow { get; }
}
