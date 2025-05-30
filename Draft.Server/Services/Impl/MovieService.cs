using Draft.Models;
using Draft.Server.Database;
using Draft.Server.Services.Movie;
using Microsoft.EntityFrameworkCore;

namespace Draft.Server.Services.Impl;

internal class MovieService(ApplicationDb database) : IMovieService {

    private readonly DbSet<DoubanMovie> _movies = database.Movies;

    public MovieQueryResults GetMovies()
        => MovieQueryResults.Success(_movies.OrderBy(m => m.Rank));

    public async Task<MovieQueryResults> SearchMovieAsync(string searchTerm) {
        var movies =
            await _movies.Where(m => m.Title.Contains(searchTerm) ||
                                     m.OtherTitles.Any(t => t.Contains(searchTerm)) ||
                                     m.Tags.Any(t => t.Contains(searchTerm)) ||
                                     m.Year.ToString().Contains(searchTerm) ||
                                     m.StaffInfos.Contains(searchTerm) ||
                                     m.Region.Contains(searchTerm)
            ).ToArrayAsync();

        return MovieQueryResults.Success(movies);
    }

    public async Task<MovieQueryResults> FindMovieByIdAsync(int id)
        => await _movies.FindAsync(id) switch {
            { } movie => MovieQueryResults.Success(movie),
            null      => MovieQueryResults.Failed(("NotFound", "Movie not found"))
        };

    public async Task<MovieOperationResult> DeleteMovieAsync(int id) {
        DoubanMovie? entry = await _movies.FindAsync(id);
        if (entry == null) return MovieOperationResult.Failed(("NotFound", "Movie not found"));

        _movies.Remove(entry);
        await database.SaveChangesAsync();
        return MovieOperationResult.Success(null);
    }

    public async Task<MovieOperationResult> CreateMovieAsync(DoubanMovie movie) {
        if (await _movies.AnyAsync(m => string.Equals(m.Title, movie.Title.Trim())))
            return MovieOperationResult.Failed(("InvalidTitle", "Movie already exists"));

        var entry = await _movies.AddAsync(movie);
        await database.SaveChangesAsync();
        return MovieOperationResult.Success(entry.Entity);
    }

    public async Task<MovieOperationResult> UpdateMovieAsync(DoubanMovie movie) {
        if (await _movies.FindAsync(movie.Id) == null)
            return MovieOperationResult.Failed(("NotFound", "Movie not found"));

        var entry = _movies.Update(movie);
        await database.SaveChangesAsync();
        return MovieOperationResult.Success(entry.Entity);
    }
}
