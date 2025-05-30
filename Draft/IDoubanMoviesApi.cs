using Draft.Models;
using Draft.Models.Dto.Movie;
using Refit;

namespace Draft;

public interface IDoubanMoviesApi {
    const string JwtToken =

    [Put("/api/v1/douban-movies")]
    [Headers($"Authorization: Bearer {JwtToken}")]
    public Task<DoubanMovie> PutDoubanMovie([Body] DoubanMovieModifyRequest movie);
}
