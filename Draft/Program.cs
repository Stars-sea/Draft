﻿using Draft;
using Draft.Models.Dto.Movie;
using HtmlAgilityPack;
using Refit;

const string url = "https://movie.douban.com/top250";

HtmlWeb web = new() {
    UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36"
};

IDoubanMoviesApi api = RestService.For<IDoubanMoviesApi>("http://localhost:5229");

const int last = 115;

for (int i = (last + 1) / 25 * 25; i < 250; i += 25) {
    await foreach (DoubanMovieModifyRequest movie in FetchDoubanMovies(i))
        await api.PutDoubanMovie(movie);
    await Task.Delay(5_000);
    Console.WriteLine($"已成功推送 {i + 25} 个");
}

return;

async IAsyncEnumerable<DoubanMovieModifyRequest> FetchDoubanMovies(int start) {
    string       currentUrl = start == 0 ? url : $"{url}?start={start}&filter=";
    HtmlDocument document   = await web.LoadFromWebAsync(currentUrl);
    while (document.Text.Contains("有异常请求")) {
        Console.WriteLine($"被服务器盯住啦_(:з)∠)_, 歇一会~ [{currentUrl}]");
        await Task.Delay(60_000);
        document = await web.LoadFromWebAsync(currentUrl);
    }

    IEnumerable<HtmlNode> nodes = document.DocumentNode.SelectNodes("//div[@class='item']")!;
    if (start < last)
        nodes = nodes.Skip(last - start);
    
    foreach (HtmlNode node in nodes)
        yield return await DoubanMovieHelper.ParseFromHtmlAsync(node);
}
