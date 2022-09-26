using API.Helpers;
using API.Helpers.Errors;
using API.Service;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            //Estos nos permitira tener acceso a estos servicios desde cualquier componente (y igual los demás)
            services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            //Configuracion de appsettings.json
            services.Configure<JWT>(configuration.GetSection("JWT"));

            //Añadir autenticación - JWT
            services.AddAuthentication(
                option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
            });
        }

        public static void AddValidationsErros(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(u => u.Value.Errors)
                    .Select(u => u.ErrorMessage).ToArray();

                    var errosReponse = new ApiValidation()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errosReponse);
                };
            });
        }
    }
}
