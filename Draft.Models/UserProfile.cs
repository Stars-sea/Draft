namespace Draft.Models;

public class UserProfile {
    public int Id { get; set; }

    public required string Nickname { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public required int Salt { get; set; }

    public required ICollection<Favorite> Favorites { get; set; }
}
