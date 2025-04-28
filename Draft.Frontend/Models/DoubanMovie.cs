namespace Draft.Frontend.Models;

public record DoubanMovie(
    string Title,
    List<string> OtherTitles,
    List<string> Descriptions,
    float Rating,
    int RatingCount,
    string? Quote,
    string Url,
    string PreviewImage
);
