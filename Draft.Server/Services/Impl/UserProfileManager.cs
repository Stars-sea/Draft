using System.Diagnostics;
using Draft.Models;
using Draft.Models.Identity;
using Draft.Server.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Draft.Server.Services.Impl;

public class UserProfileManager(ApplicationDb dbContext, IPasswordHasher<UserProfile> hasher) : IUserProfileManager {

    private readonly DbSet<UserProfile> _profiles = dbContext.Profiles;


    public async Task<int> RegisterUserAsync(string email, string nickname, string password) {
        if (await IsEmailExistsAsync(email) || await IsNicknameExistsAsync(nickname))
            return -1;

        UserProfile userProfile = new() {
            Nickname     = nickname,
            Email        = email,
            PasswordHash = "",
            Favorites    = []
        };
        userProfile.PasswordHash = hasher.HashPassword(userProfile, password);

        _profiles.Add(userProfile);
        await dbContext.SaveChangesAsync();
        return userProfile.Id;
    }

    public async Task<bool> UnregisterUserAsync(int id) {
        UserProfile? profile = await FindUserAsync(id);
        if (profile == null)
            return false;

        _profiles.Remove(profile);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateUserAsync(UserProfile userProfile) {
        if (!await IsExistsAsync(userProfile.Id))
            return false;

        _profiles.Update(userProfile);
        return true;
    }


    #region Query Profiles

    public async Task<bool> IsExistsAsync(int userId)
        => await _profiles.FindAsync(userId) != null;

    public async Task<bool> IsEmailExistsAsync(string email)
        => await _profiles.AnyAsync(p => p.Email == email);

    public async Task<bool> IsNicknameExistsAsync(string nickname)
        => await _profiles.AnyAsync(p => p.Nickname == nickname);

    public bool TryFindUser(UserIdentity userIdentity, out int userId) {
        if (userIdentity.IsIdValid() && _profiles.Find(userIdentity.Id) != null) {
            Debug.Assert(userIdentity.Id != null);
            userId = userIdentity.Id.Value;
            return true;
        }

        if (userIdentity.IsEmailValid() &&
            _profiles.FirstOrDefault(p => p.Email == userIdentity.Email) is { } profile1) {
            userId = profile1.Id;
            return true;
        }

        if (userIdentity.IsNicknameValid() &&
            _profiles.FirstOrDefault(p => p.Nickname == userIdentity.Nickname) is { } profile2) {
            userId = profile2.Id;
            return true;
        }

        userId = 0;
        return false;
    }

    public async Task<UserProfile?> FindUserAsync(int id) {
        if (id <= 0) return null;
        
        return await _profiles.FindAsync(id);
    }

    public async Task<UserProfile?> FindUserAsync(UserIdentity userIdentity) {
        if (userIdentity.IsIdValid())
            return await _profiles.FindAsync(userIdentity.Id);

        if (userIdentity.IsEmailValid())
            return await _profiles.FirstOrDefaultAsync(p => p.Email == userIdentity.Email);

        if (userIdentity.IsNicknameValid())
            return await _profiles.FirstOrDefaultAsync(p => p.Nickname == userIdentity.Nickname);

        return null;
    }

    #endregion

}
