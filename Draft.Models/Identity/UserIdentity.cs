using System.Text.RegularExpressions;

namespace Draft.Models.Identity;

public sealed partial record UserIdentity {
    public int? Id { get; init; }

    public string? Email { get; init; }

    public string? Nickname { get; init; }

    public bool IsIdValid() => Id is > 0;

    public bool IsEmailValid() => Email != null && EmailPattern().IsMatch(Email);

    public bool IsNicknameValid() => Nickname != null && NicknamePattern().IsMatch(Nickname);

    [GeneratedRegex(@"^([A-Za-z0-9_\-\.\u4e00-\u9fa5])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,8})$")]
    private static partial Regex EmailPattern();

    [GeneratedRegex(@"^\w{2,}$")]
    private static partial Regex NicknamePattern();
}
