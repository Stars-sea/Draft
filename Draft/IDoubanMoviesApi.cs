using Draft.Models;
using Refit;

namespace Draft;

public interface IDoubanMoviesApi {
    [Put("/api/v1/douban-movies")]
    public Task<DoubanMovie> PutDoubanMovie([Body] DoubanMovie movie);
}
