using Draft.Models;
using Draft.Server.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Draft.Server.Controllers;

[ApiController]
[Route("api/v1/douban-movies")]
public class DoubanMoviesController : ControllerBase {

    // TODO: wrap with service
    private readonly ApplicationDb _database;

    public DoubanMoviesController(ApplicationDb database) {
        _database = database;
        _database.Database.EnsureCreated();
    }

    [HttpPut]
    public async Task<IResult> PutMovieEntry(DoubanMovie movie) {
        if (await _database.Movies.AnyAsync(m => string.Equals(m.Title, movie.Title)))
            return Results.Conflict("Movie already exists");

        await _database.Movies.AddAsync(movie);
        await _database.SaveChangesAsync();

        return Results.Created($"/{movie.Id}", movie);
    }

    [HttpGet]
    public async Task<IResult> GetMovieEntries() =>
        Results.Ok(await _database.Movies.ToListAsync());

    [HttpGet("simple")]
    public async Task<IResult> GetMovieSimple() {
        return Results.Ok(
            await _database.Movies
                           .OrderBy(m => m.Rank)
                           .Select(m => new DoubanMovieSimple { Id = m.Id, Title = m.Title })
                           .ToListAsync()
        );
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetMovieEntry(int id) =>
        await _database.Movies.FindAsync(id) is { } movie ? Results.Ok(movie) : Results.NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteMovieEntry(int id) {
        DoubanMovie? movie = await _database.Movies.FindAsync(id);
        if (movie == null) return Results.NotFound();

        _database.Movies.Remove(movie);
        await _database.SaveChangesAsync();
        return Results.NoContent();
    }
}
