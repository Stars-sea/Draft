using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Draft.Models;
using Draft.Models.Dto.Movie;
using Draft.Server.Database;
using Draft.Server.Extensions;
using Draft.Server.Services.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Draft.Server.Controllers;

[ApiController]
[Route("api/v1/profile")]
public class ProfileController(
    UserManager<UserProfile> userManager,
    IMovieService movieService,
    ApplicationDb database
) : ControllerBase {

    [NonAction]
    private async Task<UserProfile?> GetUserFromTokenAsync() {
        string? username = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
        return username == null ? null : await userManager.FindByNameAsync(username);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<DoubanMovieSimpleResponse>> GetUser(string username) {
        UserProfile? user = await userManager.FindByNameAsync(username);
        if (user == null) return NotFound();

        return User.HasClaim(JwtRegisteredClaimNames.Name, user.UserName!)
            ? Ok(user.ToDetailedResponse())
            : Ok(user.ToResponse());
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser() {
        UserProfile? profile = await GetUserFromTokenAsync();
        if (profile == null) return Unauthorized();
        return Ok(profile.ToDetailedResponse());
    }

    // TODO: impl favorites service

    [HttpGet("favorites/{movieId:int}")]
    [Authorize]
    public async Task<ActionResult<Favorite>> GetFavorites(int movieId) {
        UserProfile? user = await GetUserFromTokenAsync();
        if (user == null) return NotFound();

        Favorite? favorite = user.Favorites.FirstOrDefault(f => f.MovieId == movieId);
        return favorite != null ? Ok(favorite) : NotFound();
    }
    
    [HttpPost("favorites/{movieId:int}")]
    [Authorize]
    public async Task<IActionResult> AddFavorite(int movieId) {
        UserProfile? user = await GetUserFromTokenAsync();
        if (user == null) return NotFound();
        
        if (user.Favorites.Any(f => f.MovieId == movieId))
            return Conflict();

        MovieQueryResults movieResult = await movieService.FindMovieByIdAsync(movieId);
        if (!movieResult.IsSuccess) return NotFound();

        Favorite favorite = new() {
            UserId    = user.Id,
            MovieId   = movieId,
            CreatedAt = DateTime.UtcNow,
            User      = user,
            Movie     = movieResult.Content!.First()
        };

        database.Favorites.Add(favorite);
        await database.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("favorites/{movieId:int}")]
    [Authorize]
    public async Task<IActionResult> RemoveFavorite(int movieId) {
        UserProfile? user = await GetUserFromTokenAsync();
        if (user == null) return NotFound();

        Favorite? favorite =
            await database.Favorites.FirstOrDefaultAsync(f => f.UserId == user.Id && f.MovieId == movieId);

        if (favorite == null) return NotFound();

        database.Favorites.Remove(favorite);
        await database.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("favorites")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<DoubanMovieSimpleResponse>>> GetFavorites() {
        UserProfile? user = await GetUserFromTokenAsync();
        if (user == null) return NotFound();

        var favorites =
            await database.Favorites
                          .Where(f => f.UserId == user.Id)
                          .Include(f => f.Movie)
                          .ToListAsync();

        return Ok(favorites.Select(f => f.Movie.ToSimpleResponse()));
    }
}
