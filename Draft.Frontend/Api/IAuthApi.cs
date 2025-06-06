using Draft.Models.Dto.Authentication;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Draft.Frontend.Api;

public interface IAuthApi {
    [Post("/api/v1/auth/login")]
    Task<AuthenticationResponse> Login([FromBody] LoginRequest request);
    
    [Post("/api/v1/auth/register")]
    Task<AuthenticationResponse> Register([FromBody] RegisterRequest request);
}
