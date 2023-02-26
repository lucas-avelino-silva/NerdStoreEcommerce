using FluentValidation.Results;
using MediatR;
using SNE.Core.Messages;

namespace NSE.Pedido.API.Application.Commands
{
    public class PedidoCommandHandler : CommandHandler, IRequestHandler<AdicionarPedidoCommand, ValidationResult>
    {
        public Task<ValidationResult> Handle(AdicionarPedidoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            // Validacao do comando

            // mapear pedido

            //aplicar voucher se houver

            // validar pedido

            // processar pagamento

            // se pagamento tudo ok

            //adicionar evento

            //adicionar pedido repositorio

            //persistir dados de pedido e voucher
        }
    }
}
