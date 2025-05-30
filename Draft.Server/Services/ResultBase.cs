namespace Draft.Server.Services;

public record ResultBase<TContent> where TContent : notnull {
    public bool IsSuccess { get; init; }

    public required IEnumerable<ErrorMessage> Errors { get; init; }

    public TContent? Content { get; init; }

    public static implicit operator bool(ResultBase<TContent> result) => result.IsSuccess;
}
