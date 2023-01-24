using MediatR;
using Microsoft.AspNetCore.Builder;
using NSE.Clientes.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", true, true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddMediatR(typeof(Program));

builder.Services.RegisterServices();

builder.Services.AddApiConfiguration(builder.Configuration);

var app = builder.Build();


app.useApiConfiguration(app.Environment);

app.MapControllers();

app.Run();
