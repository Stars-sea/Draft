namespace Draft.Models.Dto.Movie;

public sealed record DoubanMovieSimpleResponse(
    int Id,
    string Title
) {
    public static DoubanMovieSimpleResponse Create(DoubanMovie movie)
        => new(movie.Id, movie.Title);
}

public sealed record DoubanMovieResponse(
    int? Id,
    string Title,
    ICollection<string>? OtherTitles,
    string? StaffInfos,
    string? Year,
    string? Region,
    ICollection<string>? Tags,
    float? Rating,
    int? RatingCount,
    int? Rank,
    string? Quote,
    string? Url,
    string? PreviewImage
) {
    private DoubanMovieResponse(DoubanMovie movie) : this(
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
        movie.Url,
        movie.PreviewImage
    ) {
    }

    public static DoubanMovieResponse Create(DoubanMovie movie) => new(movie);
}
