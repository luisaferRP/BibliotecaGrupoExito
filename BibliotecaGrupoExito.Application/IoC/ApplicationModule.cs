using Autofac;
using BibliotecaGrupoExito.Application.Services; 
using BibliotecaGrupoExito.Application.Interfaces; 

namespace BibliotecaGrupoExito.Application.IoC
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PrestamoService>().As<IPrestamoService>().InstancePerLifetimeScope();
            builder.RegisterType<MaterialService>().As<IMaterialService>().InstancePerLifetimeScope();
            builder.RegisterType<UsuarioService>().As<IUsuarioService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}