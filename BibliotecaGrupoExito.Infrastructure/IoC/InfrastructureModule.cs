using Autofac;
using BibliotecaGrupoExito.Domain.Interfaces;
using BibliotecaGrupoExito.Infrastructure.Data;
using BibliotecaGrupoExito.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore; // Necesario para DbContextOptions

namespace BibliotecaGrupoExito.Infrastructure.IoC
{
    public class InfrastructureModule : Module
    {
        private readonly string _connectionString;

        public InfrastructureModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Registrar DbContext
            builder.Register(context =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(_connectionString); // O .UseNpgsql para PostgreSQL
                return new ApplicationDbContext(optionsBuilder.Options);
            }).InstancePerLifetimeScope(); // Una instancia del DbContext por cada ámbito de solicitud web

            // Registrar Repositorios
            builder.RegisterType<MaterialRepository>().As<IMaterialRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UsuarioRepository>().As<IUsuarioRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PrestamoRepository>().As<IPrestamoRepository>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}