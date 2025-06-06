using Draft.Models;

namespace Draft.Server.Services.Movie;

public record MovieQueryResults : ResultBase<IEnumerable<DoubanMovie>> {
    public static MovieQueryResults Success(params IEnumerable<DoubanMovie> movies)
        => new() {
            IsSuccess = true,
            Content   = movies,
            Errors    = []
        };

    public static MovieQueryResults Failed(params IEnumerable<ErrorMessage> errors)
        => new() {
            IsSuccess = false,
            Errors    = errors
        };
}
