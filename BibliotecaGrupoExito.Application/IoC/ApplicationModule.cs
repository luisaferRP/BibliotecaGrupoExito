using Autofac;
using BibliotecaGrupoExito.Application.Services; // Asumiendo que crearás PrestamoService aquí
using BibliotecaGrupoExito.Application.Interfaces; // Asumiendo que crearás IPrestamoService aquí

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