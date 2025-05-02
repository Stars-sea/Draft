namespace Draft.Frontend.Models;

public record DoubanMovie(
    int Rank,
    string Title,
    ICollection<string> OtherTitles,
    string StaffInfos,
    string Year,
    string Region,
    ICollection<string> Tags,
    float Rating,
    int RatingCount,
    string? Quote,
    string Url,
    string PreviewImage
);
