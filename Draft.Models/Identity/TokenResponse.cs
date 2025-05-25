namespace Draft.Models.Identity;

public sealed record TokenResponse(DateTime Expires, string Token);
