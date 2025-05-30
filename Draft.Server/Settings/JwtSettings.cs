namespace Draft.Server.Settings;

internal class JwtSettings {
    public const string Section = "JwtSettings";

    public string? ValidIssuer { get; set; }

    public string? ValidAudience { get; set; }

    public string? Secret { get; set; }
}
