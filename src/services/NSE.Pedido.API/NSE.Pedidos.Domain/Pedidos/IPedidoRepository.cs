using SNE.Core.Data;

namespace NSE.Pedidos.Domain.Pedidos
{
    public interface IPedidoRepository : IRepository<PedidoDomain>
    {
        Task<PedidoDomain> ObterPorId(Guid id);

        Task<List<PedidoDomain>> ObterListaPorClienteId(Guid clienteId);

        void Adicionar(PedidoDomain pedido);

        void Atualizar(PedidoDomain pedido);

        // Pedido Item. Repositorio por agregacao
        Task<PedidoItem> ObterItemPorId(Guid id);

        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);
    }
}
