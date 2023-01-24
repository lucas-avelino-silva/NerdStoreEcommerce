using FluentValidation.Results;
using MediatR;
using NSE.Clientes.API.Application.Events;
using NSE.Clientes.API.Models;
using SNE.Core.Messages;

namespace NSE.Clientes.API.Application.Commands
{
    // IRequestHandler interface do mediatR. implementar a interface
    public class ClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, ValidationResult>
    {
        private readonly IClienteRepository _repository;

        public ClienteCommandHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        // Vai ser o manipulador do meu comando.
        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.Valido()) return message.ValidationResult;

            var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);

            var clienteExistente = await _repository.ObterPorCpf(cliente.Cpf.Numero);

            if(clienteExistente != null)
            {
                AdicionarErro("Existe um cliente com esse CPF.");
                return ValidationResult;
            }

            _repository.Adicionar(cliente);

            cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));

            if (!await _repository.UnitOfWork.Commit()) AdicionarErro("Houve um erro ao persistir os dados.");

            return message.ValidationResult;
        }
    }
}
