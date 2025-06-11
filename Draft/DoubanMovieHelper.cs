using System.Text.RegularExpressions;
using Draft.Models.Dto.Movie;
using HtmlAgilityPack;

namespace Draft;

public static partial class DoubanMovieHelper {
    private static readonly HttpClient Client = new();

    private static int _currentTaskCount;

    static DoubanMovieHelper() {
        Client.DefaultRequestHeaders.Add(
            "User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36"
        );
    }

    private static async Task<string> GetImageAsync(string url) {
        while (_currentTaskCount > 5) {
            await Task.Delay(10);
        }

        var task = Client.GetByteArrayAsync(url);

        ++_currentTaskCount;
        byte[] image = await task;
        --_currentTaskCount;

        return Convert.ToBase64String(image);
    }

    public static async Task<DoubanMovieModifyRequest> ParseFromHtmlAsync(HtmlNode node) {
        HtmlNode ratingNode = node.SelectSingleNode(".//span[@class='rating_num']")!;

        int     rank        = int.Parse(node.SelectSingleNode(".//em")!.InnerText);
        string  title       = node.SelectSingleNode(".//span[@class='title']")!.InnerText;
        string  description = node.SelectSingleNode(".//div[@class='bd']/p")!.InnerText;
        float   rating      = float.Parse(ratingNode.InnerText);
        int     ratingCount = int.Parse(ratingNode.SelectSingleNode("..//span[4]")!.InnerText[..^3]);
        string? quote       = node.SelectSingleNode(".//p[@class='quote']/span")?.InnerText;
        string  url         = node.SelectSingleNode(".//div[@class='hd']/a")!.GetAttributeValue("href", "");
        string  image       = node.SelectSingleNode(".//img[@src]")!.GetAttributeValue("src", "");

        var otherTitles =
            node.SelectNodes(".//span[@class='title'] | .//span[@class='other']")!
                .Select(n => n.InnerText).Skip(1)
                .SelectMany(s => HtmlSymbolRegex().Replace(s, "").Split('/'))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToList();

        string[] descriptions =
            description
                .Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => HtmlSymbolRegex().Replace(s, "").Trim())
                .ToArray();

        string   staffInfos = descriptions[0];
        string[] infos      = descriptions[1].Split('/').Select(s => s.Trim()).ToArray();

        return new DoubanMovieModifyRequest(title) {
            Rank               = rank,
            OtherTitles        = otherTitles,
            StaffInfos         = staffInfos,
            Year               = string.Join('/', infos[..^2]),
            Region             = infos[^2],
            Tags               = infos[^1].Split(' ').ToList(),
            Rating             = rating,
            RatingCount        = ratingCount,
            Quote              = quote,
            Url                = url,
            PreviewImageBase64 = await GetImageAsync(url),
        };
    }

    [GeneratedRegex(@"&\w+?;")]
    private static partial Regex HtmlSymbolRegex();
}
