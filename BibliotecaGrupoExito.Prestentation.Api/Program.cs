using Autofac;
using Autofac.Extensions.DependencyInjection;
using BibliotecaGrupoExito.Application.IoC; 
using BibliotecaGrupoExito.Infrastructure.IoC; 
using BibliotecaGrupoExito.Infrastructure.Data; 
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Configurar Autofac como ServiceProvider
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Configurar el contenedor de Autofac
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Biblioteca Grupo Éxito API",
        Version = "v1",
        Description = "API para la gestión de materiales, usuarios y préstamos de la biblioteca del Grupo Éxito."
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca Grupo Éxito API v1");
        options.RoutePrefix = string.Empty;
    });
}

    app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
}

app.Run();

public partial class Program { }