using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Inyección
var serverVersion = new MySqlServerVersion(new Version(8,0,28));
builder.Services.AddDbContext<TiendaContext>((options) =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion);
});
//Inyección


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

//Se ejecuta al inicio de la aplicación
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
	try
	{
		var context = services.GetRequiredService<TiendaContext>();
		await context.Database.MigrateAsync();

		//Llamada a este metodo para crear información desde un csv y la inyección y ejecución de esta
		await TiendaContextSeed.SeedAsync(context, loggerFactory);
	}
	catch (Exception ex)
	{
		var logger = loggerFactory.CreateLogger<Program>();
		logger.LogError(ex, "Ocurrió un problema durante la migración.");
	}
}
//

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
