using API.Extensions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Automapper config
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

//Se llama a los metodo de extensi�n Extensions/ApplicationServiceExtension.cs
builder.Services.ConfigureCors();
builder.Services.AddAplicacionServices();
builder.Services.AddJwt(builder.Configuration);
//

builder.Services.AddControllers();

//Inyecci�n
var serverVersion = new MySqlServerVersion(new Version(8,0,28));
builder.Services.AddDbContext<TiendaContext>((options) =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion);
});
//Inyecci�n


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Se ejecuta al inicio de la aplicaci�n
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
	try
	{
		var context = services.GetRequiredService<TiendaContext>();
		await context.Database.MigrateAsync();

		//Llamada a este metodo para crear informaci�n desde un csv y la inyecci�n y ejecuci�n de esta
		await TiendaContextSeed.SeedAsync(context, loggerFactory);
		await TiendaContextSeed.SeedRolesAsync(context, loggerFactory);
	}
	catch (Exception ex)
	{
		var logger = loggerFactory.CreateLogger<Program>();
		logger.LogError(ex, "Ocurri� un problema durante la migraci�n.");
	}
}
//

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

//Middwlare de autenticaci�n y siempre debe ir antes el de authorizacion (vino por defecto en el program el authorizacion)
app.UseAuthentication();
// 

app.UseAuthorization();

app.MapControllers();

app.Run();
