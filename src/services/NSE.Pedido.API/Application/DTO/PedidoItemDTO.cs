using NSE.Pedidos.Domain;

namespace NSE.Pedido.API.Application.DTO
{
    public class PedidoItemDTO
    {
        public Guid PedidoId { get; set; }

        public Guid ProdutoId { get; set; }

        public string Nome { get; set; }

        public int Quantidade { get; set; }

        public decimal Valor { get; set; }

        public decimal Imagem { get; set; }

        public static PedidoItem ParaPedidoItem(PedidoItemDTO pedidoItemDTO)
        {
            return new PedidoItem(pedidoItemDTO.PedidoId, pedidoItemDTO.Nome, pedidoItemDTO.Quantidade, pedidoItemDTO.Valor, pedidoItemDTO.Imagem);
        }
    }
}
