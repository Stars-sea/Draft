using HtmlAgilityPack;

namespace Draft;

public record DoubanMovie(
    string Title,
    string Description,
    string Rating,
    string Quote,
    string Url
)
{
    public static DoubanMovie ParseFromHtml(HtmlNode node)
    {
        string title = node.SelectSingleNode(".//span[@class='title']")!.InnerText;
        string description = node.SelectSingleNode(".//div[@class='bd']/p")!.InnerText;
        string rating = node.SelectSingleNode(".//span[@class='rating_num']")!.InnerText;
        string quote = node.SelectSingleNode(".//p[@class='quote']/span")?.InnerText ?? string.Empty;
        string url = node.SelectSingleNode(".//div[@class='hd']/a")!.GetAttributeValue("href", "");

        description = string.Join('\n',
            description.Split('\n')
                       .Where(s => !string.IsNullOrWhiteSpace(s))
                       .Select(s => s.Trim()));
        
        return new DoubanMovie(title, description, rating, quote, url);
    }
}
