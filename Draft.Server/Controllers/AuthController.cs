using Draft.Models.Dto.Authentication;
using Draft.Server.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Draft.Server.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IAuthenticationService authenticationService) : ControllerBase {

    [HttpPost("login")]
    public async Task<IActionResult> PostLogin([FromBody] LoginRequest loginRequest) {
        AuthenticationResult result = await authenticationService.LoginAsync(
            loginRequest.Email,
            loginRequest.Password
        );

        return result.IsSuccess ? Ok(result.Content) : Unauthorized(result.Errors);
    }

    [HttpPost("register")]
    public async Task<IActionResult> PostRegister([FromBody] RegisterRequest registerRequest) {
        AuthenticationResult result = await authenticationService.RegisterAsync(
            registerRequest.Email,
            registerRequest.Username,
            registerRequest.Password
        );

        return result.IsSuccess
            ? RedirectToAction(
                "GetUser",
                nameof(ProfileController)[..^10],
                new { registerRequest.Username }
            )
            : BadRequest(result.Errors);
    }
}
