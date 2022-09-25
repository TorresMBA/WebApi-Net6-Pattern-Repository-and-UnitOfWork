using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Nombres)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(p => p.ApellidoMaterno)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(p => p.Username)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(p => p.Email)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.HasMany(p => p.Roles)
                    .WithMany(p => p.Usuarios)
                    .UsingEntity<UsuarioRoles>(
                        j => j.HasOne(pt => pt.Rol)
                              .WithMany(t => t.UsuariosRoles)
                              .HasForeignKey(pt => pt.RolId),

                        j => j.HasOne(pt => pt.Usuario)
                              .WithMany(t => t.UsuariosRoles)
                              .HasForeignKey(pt => pt.UsuarioId),

                        j => { 
                            j.HasKey(t => new { t.UsuarioId, t.RolId });
                        }
                    );
        }
    }
}
