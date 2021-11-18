using System.Threading;
using System.Threading.Tasks;
using Banyan.Events;
using Banyan.Navigation;
using MediatR;

namespace BanyanDemo.Lifecycle
{
    public class ApplicationStartupHandler : INotificationHandler<ApplicationStarted>
    {
        public ApplicationStartupHandler(INavigationService navigation)
        {
            _navigation = navigation;
        }

        private readonly INavigationService _navigation;

        public async Task Handle(ApplicationStarted notification, CancellationToken cancellationToken)
        {
            await _navigation.SetMainPage<MainPage>();

            await _navigation.NavigateToMainPage();
        }
    }
}