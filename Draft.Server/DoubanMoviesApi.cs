using Draft.Server.Database;
using Draft.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Draft.Server;

public static class DoubanMoviesApi {
    private static async Task<IResult> CreateMovieEntry(DoubanMovie movie, DoubanMovieDb database) {
        if (await database.Movies.AnyAsync(m => string.Equals(m.Title, movie.Title)))
            return Results.Conflict("Movie already exists");

        await database.Movies.AddAsync(movie);
        await database.SaveChangesAsync();

        return Results.Created($"/{movie.Title}", movie);
    }

    private static async Task<IResult> GetMovieEntries(DoubanMovieDb database) =>
        Results.Ok(await database.Movies.ToListAsync());

    private static async Task<IResult> GetMovieTitles(DoubanMovieDb database) {
        return Results.Ok(
            await database.Movies
                          .OrderBy(m => m.Rank)
                          .Select(m => m.Title)
                          .ToListAsync()
        );
    }

    private static async Task<IResult> GetMovieEntry(string id, DoubanMovieDb database) =>
        await database.Movies.FindAsync(id) is { } movie ? Results.Ok(movie) : Results.NotFound();

    private static async Task<IResult> DeleteMovieEntry(string id, DoubanMovieDb database) {
        DoubanMovie? movie = await database.Movies.FindAsync(id);
        if (movie == null) return Results.NotFound();

        database.Movies.Remove(movie);
        await database.SaveChangesAsync();
        return Results.NoContent();
    }

    public static RouteGroupBuilder MapDoubanMoviesApi(this RouteGroupBuilder group) {
        group.MapPost("/", CreateMovieEntry)
             .WithSummary("Creates a new movie entry");
        group.MapGet("/", GetMovieEntries)
             .WithSummary("Gets all movie entries");

        group.MapGet("/titles", GetMovieTitles)
             .WithSummary("Gets all movie titles");

        group.MapGet("/{id}", GetMovieEntry)
             .WithSummary("Gets movie entry with specified id");
        group.MapDelete("/{id}", DeleteMovieEntry)
             .WithSummary("Deletes movie entry with specified id");
        return group;
    }
}
