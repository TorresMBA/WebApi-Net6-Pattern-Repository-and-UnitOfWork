using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Producto");

            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.Marca)
                .WithMany(p => p.Productos)
                .HasForeignKey(x => x.MarcaId);

            builder.HasOne(p => p.Categoria)
                .WithMany(p => p.Productos)//Se agrega p => p.Productos para que EF para mysql no agregue unas demás
                .HasForeignKey(p => p.CategoriaId);

        }
    }
}
