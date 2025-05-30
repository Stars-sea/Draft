using Draft.Models;
using Draft.Models.Dto.Movie;
using Refit;

namespace Draft.Frontend.Api;

public interface IDoubanMovieApi {
    [Put("/api/v1/douban-movies")]
    public Task<DoubanMovie> PutDoubanMovie([Body] DoubanMovie movie);

    [Get("/api/v1/douban-movies")]
    public Task<List<DoubanMovieResponse>> GetDoubanMovies();

    [Get("/api/v1/douban-movies/simple")]
    public Task<List<DoubanMovieSimpleResponse>> GetDoubanMoviesSimple();

    [Get("/api/v1/douban-movies/{id}")]
    public Task<DoubanMovie> GetDoubanMovie(int id);

    [Delete("/api/v1/douban-movies/{id}")]
    public Task DeleteDoubanMovie(int id);
}
