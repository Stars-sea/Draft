using Draft.Models;
using Draft.Server.Services.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Draft.Server.Services.Impl;

public class AuthenticationService(
    UserManager<UserProfile> userManager,
    SignInManager<UserProfile> signInManager,
    IJwtTokenGenerator tokenGenerator
) : IAuthenticationService {

    public async Task<AuthenticationResult> RegisterAsync(string email, string username, string password) {
        IdentityResult result = await userManager.CreateAsync(
            new UserProfile {
                Email    = email,
                UserName = username
            },
            password
        );

        return !result.Succeeded
            ? AuthenticationResult.Failed(result.Errors.ToArray())
            : AuthenticationResult.Success(null);
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password) {
        UserProfile? profile = await userManager.FindByEmailAsync(email);
        if (profile == null)
            return AuthenticationResult.Failed(
                new IdentityError {
                    Code        = "InvalidEmailOrPassword",
                    Description = "Invalid email or password."
                }
            );

        SignInResult result = await signInManager.CheckPasswordSignInAsync(profile, password, false);
        if (!result.Succeeded || profile.Email == null || profile.UserName == null)
            return AuthenticationResult.Failed(
                new IdentityError {
                    Code        = "InvalidEmailOrPassword",
                    Description = "Invalid email or password."
                }
            );

        return AuthenticationResult.Success(tokenGenerator.Generate(profile.Id, profile.Email, profile.UserName));
    }
}
