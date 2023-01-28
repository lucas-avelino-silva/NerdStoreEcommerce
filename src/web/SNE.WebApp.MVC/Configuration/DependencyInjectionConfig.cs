using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using SNE.WebApp.MVC.Extensions;
using SNE.WebApp.MVC.Services;
using SNE.WebApp.MVC.Services.Handlers;

namespace SNE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegistrarServicos(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAutentificacaoService, AutentificacaoService>();

            //aqui eu falo q vai usar esse handler pra manipular esse request
            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            //Polly
            //.AddTransientHttpErrorPolicy(p =>
            //        //que tipo de politica? "WaitAndRetryAsync" com 3 tentativas com o tempo de espera de 600
            //        p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));
            //.AddPolicyHandler(PollyExtensions.EsperarTentar())
            //.AddTransientHttpErrorPolicy(p =>
            //    p.CircuitBreakerAsync(5, TimeSpan.FromMilliseconds(30)));

            services.AddHttpClient("Refit", options =>
            {
                options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
            })
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, AspNetUser>();
        }
    }

    public static class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
        {
            var retryWaitPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                });

            return retryWaitPolicy;
        }
    }
}
