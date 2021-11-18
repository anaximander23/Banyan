using System.Threading.Tasks;
using Banyan.Navigation;
using Lamar;
using Microsoft.Maui.Controls;

namespace Banyan
{
    public abstract class ApplicationStartup<TApp, TMainPage>
        where TApp : ApplicationCore
        where TMainPage : Page
    {
        public abstract void ConfigureServices(ServiceRegistry services);

        public abstract Task InitialiseNavigation(INavigationService navigationService);
    }
}