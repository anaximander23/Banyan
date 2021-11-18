using Banyan.Navigation;
using Lamar;

namespace Banyan.ServiceRegistries
{
    internal class NavigationRegistry : ServiceRegistry
    {
        public NavigationRegistry()
        {
            For<INavigationRoot>().Use(c => c.GetInstance<ApplicationCore>());
            For<IPageFactory>().Use<LamarPageFactory>().Singleton();
            For<INavigationService>().Use<NavigationService>().Singleton();

            Scan(scan =>
            {
                scan.TheCallingAssembly();

                scan.ConnectImplementationsToTypesClosing(typeof(PageModel<>));
            });
        }
    }
}