using Core.Entities;
using CsvHelper;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;

namespace Infrastructure.Data
{
    public class TiendaContextSeed
    {
        //Este metodo lee unos archivos csv para la creación de data inicial en la base de datos
        public static async Task SeedAsync(TiendaContext context, ILoggerFactory loggerFactory)
        {
			try
			{
				var ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

				if (!context.Marca.Any())
				{
					using (var readerMarca = new StreamReader(ruta + @"/Data/Csv/marcas.csv")) 
					{
						using (var csvMarcas = new CsvReader(readerMarca, CultureInfo.InvariantCulture))
						{
							var marcas = csvMarcas.GetRecords<Marca>();
							context.Marca.AddRange(marcas);
							await context.SaveChangesAsync();
						}
					}
				}

                if (!context.Categoria.Any())
                {
                    using (var readerCategoria = new StreamReader(ruta + @"/Data/Csv/categorias.csv"))
                    {
                        using (var csvMCategoria = new CsvReader(readerCategoria, CultureInfo.InvariantCulture))
                        {
                            var categorias = csvMCategoria.GetRecords<Categoria>();
                            context.Categoria.AddRange(categorias);
                            await context.SaveChangesAsync();
                        }
                    }
                }

                if (!context.Productos.Any())
                {
                    using (var readerProductos = new StreamReader(ruta + @"/Data/Csv/productos.csv"))
                    {
                        using (var csvProductos = new CsvReader(readerProductos, CultureInfo.InvariantCulture))
                        {
                            var listadoProductosCsv = csvProductos.GetRecords<Producto>();

                            List<Producto> productos = new List<Producto>();
                            foreach (var item in listadoProductosCsv)
                            {
                                productos.Add(new Producto
                                {
                                    Id = item.Id,
                                    Nombre = item.Nombre,
                                    Precio = item.Precio,
                                    FechaCreacion = item.FechaCreacion,
                                    CategoriaId = item.CategoriaId,
                                    MarcaId = item.MarcaId

                                });
                            }

                            context.Productos.AddRange(productos);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<TiendaContext>();
				logger.LogError(ex.Message);
				throw;
			}
        }
    }
}
