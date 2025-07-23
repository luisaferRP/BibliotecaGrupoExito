using Autofac;
using BibliotecaGrupoExito.Application.Services; 
using BibliotecaGrupoExito.Application.Interfaces; 

namespace BibliotecaGrupoExito.Application.IoC
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Registrar servicios de aplicación
            // (IPrestamoService se definirá en el siguiente paso)
            builder.RegisterType<PrestamoService>().As<IPrestamoService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}