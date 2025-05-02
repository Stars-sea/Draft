# Draft

Some draft code that implements a ASP.NET server.  
随便写着玩玩，也学学 ASP.NET 和数据库

## Draft

A spider that fetch douban Top 250 movies, and stores these data into database.  
爬取豆瓣评分 Top 250 的电影并储存到数据库

The schema of a single movie data / 数据结构如下:

```csharp
// Draft.Server.Models;

public class DoubanMovie {
    public int Rank { get; init; }

    public string Title { get; set; }

    public ICollection<string> OtherTitles { get; set; }

    public string StaffInfos { get; set; }

    public string Year { get; set; }

    public string Region { get; set; }

    public ICollection<string> Tags { get; set; }

    public float Rating { get; set; }

    public int RatingCount { get; set; }

    public string? Quote { get; set; }

    public string Url { get; set; }

    public string PreviewImage { get; set; }
}
```

And we use `Title` property as the key of the database sheet.  
Type `ICollection<string>` is serialized as JSON string.

`Title` 属性作为数据表的键，
`ICollection<string>` 类型会被转换成 JSON 格式的字符串，再存到数据库

Database connection string can be configured in `Draft/DoubanMovieDb.cs`  
数据库连接字符串可以在 `Draft/DoubanMovieDb.cs` 中配置

## Draft.Server

The server that process requests and 
enables other apps access to the data with a RESTful API style.  
处理来自其他应用的请求并返回数据 (使用 RESTful API 风格)

The OpenAPI Document is available if the server is running:  
在服务器运行时你可以访问到 OpenAPI 文档:
```text
http://localhost:5229/openapi/v1.json
```

Also, the database connection can be edited in `Draft.Server/appsettings(.Development).json`  
同样，关于数据库的连接也可以在 `Draft.Server/appsettings(.Development).json` 中设置

## Draft.Frontend

The server display the data from the backend server.  
显示来自后端服务器的数据

The api url is configured in `Draft.Frontend/appsettings(.Development).json`  
API 地址在 `Draft.Server/appsettings(.Development).json` 里配置

To access douban image resource, 
the server also acts as the proxy of these resources and caches them.  
为访问到豆瓣相关图片资源，此服务器也代理并缓存了这些图片

The webpage URL / 网页链接: 
```text
http://localhost:5077
```

The proxy of resource / 资源代理链接:
```text
http://localhost:5077/img/{id}
```

## LICENSE

This repo is distribute under [MIT](https://mit-license.org/)  
此仓库遵循 [MIT](https://mit-license.org/) 协议开源
