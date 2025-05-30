namespace Draft.Models;

public class Favorite {
    public string UserId { get; set; }

    public int MovieId { get; set; }

    public required DateTime CreatedAt { get; init; }

    public required UserProfile User { get; init; }

    public required DoubanMovie Movie { get; init; }
}
