using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using SNE.Core.DomainObjects;

namespace NSE.Clientes.API.Data.Mappings
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome).IsRequired().HasColumnType("varchar(200)");

            // O cpf pertence ao cliente. O cliente possui o cpf
            //O cpf é uma classe, entao qunado eu falo do cpf, na vdd eu to mapeando o numero
            builder.OwnsOne(c => c.Cpf, tf =>
            {
                tf.Property(c => c.Numero).IsRequired().HasMaxLength(Cpf.CpfMaxLength).HasColumnName("Cpf").HasColumnType($"varchar({Cpf.CpfMaxLength})");
            });

            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.Endereco).IsRequired().HasMaxLength(11).HasColumnName("Email").HasColumnType($"varchar({Email.EnderecoMaxLength})");
            });

            builder.HasOne(c => c.Endereco).WithOne(c => c.Cliente).HasForeignKey<Endereco>(x => x.ClientId);

            builder.ToTable("Clientes");
        }
    }
}
