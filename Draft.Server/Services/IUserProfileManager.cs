using Draft.Models;
using Draft.Models.Identity;

namespace Draft.Server.Services;

public interface IUserProfileManager {

    Task<int> RegisterUserAsync(string email, string nickname, string password);

    Task<bool> UnregisterUserAsync(int id);

    Task<bool> UpdateUserAsync(UserProfile userProfile);

    #region Query Profile

    Task<bool> IsExistsAsync(UserIdentity userIdentity) {
        if (userIdentity.IsIdValid()) return IsExistsAsync(userIdentity.Id!.Value);
        if (userIdentity.IsEmailValid()) return IsEmailExistsAsync(userIdentity.Email!);
        if (userIdentity.IsNicknameValid()) return IsNicknameExistsAsync(userIdentity.Nickname!);

        return Task.FromResult(false);
    }

    Task<bool> IsExistsAsync(int userId);

    Task<bool> IsEmailExistsAsync(string email);

    Task<bool> IsNicknameExistsAsync(string nickname);

    bool TryFindUser(UserIdentity userIdentity, out int userId);

    Task<UserProfile?> FindUserAsync(int id);
    
    Task<UserProfile?> FindUserAsync(UserIdentity userIdentity);

    #endregion

}
