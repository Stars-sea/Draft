namespace Draft.Server.Services.Authentication;

internal sealed record AuthenticationToken(
    Guid Id,
    string Username,
    string Email,
    string Token,
    DateTime Expires
);
