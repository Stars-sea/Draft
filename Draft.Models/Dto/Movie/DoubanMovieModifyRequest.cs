namespace Draft.Models.Dto.Movie;

public sealed record DoubanMovieModifyRequest(string Title) {
    public ICollection<string>? OtherTitles { get; set; }

    public string? StaffInfos { get; set; }

    public string? Year { get; set; }

    public string? Region { get; set; }

    public ICollection<string>? Tags { get; set; }

    public float? Rating { get; set; }

    public int? RatingCount { get; set; }

    public int? Rank { get; set; }

    public string? Quote { get; set; }

    public string? Url { get; set; }

    public string? PreviewImage { get; set; }
}
