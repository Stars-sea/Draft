using Draft;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

const string url = "https://movie.douban.com/top250";

HtmlWeb web = new() {
    UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36"
};

await using DoubanMovieDbContext dbContext = new();

try {
    if (dbContext.Database.GetService<IDatabaseCreator>() is not RelationalDatabaseCreator creator)
        throw new NullReferenceException("RelationalDatabaseCreator is null");
    await creator.EnsureCreatedAsync();
}
catch (Exception e) {
    Console.WriteLine(e);
    throw;
}

for (var i = 0; i < 250; i += 25) {
    var nodes  = await FetchDoubanPageNodes(i);
    var movies = nodes.Select(DoubanMovie.ParseFromHtml).ToArray();

    await dbContext.Movies.AddRangeAsync(movies);
}

await dbContext.SaveChangesAsync();

return;

async Task<IEnumerable<HtmlNode>> FetchDoubanPageNodes(int start) {
    string       currentUrl = start == 0 ? url : $"{url}?start={start}&filter=";
    HtmlDocument document   = await web.LoadFromWebAsync(currentUrl);
    while (document.Text.Contains("有异常请求")) {
        Console.WriteLine($"被服务器盯住啦_(:з)∠)_, 歇一会~ [{currentUrl}]");
        await Task.Delay(60_000);
        document = await web.LoadFromWebAsync(currentUrl);
    }
    return document.DocumentNode.SelectNodes("//div[@class='item']") ?? throw new NullReferenceException();
}
