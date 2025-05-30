using Draft.Models;

namespace Draft.Server.Services.Movie;

internal interface IMovieService {
    MovieQueryResults GetMovies();

    Task<MovieQueryResults> SearchMovieAsync(string searchTerm);

    Task<MovieQueryResults> FindMovieByIdAsync(int id);

    Task<MovieOperationResult> CreateMovieAsync(DoubanMovie movie);

    Task<MovieOperationResult> UpdateMovieAsync(DoubanMovie movie);

    Task<MovieOperationResult> DeleteMovieAsync(int id);
}
