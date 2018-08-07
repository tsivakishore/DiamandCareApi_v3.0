using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;


namespace DiamandCare.WebApi
{
    /// <summary>
    /// http://blog.ploeh.dk/2012/10/03/DependencyInjectioninASP.NETWebAPIwithCastleWindsor/
    /// </summary>
    public class RepositoriesInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // It resolves the Controllers dependencies
            container.Register(Classes.FromThisAssembly()
                .Pick().If(t => t.Name.EndsWith("Controller"))
                .Configure(configurer => configurer.Named(configurer.Implementation.Name))
                .LifestylePerWebRequest());

            container.Register(Classes.FromThisAssembly()
               .Pick().If(t => t.Name.EndsWith("Repository"))
               .Configure(configurer => configurer.Named(configurer.Implementation.Name))
               .LifestylePerWebRequest());            
        }
    }
}