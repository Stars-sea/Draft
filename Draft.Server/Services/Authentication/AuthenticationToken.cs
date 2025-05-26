namespace Draft.Server.Services.Authentication;

public sealed record AuthenticationToken(
    Guid Id,
    string Username,
    string Email,
    string Token,
    DateTime Expires
);
