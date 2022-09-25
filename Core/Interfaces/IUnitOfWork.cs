using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IProductoRepository Producto { get; }

        IMarcaRepository Marca { get; }

        ICategoriaRepository Categoria { get; }

        IRolRepository Rol { get; }

        IUsuarioRepository Usuario { get; }

        Task<int> SaveAsync();
    }
}
