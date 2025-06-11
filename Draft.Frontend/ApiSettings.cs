namespace Draft.Frontend;

public sealed record ApiSettings {
    public required string BaseUrl { get; set; }
    
    public required string TokenStorageKey { get; set; }
}
