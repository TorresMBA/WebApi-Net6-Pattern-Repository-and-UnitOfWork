using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Producto : BaseEntity
    {
        public string? Nombre { get; set; }

        public decimal Precio { get; set; }

        public DateTime FechaCreacion { get; set; }
       
        public int MarcaId { get; set; }

        public Marca Marca { get; set; }

        public int  CategoriaId { get; set; }

        public Categoria Categoria { get; set; }
    }
}
