using Draft.Models;
using Draft.Server.Database;
using Draft.Server.Services.Movie;
using Microsoft.EntityFrameworkCore;

namespace Draft.Server.Services.Impl;

internal class MovieService(ApplicationDb database) : IMovieService {

    private DbSet<DoubanMovie> Movies => database.Movies;

    public MovieQueryResults GetMovies()
        => MovieQueryResults.Success(Movies.OrderBy(m => m.Rank));

    public async Task<MovieQueryResults> SearchMovieAsync(string searchTerm) {
        var movies =
            await Movies.Where(m => m.Title.Contains(searchTerm) ||
                                    m.OtherTitles.Any(t => t.Contains(searchTerm)) ||
                                    m.Tags.Any(t => t.Contains(searchTerm)) ||
                                    m.Year.ToString().Contains(searchTerm) ||
                                    m.StaffInfos.Contains(searchTerm) ||
                                    m.Region.Contains(searchTerm)
            ).ToArrayAsync();

        return MovieQueryResults.Success(movies);
    }

    public async Task<MovieQueryResults> FindMovieByIdAsync(int id)
        => await Movies.FindAsync(id) switch {
            { } movie => MovieQueryResults.Success(movie),
            null      => MovieQueryResults.Failed(("NotFound", "Movie not found"))
        };

    public async Task<MovieOperationResult> DeleteMovieAsync(int id) {
        DoubanMovie? entry = await Movies.FindAsync(id);
        if (entry == null) return MovieOperationResult.Failed(("NotFound", "Movie not found"));

        Movies.Remove(entry);
        await database.SaveChangesAsync();
        return MovieOperationResult.Success(null);
    }

    public async Task<MovieOperationResult> CreateMovieAsync(DoubanMovie movie) {
        if (await Movies.AnyAsync(m => string.Equals(m.Title, movie.Title.Trim())))
            return MovieOperationResult.Failed(("InvalidTitle", "Movie already exists"));

        var entry = await Movies.AddAsync(movie);
        await database.SaveChangesAsync();
        return MovieOperationResult.Success(entry.Entity);
    }

    public async Task<MovieOperationResult> UpdateMovieAsync(DoubanMovie movie) {
        if (await Movies.FindAsync(movie.Id) == null)
            return MovieOperationResult.Failed(("NotFound", "Movie not found"));

        var entry = Movies.Update(movie);
        await database.SaveChangesAsync();
        return MovieOperationResult.Success(entry.Entity);
    }
}
