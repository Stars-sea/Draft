namespace Draft.Models.Identity;

public sealed record LoginRequest(UserIdentity Identity, string Password);
