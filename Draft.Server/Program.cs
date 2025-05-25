using Draft.Server;
using Draft.Server.Database;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<DoubanMovieDb>(database => {
        string doubanMoviesConnectionString = builder.Configuration.GetConnectionString("DOUBAN_MOVIES_DATABASE")!;
        database.UseMySQL(doubanMoviesConnectionString);
    }
);

builder.Services.AddControllers();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
