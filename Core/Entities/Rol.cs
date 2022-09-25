using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Rol : BaseEntity
    {
        public string Nombre { get; set; }

        public ICollection<Usuario> Usuarios { get; set; } = new HashSet<Usuario>();

        public ICollection<UsuarioRoles> UsuariosRoles { get; set; }
    }
}
