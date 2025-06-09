using Draft.Frontend.Api;
using Draft.Models.Dto.Movie;

namespace Draft.Frontend;

public static class DoubanImageProxy {
    private static readonly HttpClient Client = new();

    private static readonly Dictionary<int, byte[]> ImageCache = new();

    private static int _currentTaskCount = 0;

    static DoubanImageProxy() {
        Client.DefaultRequestHeaders.Add(
            "User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36"
        );
    }

    private static async Task<byte[]> GetImageAsync(DoubanMovieResponse movie) {
        int id = movie.Id!.Value;
        if (ImageCache.TryGetValue(id, out byte[]? value))
            return value;

        while (_currentTaskCount > 5) {
            await Task.Delay(10);
        }

        var task = Client.GetByteArrayAsync(movie.PreviewImage);

        ++_currentTaskCount;
        byte[] image = await task;
        --_currentTaskCount;

        return ImageCache[id] = image;
    }

    public static async Task<IResult> GetDoubanImage(int id, IDoubanMovieApi api) {
        if (ImageCache.TryGetValue(id, out byte[]? image))
            return Results.File(image, "image/jpeg");

        DoubanMovieResponse movie = await api.GetDoubanMovie(id);
        byte[]              bytes = await GetImageAsync(movie);
        return Results.File(bytes, "image/jpeg");
    }
}
