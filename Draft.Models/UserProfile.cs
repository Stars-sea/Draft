using Microsoft.AspNetCore.Identity;

namespace Draft.Models;

public class UserProfile : IdentityUser {
    public ICollection<Favorite> Favorites { get; set; } = [];
}
