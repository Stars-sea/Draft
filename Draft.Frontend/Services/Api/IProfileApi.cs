using Draft.Models.Dto.Movie;
using Draft.Models.Dto.Profile;
using Refit;

namespace Draft.Frontend.Services.Api;

[Headers("Authorization: Bearer")]
public interface IProfileApi {
    [Get("/api/v1/profile/{username}")]
    Task<ProfileResponse> GetProfile(string username);
    
    [Get("/api/v1/profile")]
    Task<DetailedProfileResponse> GetProfile();
    
    [Get("/api/v1/profile/favorites/{id}")]
    Task GetFavorite(int id);

    [Post("/api/v1/profile/favorites/{id}")]
    Task AddFavorite(int id);
    
    [Delete("/api/v1/profile/favorites/{id}")]
    Task RemoveFavorite(int id);
    
    [Get("/api/v1/profile/favorites")]
    Task<ICollection<DoubanMovieSimpleResponse>> GetFavorites();
}
