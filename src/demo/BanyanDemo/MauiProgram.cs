using Banyan;
using BanyanDemo.Pages;
using Lamar;
using Microsoft.Maui.Hosting;

namespace BanyanDemo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder()
                .UseBanyanApp<App>(app =>
                {
                    app.SetMainPage<MainPage>();
                    app.ConfigureServices(ConfigureServices);
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }

        private static void ConfigureServices(ServiceRegistry services)
        {
            services.Scan(scan =>
            {
                scan.TheCallingAssembly();

                scan.AddAllTypesOf<PageModel>();
                scan.AddAllTypesOf(typeof(PageModel<>));
            });
        }
    }
}