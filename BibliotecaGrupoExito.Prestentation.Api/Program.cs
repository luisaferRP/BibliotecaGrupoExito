using Autofac;
using Autofac.Extensions.DependencyInjection;
using BibliotecaGrupoExito.Application.IoC; // Para ApplicationModule
using BibliotecaGrupoExito.Infrastructure.IoC; // Para InfrastructureModule
using BibliotecaGrupoExito.Infrastructure.Data; // Para DbContext
using Microsoft.EntityFrameworkCore; // Para UseSqlServer

var builder = WebApplication.CreateBuilder(args);

// Configurar Autofac como ServiceProvider
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Configurar el contenedor de Autofac
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Obtener la cadena de conexión de appsettings.json
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("DefaultConnection string is not configured.");
    }

    // Cargar los módulos de Autofac para cada capa
    containerBuilder.RegisterModule(new InfrastructureModule(connectionString));
    containerBuilder.RegisterModule(new ApplicationModule());
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer(); // Descomentar si usas Minimal APIs o quieres explorar los endpoints
 builder.Services.AddSwaggerGen(); // Descomentar si usas Swagger/OpenAPI

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Opcional: Aplicar migraciones al iniciar la aplicación (solo para desarrollo/pruebas)
// Considera herramientas como DbUp para producción para migraciones más robustas.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Aplica las migraciones pendientes
}

app.Run();