using EasyNetQ;
using FluentValidation.Results;
using NSE.Clientes.API.Application.Commands;
using SNE.Core.Mediator;
using SNE.Core.Messages.Integration;

namespace NSE.Clientes.API.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        // BackgroundService é uma feature do .net core. Ele trabalha em background, ele não entra dentro do request, ele roda em paralelo
        // Ele ta "escultando", no memoento q ele receber o estimulo ele vai trabalhar

        private IBus _bus;

        //é a configuração do startup q é na onde resolvemos as injeçoes de dependencia
        private readonly IServiceProvider _serviceProvider;

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus = RabbitHutch.CreateBus("host=localhost:5672");

            _bus.Rpc.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
                new ResponseMessage(await RegistrarCliente(request)));

            return Task.CompletedTask;
        }

        private async Task<ValidationResult> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);

            ValidationResult sucesso;

            //pego o container da injeção de dependencia e crio um escopo, e vou buscar dentro dele com base na interface "IMediatorHandler" e vou buscar uma instancia
            // é a msm coisa de injetar no construtor, mas assim atrapalha fazer testes e não é indicado para todos cenários
            // não podemos injetar via construtor pq ele (IMediatorHandler) é scoped e esse cara (RegistroClienteIntegrationHandler) é resolvido como "AddHodtedService" q trabalha como singleton -  **Verificar pra ver se é isso msm

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                sucesso = await mediator.EnviarComando(clienteCommand);
            }

            return sucesso;
        }
    }
}
