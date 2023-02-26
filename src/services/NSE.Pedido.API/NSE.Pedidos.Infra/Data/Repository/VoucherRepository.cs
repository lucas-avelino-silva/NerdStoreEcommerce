using Microsoft.EntityFrameworkCore;
using NSE.Pedidos.Domain;
using NSE.Pedidos.Domain.Vouchers;
using SNE.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Pedidos.Infra.Data.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly PedidosContext _context;

        public VoucherRepository(PedidosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Voucher> ObterVoucherPorCodigo(string codigo)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(x => x.Codigo == codigo);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
