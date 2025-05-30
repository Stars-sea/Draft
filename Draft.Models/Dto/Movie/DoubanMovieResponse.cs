namespace Draft.Models.Dto.Movie;

public sealed record DoubanMovieSimpleResponse(
    int Id,
    string Title
);

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
);
