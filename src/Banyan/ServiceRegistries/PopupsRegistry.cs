using Banyan.Popups;
using Lamar;

namespace Banyan.ServiceRegistries
{
    internal class PopupsRegistry : ServiceRegistry
    {
        public PopupsRegistry()
        {
            For<IPopupService>().Use<PopupService>().Singleton();
        }
    }
}