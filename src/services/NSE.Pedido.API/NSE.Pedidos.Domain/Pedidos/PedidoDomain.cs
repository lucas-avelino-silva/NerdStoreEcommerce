using NSE.Pedidos.Domain.Vouchers;
using SNE.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Pedidos.Domain
{
    public class PedidoDomain : Entity, IAggregateRoot
    {
        public int Codigo { get; private set; }

        public Guid ClienteId { get; private set; }

        public Guid? VoucherId { get; private set; }

        public bool VoucherUtilizado { get; private set; }

        public decimal Desconto { get; private set; }

        public decimal ValorTotal { get; private set; }

        public DateTime DataCadastro { get; private set; }

        public PedidoStatus PedidoStatus { get; private set; }

        private readonly List<PedidoItem> _pedidoItens;

        public PedidoDomain(Guid clienteId, decimal valorTotal, List<PedidoItem> pedidoItens, bool voucherUtilizado = false, decimal desconto = 0, Guid? voucherId = null)
        {
            ClienteId = clienteId;
            ValorTotal = valorTotal;          
            _pedidoItens = pedidoItens;

            VoucherId = voucherId;
            VoucherUtilizado = voucherUtilizado;
            Desconto = desconto;
        }

        //EF
        public PedidoDomain()
        {
                
        }

         //ninguem consegue manipular a lista, apenas ler
        public IReadOnlyCollection<PedidoItem> Pedidoitens => _pedidoItens;

        //obj de valor
        public Endereco Endereco { get; private set; }

        //relacao do entity framework
        public Voucher Voucher { get; private set; }

        public void AutorizarPedido()
        {
            PedidoStatus = PedidoStatus.Autorizado;
        }

        public void AtribuirVoucher(Voucher voucher)
        {
            VoucherUtilizado = true;
            VoucherId = voucher.Id;
            Voucher = voucher;
        }

        public void AtribuirEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }

        public void CalcularValorPedido()
        {
            ValorTotal = Pedidoitens.Sum(x => x.CalcularValor());

            CalcularValorTotalDesconto();
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;

            var valor = ValorTotal;

            if (Voucher.TipoDesconto == TipoDescontoVoucher.Porcentagem)
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
    }
}
