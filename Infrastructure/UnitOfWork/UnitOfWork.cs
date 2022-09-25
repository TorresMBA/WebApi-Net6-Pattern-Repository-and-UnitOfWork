using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly TiendaContext _context;

        private IProductoRepository _productos;

        private IMarcaRepository _marcas;

        private ICategoriaRepository _categoria;

        private IRolRepository _roles;

        private IUsuarioRepository _usuarios;

        public IProductoRepository Producto
        {
            get
            {
                if (_productos == null)
                {
                    _productos = new ProductoRepository(_context);
                }
                return _productos;
            }
        }

        public IMarcaRepository Marca
        {
            get
            {
                if (_marcas == null)
                {
                    _marcas = new MarcaRepository(_context);
                }
                return _marcas;
            }
        }

        public ICategoriaRepository Categoria
        {
            get
            {
                if (_categoria == null)
                {
                    _categoria = new CategoriaRepository(_context);
                }
                return _categoria;
            }
        }

        public IUsuarioRepository Usuario
        {
            get
            {
                if (_usuarios == null)
                {
                    _usuarios = new UsuarioRepository(_context);
                }
                return _usuarios;
            }
        }

        public IRolRepository Rol
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new RolRepository(_context);
                }
                return _roles;
            }
        }

        public UnitOfWork(TiendaContext context)
        {
            _context = context;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
