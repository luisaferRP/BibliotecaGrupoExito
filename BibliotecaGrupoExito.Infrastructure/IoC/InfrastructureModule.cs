using Autofac;
using BibliotecaGrupoExito.Domain.Interfaces;
using BibliotecaGrupoExito.Infrastructure.Data;
using BibliotecaGrupoExito.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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
            builder.Register(context =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(_connectionString);
                return new ApplicationDbContext(optionsBuilder.Options);
            }).InstancePerLifetimeScope();

            //Register repositories
            builder.RegisterType<MaterialRepository>().As<IMaterialRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UsuarioRepository>().As<IUsuarioRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PrestamoRepository>().As<IPrestamoRepository>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}