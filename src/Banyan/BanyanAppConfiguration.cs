using System;
using System.Collections.Generic;
using Banyan.Navigation;
using Lamar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace Banyan
{
    public class BanyanAppConfiguration
    {
        public BanyanAppConfiguration()
        {
            _configActions = new List<Action<ServiceRegistry>>();
            _appActions = new List<Action<ApplicationCore, IServiceProvider>>();
        }

        private readonly List<Action<ServiceRegistry>> _configActions;
        private readonly List<Action<ApplicationCore, IServiceProvider>> _appActions;

        public void ConfigureServices(Action<ServiceRegistry> configureServices)
        {
            _configActions.Add((services) => configureServices?.Invoke(services));
        }

        public void SetMainPage<TPage>() where TPage : Page
        {
            _appActions.Add(SetMainPageFunc<TPage>);
        }

        internal void ApplyConfigActions(ServiceRegistry services)
        {
            foreach (Action<ServiceRegistry> action in _configActions)
            {
                action?.Invoke(services);
            }
        }

        internal void ApplyAppActions(ApplicationCore app, IServiceProvider services)
        {
            foreach (Action<ApplicationCore, IServiceProvider> action in _appActions)
            {
                action.Invoke(app, services);
            }
        }

        private void SetMainPageFunc<TPage>(ApplicationCore app, IServiceProvider services)
            where TPage : Page
        {
            IPageFactory pageFactory = services.GetRequiredService<IPageFactory>();
            System.Threading.Tasks.Task<Page> mainPageCreation = pageFactory.CreatePage<TPage>();

            mainPageCreation.Wait();

            if (mainPageCreation.IsCompletedSuccessfully)
            {
                app.InitialPage = mainPageCreation.Result;
            }
        }
    }
}