namespace Draft.Models;

public class DoubanMovie : DoubanMovieSimple {
    // public int Id { get; set; }

    public int Rank { get; init; }

    // public required string Title { get; set; }

    public required ICollection<string> OtherTitles { get; set; }

    public required string StaffInfos { get; set; }

    public required string Year { get; set; }

    public required string Region { get; set; }

    public required ICollection<string> Tags { get; set; }

    public float Rating { get; set; }

    public int RatingCount { get; set; }

    public string? Quote { get; set; }

    public required string Url { get; set; }

    public required string PreviewImage { get; set; }

    public required ICollection<Favorite> Favorites { get; set; }
}
