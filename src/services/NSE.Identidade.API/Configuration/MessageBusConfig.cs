using NSE.Clientes.API.Services;
using NSE.MessageBus;
using SNE.Core.Utils;

namespace NSE.Identidade.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
                //.AddHostedService<RegistroClienteIntegrationHandler>();
        }
    }
}
