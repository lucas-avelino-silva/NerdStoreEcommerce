namespace NSE.Clientes.API.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "NerdStore Enterprise Clientes API",
                    Description = "Enterprise Applications.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact() { Name = "Lucas Avelino", Email = "lucasavelino2014@hotmail.com" },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(C =>
                {
                    C.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            return app;
        }
    }
}
