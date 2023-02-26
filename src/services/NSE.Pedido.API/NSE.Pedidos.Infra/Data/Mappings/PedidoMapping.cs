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
    public class PedidoDomainMapping : IEntityTypeConfiguration<PedidoDomain>
    {
        public void Configure(EntityTypeBuilder<PedidoDomain> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.Endereco, e =>
            {
                e.Property(z => z.Logradouro).HasColumnName("Logradouro");

                e.Property(z => z.Numero).HasColumnName("Numero");

                e.Property(z => z.Complemento).HasColumnName("Complemento");

                e.Property(z => z.Bairro).HasColumnName("Bairro");

                e.Property(z => z.Cep).HasColumnName("Cep");

                e.Property(z => z.Cidade).HasColumnName("Cidade");

                e.Property(z => z.Estado).HasColumnName("Estado");
            });

            builder.Property(x => x.Codigo).HasDefaultValueSql("NEXT VALUE FOR MinhaSequencia");

            // 1 : N => Pedido : PedidoItens
            builder.HasMany(x => x.Pedidoitens).WithOne(x => x.Pedido).HasForeignKey(x => x.PedidoId);

            builder.ToTable("Pedidos");
        }
    }
}
