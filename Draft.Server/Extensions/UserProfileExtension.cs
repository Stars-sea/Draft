using Draft.Models;
using Draft.Models.Dto.Profile;

namespace Draft.Server.Extensions;

internal static class UserProfileExtension {
    public static ProfileResponse ToResponse(this UserProfile profile) => new(profile.Id, profile.UserName!);

    public static DetailedProfileResponse ToDetailedResponse(this UserProfile profile)
        => new(profile.Id, profile.UserName!, profile.Email!);
}
