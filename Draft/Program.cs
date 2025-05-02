using Draft;
using Draft.Server.Models;
using HtmlAgilityPack;

const string url = "https://movie.douban.com/top250";

HtmlWeb web = new() {
    UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36"
};

await using DoubanMovieDb db = new();

db.Database.EnsureCreated();

for (var i = 0; i < 250; i += 25) {
    await foreach (DoubanMovie movie in FetchDoubanMovies(i)) await db.Movies.AddAsync(movie);
    await db.SaveChangesAsync();
}

return;

async IAsyncEnumerable<DoubanMovie> FetchDoubanMovies(int start) {
    string       currentUrl = start == 0 ? url : $"{url}?start={start}&filter=";
    HtmlDocument document   = await web.LoadFromWebAsync(currentUrl);
    while (document.Text.Contains("有异常请求")) {
        Console.WriteLine($"被服务器盯住啦_(:з)∠)_, 歇一会~ [{currentUrl}]");
        await Task.Delay(60_000);
        document = await web.LoadFromWebAsync(currentUrl);
    }

    HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//div[@class='item']")!;
    for (var i = 0; i < nodes.Count; ++i) yield return DoubanMovieHelper.ParseFromHtml(nodes[i], start + i);
}
