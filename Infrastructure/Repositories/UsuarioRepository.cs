using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(TiendaContext tiendaContext) : base(tiendaContext)
        {
        }

        public async Task<Usuario> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _tiendaContext.Usuarios
                                            .Include(u => u.Roles)
                                            .Include(u => u.RefreshTokens)
                                            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
        }

        public async Task<Usuario> GetByUsernameAsync(string username)
        {
            return await _tiendaContext.Usuarios
                                            .Include(u => u.Roles)
                                            .Include(u => u.RefreshTokens)
                                            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }
    }
}
