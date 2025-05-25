using Draft.Server.Database;
using Draft.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Draft.Server.Controllers;


[ApiController]
[Route("api/v1/douban-movies")]
public class DoubanMoviesController(DoubanMovieDb database) : ControllerBase {
    
    [HttpPut]
    public async Task<IResult> PutMovieEntry(DoubanMovie movie) {
        if (await database.Movies.AnyAsync(m => string.Equals(m.Title, movie.Title)))
            return Results.Conflict("Movie already exists");

        await database.Movies.AddAsync(movie);
        await database.SaveChangesAsync();

        return Results.Created($"/{movie.Title}", movie);
    }

    [HttpGet]
    public async Task<IResult> GetMovieEntries() =>
        Results.Ok(await database.Movies.ToListAsync());

    [HttpGet("titles")]
    public async Task<IResult> GetMovieTitles() {
        return Results.Ok(
            await database.Movies
                          .OrderBy(m => m.Rank)
                          .Select(m => m.Title)
                          .ToListAsync()
        );
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetMovieEntry(string id) =>
        await database.Movies.FindAsync(id) is { } movie ? Results.Ok(movie) : Results.NotFound();

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteMovieEntry(string id) {
        DoubanMovie? movie = await database.Movies.FindAsync(id);
        if (movie == null) return Results.NotFound();

        database.Movies.Remove(movie);
        await database.SaveChangesAsync();
        return Results.NoContent();
    }
}
