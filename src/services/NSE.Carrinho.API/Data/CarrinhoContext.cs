using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Model;
using SNE.Core.Messages;

namespace NSE.Carrinho.API.Data
{
    public class CarrinhoContext : DbContext
    {
        public CarrinhoContext(DbContextOptions<CarrinhoContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<CarrinhoItem> CarrinhoItens { get; set; }

        public DbSet<CarrinhoCliente> CarrinhoCliente { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            modelBuilder.Ignore<ValidationResult>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Entity<CarrinhoCliente>()
                .HasIndex(c => c.ClienteId)
                .HasName("IDX_Cliente");

            modelBuilder.Entity<CarrinhoCliente>()
                .Ignore(x => x.Voucher)
                .OwnsOne(x => x.Voucher, v =>
                {
                    v.Property(z => z.Codigo)
                        .HasColumnName("VoucherCodigo")
                        .HasColumnType("varchar(50)");

                    v.Property(z => z.TipoDesconto)
                        .HasColumnName("TipoDesconto");

                    v.Property(z => z.Percentual)
                        .HasColumnName("Percentual");

                    v.Property(z => z.ValorDesconto)
                        .HasColumnName("ValorDesconto");
                });

            modelBuilder.Entity<CarrinhoCliente>()
                .HasMany(c => c.Itens)
                .WithOne(x => x.CarrinhoCliente)
                .HasForeignKey(x => x.CarrinhoId);

            // desabilitando o delete em cascada nos relacionamento
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }
    }
}
