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
    }
}
