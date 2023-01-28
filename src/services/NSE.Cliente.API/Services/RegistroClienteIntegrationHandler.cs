using FluentValidation.Results;
using NSE.Clientes.API.Application.Commands;
using NSE.MessageBus;
using SNE.Core.Mediator;
using SNE.Core.Messages.Integration;

namespace NSE.Clientes.API.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        // BackgroundService é uma feature do .net core. Ele trabalha em background, ele não entra dentro do request, ele roda em paralelo
        // Ele ta "escultando", no memoento q ele receber o estimulo ele vai trabalhar

        //Aqui estou consumindo a fila.

        private readonly IMessageBus _bus;

        //é a configuração do startup q é na onde resolvemos as injeçoes de dependencia
        private readonly IServiceProvider _serviceProvider;

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;

            _bus = bus;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
                    await RegistrarCliente(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //O método "ExecuteAsync" vai ser chamado assim q minha aplicação subir
            SetResponder();

            return Task.CompletedTask;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
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

            return new ResponseMessage(sucesso);
        }
    }
}
