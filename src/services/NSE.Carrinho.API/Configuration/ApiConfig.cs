using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Carrinho.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CarrinhoContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddSwaggerConfiguration();

            services.RegisterServices();

            services.AddJwtConfiguration(configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder => builder
                                    .AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });
        }

        public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerConfiguration();

            app.UseHttpsRedirection();

            //app.UseRouting();

            //Usar o de baixo no lugar do app.UseAuthConfiguration();
            app.UseJwtConfiguration();

        }
    }
}
