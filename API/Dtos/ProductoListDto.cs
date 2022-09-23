
namespace API.Dtos
{
    public class ProductoListDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public decimal Precio { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int MarcaId { get; set; }

        public string Marca { get; set; }

        public int CategoriaId { get; set; }

        public string Categoria { get; set; }
    }
}
