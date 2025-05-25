using Draft.Frontend.Api;
using Draft.Models;

namespace Draft.Frontend;

public static class DoubanImageProxy {
    private static readonly HttpClient Client = new();

    private static readonly Dictionary<int, byte[]> ImageCache = new();

    static DoubanImageProxy() {
        Client.DefaultRequestHeaders.Add(
            "User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36"
        );
    }

    public static async Task<IResult> GetDoubanImage(int id, IDoubanMovieApi api) {
        if (ImageCache.TryGetValue(id, out byte[]? image)) return Results.File(image, "image/jpeg");

        DoubanMovie movie = await api.GetDoubanMovie(id);
        byte[]      bytes = await Client.GetByteArrayAsync(movie.PreviewImage);

        ImageCache[id] = bytes;
        return Results.File(bytes, "image/jpeg");
    }
}
