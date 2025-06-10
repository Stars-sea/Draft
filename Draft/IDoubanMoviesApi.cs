using Draft.Models.Dto.Movie;
using Refit;

namespace Draft;

public interface IDoubanMoviesApi {
    const string JwtToken =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyY2NjM2FjNC1jNzRlLTQzOWUtYTM5OC0wODEwOGUxODJjOTQiLCJuYW1lIjoiU3RhcnNfc2VhIiwiZW1haWwiOiJTdGFyc19zZWFAb3V0bG9vay5jb20iLCJqdGkiOiIzYTQ4NWUwYi04MmFhLTQ5YTAtOTRmZi0yYzgzNTM0ZjUxNWUiLCJleHAiOjE3NTAwNTA0OTEsImlzcyI6IlN0YXJzLXNlYSIsImF1ZCI6IlN0YXJzLXNlYSJ9.oOWSSLX-y2Bco180b-gHAArF6PyrHCCh4TAZApIFlhk";

    [Put("/api/v1/douban-movies")]
    [Headers($"Authorization: Bearer {JwtToken}")]
    public Task<DoubanMovieSimpleResponse> PutDoubanMovie([Body] DoubanMovieModifyRequest movie);
}
