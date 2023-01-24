using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NSE.WebAPI.Core.Identidade
{
    public static class JwtConfig
    {

        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // jwt

            var appSettingsSection = configuration.GetSection("TokenApi");

            //aqui eu falo q a classe q eu criei representa um trecho do meu appSettings
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                // toda vez q for autentiticar alguem o padrao de autentificação é com base em token
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // toda vez q for autentiticar alguem o padrao de autentificação é por token
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                //Vai requerer q a pessoa q esta chamando a api venha por https
                x.RequireHttpsMetadata = true;

                //se o token deve ser guardado no authenticationProprerties após uma autentificação feita com sucesso
                x.SaveToken = true;

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    //validar se quem está emitindo tem q ser o msm quando vc receber o token. entao no token vai ter quem emitiu aquele token. essa validação será feita não só no nome do emissor, mas tbm com a chave em si.
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });
        }

        public static void UseJwtConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseAuthorization();
        }
    }
}

