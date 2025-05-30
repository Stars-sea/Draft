namespace Draft.Models.Dto.Profile;

public record ProfileResponse(
    string Id,
    string Username
);

public record DetailedProfileResponse(
    string Id,
    string Username,
    string Email
) : ProfileResponse(Id, Username);
