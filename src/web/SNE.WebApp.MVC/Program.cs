using SNE.WebApp.MVC.Configuration;
using SNE.WebApp.MVC.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", true, true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

// Add services to the container.

builder.Services.AddIdentityConfiguration();

builder.Services.AddWebAppConfiguration(builder.Configuration);

builder.Services.RegistrarServicos(builder.Configuration);

var app = builder.Build();

app.UseWebAppConfiguration(builder.Environment);

app.UseIdentityConfiguration();

var supportedCultures = new[] { new CultureInfo("pt-BR") };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

//todo request vai passar por aqui, nao preciso espalhar request pela aplicacao, pq estou centralizando tdos erros dentro dele
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalogo}/{action=Index}/{id?}");

app.Run();
