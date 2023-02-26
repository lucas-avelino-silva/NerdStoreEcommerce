using SNE.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Pedidos.Domain
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; private set; }

        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }

        public int Quantidade { get; private set; }

        public decimal ValorUnitario { get; private set; }

        public decimal ProdutoImagem { get; set; }

        // Relacao EF
        public PedidoDomain Pedido { get; set; }

        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario, decimal produtoImagem)
        {
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ProdutoImagem = produtoImagem;
        }

        // EF
        public PedidoItem()
        {

        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}
