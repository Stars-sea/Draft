using Draft.Server.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services
       .AddDatabase(builder.Configuration)
       .AddIdentity()
       .AddAuthentication(builder.Configuration)
       .AddInfrastructure();

builder.Services.AddControllers();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
