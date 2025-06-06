using System.IdentityModel.Tokens.Jwt;
using Draft.Models;
using Draft.Server.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Draft.Server.Controllers;

[ApiController]
[Route("api/v1/profile")]
public class ProfileController(UserManager<UserProfile> userManager) : ControllerBase {
    [HttpGet("{username}")]
    public async Task<IActionResult> GetUser(string username) {
        UserProfile? user = await userManager.FindByNameAsync(username);
        if (user == null) return NotFound();

        return User.HasClaim(JwtRegisteredClaimNames.Name, user.UserName!)
            ? Ok(user.ToDetailedResponse())
            : Ok(user.ToResponse());
    }
}
