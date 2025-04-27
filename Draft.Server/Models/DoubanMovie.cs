namespace Draft.Server.Models;

public record DoubanMovie(
    string Title,
    ICollection<string> OtherTitles,
    ICollection<string> Descriptions,
    float Rating,
    int RatingCount,
    string? Quote,
    string Url,
    string PreviewImage
);
