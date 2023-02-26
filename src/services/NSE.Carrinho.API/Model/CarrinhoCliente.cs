﻿using FluentValidation;
using FluentValidation.Results;

namespace NSE.Carrinho.API.Model
{
    public class CarrinhoCliente
    {
        public CarrinhoCliente(Guid clienteid)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteid;
            Itens = new List<CarrinhoItem>();
        }

        public CarrinhoCliente()
        {
        }

        internal const int MAX_QUANTIDADE_ITEM = 5;

        public Guid Id { get; set; }

        public Guid ClienteId { get; set; }

        public decimal ValorTotal { get; set; }

        public List<CarrinhoItem> Itens { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool VoucherUtilizado { get; set; }

        public decimal Desconto { get; set; }

        public Voucher Voucher { get; set; }

        public void AplicarVoucher(Voucher voucher)
        {
            Voucher = voucher;

            VoucherUtilizado = true;

            CalcularValorCarrinho();
        }

        private void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;

            var valor = ValorTotal;

            if(Voucher.TipoDesconto == TipoDescontoVoucher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = (valor * Voucher.Percentual.Value) / 100;

                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;

                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;

            Desconto = desconto;
        }

        internal void CalcularValorCarrinho()
        {
            //eu vou somar o resultado do valor do calculo de cada item x o numero de quantidades
            ValorTotal = Itens.Sum(p => p.CalcularValor());

            CalcularValorTotalDesconto();
        }

        internal bool CarrinhoItemExistente(CarrinhoItem item)
        {
            return Itens.Any(p => p.ProdutoId == item.ProdutoId);
        }

        internal CarrinhoItem ObterPorProdutoId(Guid produtoId)
        {
            return Itens.FirstOrDefault(p => p.ProdutoId == produtoId);
        }

        internal void AdicionarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);

            if (CarrinhoItemExistente(item))
            {
                var itemExistente = ObterPorProdutoId(item.ProdutoId);

                itemExistente.AdicionarUnidades(item.Quantidade);

                item = itemExistente;

                Itens.Remove(itemExistente);
            }

            Itens.Add(item);

            CalcularValorCarrinho();
        }

        internal void AtualizarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);

            var itemExistente = ObterPorProdutoId(item.ProdutoId);

            Itens.Remove(itemExistente);

            Itens.Add(item);

            CalcularValorCarrinho();
        }

        internal void AtualizarUnidades(CarrinhoItem item, int unidades)
        {
            item.AtualizarUnidades(unidades);

            AtualizarItem(item);
        }

        internal void RemoverItem(CarrinhoItem item)
        {
            var itemExistente = ObterPorProdutoId(item.ProdutoId);

            //if (itemExistente == null) throw new Exception("O item não pertence ao pedido");

            Itens.Remove(itemExistente);

            CalcularValorCarrinho();
        }

        internal bool EhValido()
        {
            //pra cada item eu vou execultar a validação
            var erros = Itens.SelectMany(x => new CarrinhoItem.ItemCarrinhoValidation().Validate(x).Errors).ToList();

            erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors);

            //colocando os erros na propriedade q criei lá em cima.
            ValidationResult = new ValidationResult(erros);

            return ValidationResult.IsValid;
        }

        public class CarrinhoClienteValidation : AbstractValidator<CarrinhoCliente>
        {
            public CarrinhoClienteValidation()
            {
                RuleFor(x => x.ClienteId).NotEqual(Guid.Empty).WithMessage("Cliente não reconhecido.");

                RuleFor(x => x.Itens.Count).GreaterThan(0).WithMessage("O carrinho não possui itens.");

                RuleFor(x => x.ValorTotal).GreaterThanOrEqualTo(0).WithMessage("O valor total do carrinho precisa ser maior que 0.");

            }
        }
    }
}
