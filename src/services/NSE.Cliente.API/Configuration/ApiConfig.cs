using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Data;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Clientes.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection service, IConfiguration configuration)
        {

            service.AddDbContext<ClientesContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            service.AddControllers();
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();

            service.AddCors(options =>
            {
                options.AddPolicy("Total", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }


        public static void useApiConfiguration(this IApplicationBuilder applicationBuilder, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                applicationBuilder.UseSwagger();
                applicationBuilder.UseSwaggerUI();
            }

            applicationBuilder.UseHttpsRedirection();

            applicationBuilder.UseRouting();

            applicationBuilder.UseCors("Total");

            applicationBuilder.UseJwtConfiguration();
        }
    }
}
