using Draft.Models;

namespace Draft.Server.Services.Movie;

internal record MovieOperationResult : ResultBase<DoubanMovie> {
    public static MovieOperationResult Success(DoubanMovie? movie)
        => new() {
            IsSuccess = true,
            Content   = movie,
            Errors    = []
        };

    public static MovieOperationResult Failed(params IEnumerable<ErrorMessage> errors)
        => new() {
            IsSuccess = false,
            Errors    = errors
        };
}
