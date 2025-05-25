using Draft.Models;
using Draft.Models.Identity;
using Draft.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Draft.Server.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IUserProfileManager profileManager, IJwtTokenGenerator tokenGenerator) : ControllerBase {
    [NonAction]
    private TokenResponse GetTokenResponse(UserProfile userProfile) {
        (string token, DateTime expires) = tokenGenerator.Generate(userProfile.Id, userProfile.Nickname);
        return new TokenResponse(expires, token);
    }
    
    [HttpPost("login")]
    public async Task<IResult> PostLogin([FromBody] LoginRequest loginRequest) {
        UserProfile? profile = await profileManager.FindUserAsync(loginRequest.Identity);
        return profile == null
            ? Results.BadRequest("Incorrect username or password")
            : Results.Ok(GetTokenResponse(profile));
    }

    [HttpPut("register")]
    public async Task<IResult> PostRegister([FromBody] RegisterRequest registerRequest) {
        if (!registerRequest.IsPasswordValid())
            return Results.BadRequest("Invalid password");
        
        if (await profileManager.IsEmailExistsAsync(registerRequest.Email))
            return Results.BadRequest("Email already exists");

        if (await profileManager.IsNicknameExistsAsync(registerRequest.Nickname))
            return Results.BadRequest("Nickname already exists");
        
        (string nickname, string email, string password) = registerRequest;
        return await profileManager.RegisterUserAsync(email, nickname, password) switch {
            var id and > 0 => Results.Redirect($"api/v1/auth/profile/{id}"),
            _ => Results.BadRequest("Failed to register user")
        };
    }

    [HttpGet("profile/{id:int}")]
    public async Task<IResult> GetProfile(int id) {
        UserProfile? profile = await profileManager.FindUserAsync(id);
        return profile == null ? Results.NotFound() : Results.Ok(profile);
    }
}
