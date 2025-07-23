using BibliotecaGrupoExito.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;

namespace BibliotecaGrupoExito.Tests.Integration.Configuration
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        // Genera un nombre de base de datos único para cada instancia de la fábrica.
        // Esto garantiza que cada grupo de pruebas (o cada TestClass) tenga su propia DB en memoria aislada.
        private readonly string _databaseName = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Eliminar cualquier configuración existente del DbContext de la aplicación real
                // si existe, para reemplazarla con la de memoria.
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Añadir un DbContext en memoria para las pruebas, usando el nombre único.
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName); // ¡Usamos el nombre único aquí!
                });

                // Construir el ServiceProvider temporalmente para acceder al DbContext
                // y asegurar que la base de datos en memoria se inicialice limpia.
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

                    // Asegurarse de que la base de datos se cree limpia para cada prueba
                    // (Aunque _databaseName ya es único, EnsureDeleted/Created es buena práctica para claridad).
                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.EnsureCreated();

                    // Opcional: Sembrar datos de prueba si son necesarios para todas las pruebas de integración
                    // Esto se puede hacer aquí o en cada test individual (SeedDataAsync en PrestamosControllerTests)
                    // Prefiero sembrar por test para un mayor aislamiento.
                }
            });
        }
    }
}