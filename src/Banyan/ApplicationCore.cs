using Banyan.Events;
using Banyan.Navigation;
using MediatR;
using Microsoft.Maui.Controls;

namespace Banyan
{
    public class ApplicationCore : Application, INavigationRoot
    {
        public ApplicationCore(IMediator mediator)
        {
            _mediator = mediator;
        }

        internal IMediator _mediator;

        INavigation INavigationRoot.Navigation { get; set; }

        Page INavigationRoot.MainPage
        {
            get => MainPage;
            set => MainPage = value;
        }

        protected override void OnStart()
        {
            _mediator.Publish(new ApplicationStarted());
        }

        protected override void OnSleep()
        {
            _mediator.Publish(new ApplicationEnteredSleep());
        }

        protected override void OnResume()
        {
            _mediator.Publish(new ApplicationResumed());
        }
    }
}