using API.Service;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        //Este es un metodo de extensión y es estatico, lo que hace diferente de otros metodos estaticos es que recibe this como primer parametro
        //this -> representa el tipo de dato del objeto que extenderemos en este caso será IServiceCollection
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin() // WithOrigins("https://dominio.com")
                    .AllowAnyMethod()        // WithMethod("GET", "POST")
                    .AllowAnyHeader();       // WithHeader("accept", "content-type")
                });
            });
        }

        public static void AddAplicacionServices(this IServiceCollection services)
        {
            //En caso que utilice un repositorio generico que no este en el unit of work descomentar esta linea *
            //*services.AddScoped(typeof(IGenericRepositoy<>), typeof(GenericRepository<>));

            //services.AddScoped<IProductoRepository, ProductoRepository>();
            //services.AddScoped<IMarcaRepository, MarcaRepository>();
            //services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
