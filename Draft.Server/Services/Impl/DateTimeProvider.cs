namespace Draft.Server.Services.Impl;

internal class DateTimeProvider : IDateTimeProvider {
    public DateTime UtcNow => DateTime.UtcNow;
}
