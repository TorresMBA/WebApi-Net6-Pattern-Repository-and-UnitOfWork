using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepositoy<T> where T : BaseEntity
    {
        protected readonly TiendaContext _tiendaContext;

        public GenericRepository(TiendaContext tiendaContext)
        {
            _tiendaContext = tiendaContext;
        }

        public void Add(T entity)
        {
            _tiendaContext.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _tiendaContext.Set<T>().AddRange(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _tiendaContext.Set<T>().Where(expression);
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await _tiendaContext.Set<T>().ToListAsync();
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            return await _tiendaContext.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            _tiendaContext.Set<T>().AddRange(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _tiendaContext.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _tiendaContext.Set<T>().Update(entity);
        }
    }
}
