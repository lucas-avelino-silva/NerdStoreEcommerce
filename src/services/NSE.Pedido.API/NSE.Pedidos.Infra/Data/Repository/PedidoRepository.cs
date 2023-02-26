using Microsoft.EntityFrameworkCore;
using NSE.Pedidos.Domain;
using NSE.Pedidos.Domain.Pedidos;
using SNE.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Pedidos.Infra.Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidosContext _context;

        public PedidoRepository(PedidosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;


        public void Adicionar(PedidoDomain pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public void Atualizar(PedidoDomain pedido)
        {
            _context.Pedidos.Update(pedido);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<PedidoItem> ObterItemPorId(Guid id)
        {
            return await _context.PedidoItens.FindAsync(id);
        }

        public async Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId)
        {
            return await _context.PedidoItens.FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.PedidoId == pedidoId);
        }

        public async Task<List<PedidoDomain>> ObterListaPorClienteId(Guid clienteId)
        {
            var test = await _context.Pedidos.Include(x => x.Pedidoitens).AsNoTracking().Where(x => x.ClienteId == clienteId).ToListAsync();
            return await _context.Pedidos.Include(x => x.Pedidoitens).AsNoTracking().Where(x => x.ClienteId == clienteId).ToListAsync();
        }

        public async Task<PedidoDomain> ObterPorId(Guid id)
        {
            return await _context.Pedidos.FindAsync(id);
        }
    }
}
