using System.Text.RegularExpressions;
using Draft.Server.Models;
using HtmlAgilityPack;

namespace Draft;

public static partial class DoubanMovieHelper {
    public static DoubanMovie ParseFromHtml(HtmlNode node) {
        HtmlNode ratingNode = node.SelectSingleNode(".//span[@class='rating_num']")!;

        string  title       = node.SelectSingleNode(".//span[@class='title']")!.InnerText;
        string  description = node.SelectSingleNode(".//div[@class='bd']/p")!.InnerText;
        float   rating      = float.Parse(ratingNode.InnerText);
        int     ratingCount = int.Parse(ratingNode.SelectSingleNode("..//span[4]")!.InnerText[..^3]);
        string? quote       = node.SelectSingleNode(".//p[@class='quote']/span")?.InnerText;
        string  url         = node.SelectSingleNode(".//div[@class='hd']/a")!.GetAttributeValue("href", "");
        string  image       = node.SelectSingleNode(".//img[@src]")!.GetAttributeValue("src", "");

        string[] otherTitles =
            node.SelectNodes(".//span[@class='title'] | .//span[@class='other']")!
                .Select(n => n.InnerText).Skip(1)
                .SelectMany(s => HtmlSymbolRegex().Replace(s, "").Split('/'))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToArray();

        string[] descriptions =
            description
                .Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => HtmlSymbolRegex().Replace(s, "").Trim())
                .ToArray();

        return new DoubanMovie(title, otherTitles, descriptions, rating, ratingCount, quote, url, image);
    }

    [GeneratedRegex(@"&\w+?;")]
    private static partial Regex HtmlSymbolRegex();
}