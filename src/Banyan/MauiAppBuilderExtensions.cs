using System;
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace Banyan
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder UseBanyan(this MauiAppBuilder builder)
            => builder.UseBanyan(null);

        public static MauiAppBuilder UseBanyan(this MauiAppBuilder builder, Action<ServiceRegistry> configureServices)
        {
            builder.Host.UseLamar((context, services) =>
            {
                configureServices?.Invoke(services);
            });

            return builder;
        }
    }
}