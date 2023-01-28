using FluentValidation.Results;
using MediatR;
using NSE.Clientes.API.Application.Commands;
using NSE.Clientes.API.Application.Events;
using NSE.Clientes.API.Data;
using NSE.Clientes.API.Data.Repository;
using NSE.Clientes.API.Models;
using NSE.Clientes.API.Services;
using SNE.Core.Mediator;

namespace NSE.Clientes.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();


            //aqui eu falo q o "RegistrarClienteCommand" q vai ser entregue via "IRequestHandler" e q vai retornar
            // um "ValidationResult" vai ser manipulada pelo "ClienteCommandHandler"
            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();

            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<ClientesContext>();

            services.AddScoped<IClienteRepository, ClienteRepository>();


            // esse cara trabalha no singlton. Registrando a classe background. O AddHostedService funciona no singleton
            //Uma vez q tenho uma instancia de um obj singleton, eu não posso injetar nd dentro dele q nao seja singleton
            //services.AddHostedService<RegistroClienteIntegrationHandler>();
        }
    }
}
