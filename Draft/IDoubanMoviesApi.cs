using Draft.Models.Dto.Movie;
using Refit;

namespace Draft;

public interface IDoubanMoviesApi {
    const string JwtToken =

    [Put("/api/v1/douban-movies")]
    [Headers($"Authorization: Bearer {JwtToken}")]
    public Task<DoubanMovieSimpleResponse> PutDoubanMovie([Body] DoubanMovieModifyRequest movie);
}
