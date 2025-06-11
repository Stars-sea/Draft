using Draft.Models;
using Draft.Models.Dto.Movie;

namespace Draft.Server.Extensions;

internal static class DoubanMovieExtension {
    public static DoubanMovieResponse ToResponse(this DoubanMovie movie)
        => new(
            movie.Id,
            movie.Title,
            movie.OtherTitles,
            movie.StaffInfos,
            movie.Year,
            movie.Region,
            movie.Tags,
            movie.Rating,
            movie.RatingCount,
            movie.Rank,
            movie.Quote,
            movie.Url
        );

    public static DoubanMovieSimpleResponse ToSimpleResponse(this DoubanMovie movie)
        => new(movie.Id, movie.Title);
}
