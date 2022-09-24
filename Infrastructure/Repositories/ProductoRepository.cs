using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(TiendaContext context) : base(context) {  }

        public async Task<IEnumerable<Producto>> GetProductosMasCaros(int cantidad) =>
            await _tiendaContext.Productos
                    .OrderByDescending(p => p.Precio)
                    .Take(cantidad)
                    .ToListAsync();

        public override async Task<Producto> GetByIdAsync(int id)
        {
            return await _tiendaContext.Productos
                        .Include(p => p.Marca)
                        .Include(p => p.Categoria)
                        .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _tiendaContext.Productos
                        .Include(p => p.Marca)
                        .Include(p => p.Categoria)
                        .ToListAsync();
        }

        public virtual async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var consulta = _tiendaContext.Productos as IQueryable<Producto>;

            if (!string.IsNullOrEmpty(search))
            {
                consulta = consulta.Where(p => p.Nombre.ToLower().Contains(search));
            }

            var totalRegistros = await consulta.CountAsync();

            var registros = await consulta
                                .Include(p => p.Marca)
                                .Include(p => p.Categoria)  
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return (totalRegistros, registros);
        }
    }
}
