using Microsoft.Extensions.Logging;
using SharedDesktop.Client.Services.Discovery;
using SharedDesktop.Client.Services.RealTime;

namespace SharedDesktop.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ViewModels.MainViewModel>();

            builder.Services.AddSingleton<IServiceDiscovery, ServiceDiscovery>();
            builder.Services.AddSingleton<ISignalRService, SignalRService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
