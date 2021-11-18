using Lamar;

namespace Banyan
{
    internal class AppServiceRegistry : ServiceRegistry
    {
        public AppServiceRegistry()
        {
        }

        protected void ScanForAppNavigationElements()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();

                scan.ConnectImplementationsToTypesClosing(typeof(PageModel<>));
            });
        }
    }
}