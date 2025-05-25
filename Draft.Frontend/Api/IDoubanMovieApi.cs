using Draft.Frontend.Models;
using Refit;

namespace Draft.Frontend.Api;

public interface IDoubanMovieApi {
    [Put("/api/v1/douban-movies")]
    public Task<DoubanMovie> PutDoubanMovie([Body] DoubanMovie movie);

    [Get("/api/v1/douban-movies")]
    public Task<List<DoubanMovie>> GetDoubanMovies();

    [Get("/api/v1/douban-movies/titles")]
    public Task<string> GetDoubanMoviesTitles();

    [Get("/api/v1/douban-movies/{id}")]
    public Task<DoubanMovie> GetDoubanMovie(string id);

    [Delete("/api/v1/douban-movies/{id}")]
    public Task DeleteDoubanMovie(string id);
}
