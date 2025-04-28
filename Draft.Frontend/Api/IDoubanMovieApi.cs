using Draft.Frontend.Models;
using Refit;

namespace Draft.Frontend.Api;

public interface IDoubanMovieApi {
    [Post("/api/v0/douban-movies")]
    public Task<DoubanMovie> CreateDoubanMovie([Body] DoubanMovie movie);

    [Get("/api/v0/douban-movies")]
    public Task<List<DoubanMovie>> GetDoubanMovies();

    [Get("/api/v0/douban-movies/titles")]
    public Task<string> GetDoubanMoviesTitles();

    [Get("/api/v0/douban-movies/{id}")]
    public Task<DoubanMovie> GetDoubanMovie(string id);

    [Delete("/api/v0/douban-movies/{id}")]
    public Task DeleteDoubanMovie(string id);
}
