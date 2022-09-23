using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;

namespace Infrastructure.Data
{
    public class TiendaContext : DbContext
    {
        public TiendaContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Marca> Marca { get; set; }

        public DbSet<Categoria> Categoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Este Metodo -> ApplyConfigurationsFromAssembly va a aplicar todas las configuraciónes de todas las clases de Tipo IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
