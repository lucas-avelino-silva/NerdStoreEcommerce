using FluentValidation;
using System.Text.Json.Serialization;

namespace NSE.Carrinho.API.Model
{
    public class CarrinhoItem
    {
        public CarrinhoItem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid ProdutoId { get; set; }

        public string Nome { get; set; }

        public int Quantidade { get; set; }

        public decimal Valor { get; set; }

        public string Imagem { get; set; }

        public Guid CarrinhoId { get; set; }

        [JsonIgnore] //pra nao dar pau quando serializar
        public CarrinhoCliente? CarrinhoCliente { get; set; }

        internal void AssociarCarrinho(Guid carrinhoId)
        {
            CarrinhoId = carrinhoId;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * Valor;
        }

        internal void AdicionarUnidades(int unidades)
        {
            Quantidade += unidades;
        }

        internal void AtualizarUnidades(int unidades)
        {
            Quantidade = unidades;
        }

        internal bool EhValido()
        {
            return new ItemCarrinhoValidation().Validate(this).IsValid;
        }

        public class ItemCarrinhoValidation : AbstractValidator<CarrinhoItem>
        {
            public ItemCarrinhoValidation()
            {
                RuleFor(x => x.ProdutoId).NotEqual(Guid.Empty).WithMessage("Id do produto inválido");

                RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome do produto não foi informado");

                RuleFor(x => x.Quantidade).GreaterThan(0).WithMessage(x => $"A quantidade minima para o {x.Nome} é 1");

                RuleFor(x => x.Quantidade)
                    .LessThanOrEqualTo(CarrinhoCliente.MAX_QUANTIDADE_ITEM)
                    .WithMessage(x => $"A quantidade máxima do {x.Nome} é {CarrinhoCliente.MAX_QUANTIDADE_ITEM}");

                RuleFor(x => x.Valor).GreaterThan(0).WithMessage(x => $"O valor o {x.Nome} precisa ser maior que 0");
            }
        }
    }
}
