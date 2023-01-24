using SNE.Core.DomainObjects;

namespace NSE.Clientes.API.Models
{
    public class Cliente : Entity, IAggregateRoot
    {
        public Cliente(Guid id ,string? nome, string? email, string? cpf)
        {
            Id = id;
            Nome = nome;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
            Excluido = false;
        }

        // EF Relation
        public Cliente()
        {
        }

        public string? Nome { get; private set; }
        public Email? Email { get; private set; }
        public Cpf? Cpf { get; private set; }
        public bool Excluido { get; private set; }
        public Endereco Endereco { get; private set; }
    }
}
