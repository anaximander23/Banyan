using System;
using System.Diagnostics;
using Banyan.Events;
using Banyan.Navigation;
using MediatR;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Banyan
{
    [DebuggerDisplay("App {AppInstanceId}")]
    public class ApplicationCore : Application, INavigationRoot
    {
        public ApplicationCore(IMediator mediator)
        {
            AppInstanceId = Guid.NewGuid().ToString();

            _mediator = mediator;
        }

        internal readonly IMediator _mediator;

        INavigation INavigationRoot.Navigation { get; set; }
        internal Page InitialPage { get; set; }
        private string AppInstanceId { get; }

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

        protected override Window CreateWindow(IActivationState activationState)
        {
            NavigationPage navigationPage = new NavigationPage(InitialPage);

            (this as INavigationRoot).Navigation = navigationPage.Navigation;

            return new Window(navigationPage);
        }
    }
}