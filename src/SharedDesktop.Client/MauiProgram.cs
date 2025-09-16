using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
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

            builder.Services.AddSingleton(sp =>
            {
                var connection  = new HubConnectionBuilder()
                    .WithUrl("http://192.168.68.101:9001/realtime/synchronization")
                    .WithAutomaticReconnect()
                    .Build();

                return connection;
            });

            builder.Services.AddSingleton<ISignalRService, SignalRService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
