using Banyan;
using Lamar;
using Microsoft.Maui.Hosting;

namespace BanyanDemo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();
            builder
                .UseBanyanApp<App>(app =>
                {
                    app.SetMainPage<MainPage>();
                    app.ConfigureServices(ConfigureServices);
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            return builder.Build();
        }

        private static void ConfigureServices(ServiceRegistry services)
        {
        }
    }
}