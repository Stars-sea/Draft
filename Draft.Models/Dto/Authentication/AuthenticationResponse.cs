namespace Draft.Models.Dto.Authentication;

public sealed record AuthenticationResponse(
    Guid Id,
    string Username,
    string Email,
    DateTime Expires,
    string Token
);
