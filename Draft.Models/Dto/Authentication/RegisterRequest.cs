namespace Draft.Models.Dto.Authentication;

public sealed record RegisterRequest(string Username, string Email, string Password) {
    public bool IsPasswordValid() => Password.Length > 6;
}
