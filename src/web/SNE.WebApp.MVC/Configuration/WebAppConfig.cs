using Microsoft.AspNetCore.Authentication.Cookies;
using SNE.WebApp.MVC.Extensions;

namespace SNE.WebApp.MVC.Configuration
{
    public static class WebAppConfig
    {
        public static IServiceCollection AddWebAppConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();

            services.Configure<AppSettings>(configuration);

            return services;
        }

        public static IApplicationBuilder UseWebAppConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                //sao middleware. quando eu nao tratei vai cair nesse 500, q é o generico; vou entender q vou um erro de servidor
                app.UseExceptionHandler("/erro/500");
                //quando há um problma e existe um status code de erro dentro desse response eu vou redirecionar ele pra essa rota com o parametro q é o erro do status code ({0})
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();



            return app;
        }
    }
}
