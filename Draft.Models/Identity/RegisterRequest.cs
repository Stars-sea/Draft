namespace Draft.Models.Identity;

public sealed record RegisterRequest(string Nickname, string Email, string Password) {
    public bool IsPasswordValid() => Password.Length > 6;
}
