using System.Text.Encodings.Web;
using System.Text.Json;
using Draft;
using HtmlAgilityPack;

const string url = "https://movie.douban.com/top250";

HtmlWeb web = new()
{
    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36"
};

List<DoubanMovie> movies = [];
for (var i = 0; i < 10; i++)
{
    string currentUrl = i == 0 ? url : $"{url}?start={i * 25}&filter=";
    movies.AddRange(await FetchDoubanPage(currentUrl));
}

if (File.Exists("movies.json"))
    File.Delete("movies.json");

await using FileStream file = File.OpenWrite("movies.json");
await JsonSerializer.SerializeAsync(file, movies, new JsonSerializerOptions()
{
    WriteIndented = true,
    Encoder       = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
});

return ;

async Task<IEnumerable<DoubanMovie>> FetchDoubanPage(string currentUrl)
{
    HtmlDocument doc = await web.LoadFromWebAsync(currentUrl);
    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='item']") ?? throw new InvalidOperationException();

    return nodes.Select(DoubanMovie.ParseFromHtml);
}
