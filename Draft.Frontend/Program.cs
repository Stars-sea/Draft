using Draft.Frontend;
using Draft.Frontend.Api;
using Draft.Frontend.Components;
using Refit;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(
    RestService.For<IDoubanMovieApi>(builder.Configuration["ApiUrl"]!)
);

builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.MapGet("img/{id}", DoubanImageProxy.GetDoubanImage);

app.Run();
