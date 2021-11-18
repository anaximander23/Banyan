using System;
using Banyan.Navigation;
using Banyan.ServiceRegistries;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace Banyan
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder UseBanyanApp<TApp>(this MauiAppBuilder builder, Action<BanyanAppConfiguration> configureApp)
            where TApp : ApplicationCore
        {
            var appConfig = new BanyanAppConfiguration();
            configureApp?.Invoke(appConfig);

            builder.Host.UseLamar((context, services) =>
            {
                services.IncludeRegistry<NavigationRegistry>();
                services.IncludeRegistry<PopupsRegistry>();
                services.IncludeRegistry<MediatorRegistry>();

                services.AddSingleton<ApplicationCore, TApp>();
                services.AddSingleton<INavigationRoot>(s => s.GetRequiredService<ApplicationCore>());

                appConfig.ApplyConfigActions(services);
            });

            builder.UseMauiApp<TApp>(services =>
            {
                TApp app = services.GetRequiredService<TApp>();

                appConfig.ApplyAppActions(app, services);

                return app;
            });

            return builder;
        }
    }
}