using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UsuarioRoles
    {
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public int RolId { get; set; }

        public Rol Rol { get; set; }
    }
}
