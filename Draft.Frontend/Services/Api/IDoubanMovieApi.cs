using Draft.Models;
using Draft.Models.Dto.Movie;
using Refit;

namespace Draft.Frontend.Services.Api;

[Headers("Authorization: Bearer")]
public interface IDoubanMovieApi {
    [Put("/api/v1/douban-movies")]
    Task<DoubanMovie> PutDoubanMovie([Body] DoubanMovie movie);

    [Get("/api/v1/douban-movies")]
    Task<List<DoubanMovieResponse>> GetDoubanMovies();

    [Get("/api/v1/douban-movies/simple")]
    Task<List<DoubanMovieSimpleResponse>> GetDoubanMoviesSimple();

    [Get("/api/v1/douban-movies/{id}")]
    Task<DoubanMovieResponse> GetDoubanMovie(int id);

    [Delete("/api/v1/douban-movies/{id}")]
    Task DeleteDoubanMovie(int id);
}
