using Microsoft.AspNetCore.Authentication.Cookies;

namespace SNE.WebApp.MVC.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    //quando o user nao estiver logado, ele é redirecionado pra essa pagina
                    options.LogoutPath = "/login";
                    // quando a app sabe quem é o user, mas msm assim ele nao tem acesso a um determinado recurso
                    options.AccessDeniedPath = "/acesso-negado";
                });

            return services;
        }

        public static IApplicationBuilder UseIdentityConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }
    }
}
