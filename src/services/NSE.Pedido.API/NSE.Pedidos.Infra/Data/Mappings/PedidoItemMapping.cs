using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Pedidos.Domain;
using NSE.Pedidos.Domain.Pedidos;
using NSE.Pedidos.Domain.Vouchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Pedidos.Infra.Data.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProdutoNome).IsRequired().HasColumnType("varchar(250)");

            // 1 : N => Pedido : Pagamento - ja fiz no outro... não é necessario fazer aqui... mas eu fiz.
            builder.HasOne(x => x.Pedido).WithMany(x => x.Pedidoitens);

            builder.ToTable("PedidoItens");
        }
    }
}
