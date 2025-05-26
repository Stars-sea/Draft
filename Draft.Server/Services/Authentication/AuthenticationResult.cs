using Microsoft.AspNetCore.Identity;

namespace Draft.Server.Services.Authentication;

public class AuthenticationResult {
    public AuthenticationToken? Token { get; init; }

    public required bool Succeeded { get; init; }

    public IEnumerable<IdentityError>? Errors { get; init; } = [];

    public static AuthenticationResult Success(AuthenticationToken? token)
        => new() {
            Token     = token,
            Succeeded = true
        };

    public static AuthenticationResult Failed(params IdentityError[] errors)
        => new() {
            Succeeded = false,
            Errors    = errors
        };
}
