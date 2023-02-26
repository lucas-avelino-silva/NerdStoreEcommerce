using NSE.Pedido.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);

builder.Configuration.AddJsonFile("appsettings.json", true, true);

builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddApiConfiguration(builder.Configuration);

var app = builder.Build();

app.UseApiConfiguration(app.Environment);

app.MapControllers();

app.Run();
