using Draft.Models.Dto.Profile;
using Refit;

namespace Draft.Frontend.Api;

[Headers("Authorization: Bearer")]
public interface IProfileApi {
    [Get("/api/v1/profile/{username}")]
    Task<ProfileResponse> GetProfile(string username);
}
