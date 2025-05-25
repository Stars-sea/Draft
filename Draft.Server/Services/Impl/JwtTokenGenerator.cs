using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Draft.Server.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Draft.Server.Services.Impl;

public class JwtTokenGenerator(
    IOptions<JwtSettings> settings,
    IDateTimeProvider dateTimeProvider
) : IJwtTokenGenerator {

    private JwtSettings JwtSettings => settings.Value;

    public string Generate(int userId, string nickname) {
        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Secret!)),
            SecurityAlgorithms.HmacSha256
        );

        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.GivenName, nickname),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        JwtSecurityToken securityToken = new(
            JwtSettings.ValidIssuer,
            JwtSettings.ValidAudience,
            claims,
            expires: dateTimeProvider.UtcNow.AddDays(7),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
