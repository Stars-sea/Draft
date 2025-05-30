using Microsoft.AspNetCore.Identity;

namespace Draft.Server.Services.Authentication;

internal record AuthenticationResult : ResultBase<AuthenticationToken> {

    public static AuthenticationResult Success(AuthenticationToken? token)
        => new() {
            IsSuccess = true,
            Errors    = [],
            Content   = token
        };

    public static AuthenticationResult Failed(params IEnumerable<IdentityError> errors)
        => new() {
            IsSuccess = false,
            Errors    = errors.Select(x => (ErrorMessage)x).ToArray()
        };
}
