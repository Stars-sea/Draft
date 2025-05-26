using Microsoft.AspNetCore.Identity;

namespace Draft.Models;

public class UserProfile : IdentityUser<int> {
    public ICollection<Favorite> Favorites { get; set; } = [];
}
