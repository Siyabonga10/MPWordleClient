using Microsoft.Extensions.Logging;

namespace MPWordleClient
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

            builder.Services.AddLogging(configure =>
            {
                configure.AddDebug();
                configure.AddConsole();
                configure.SetMinimumLevel(LogLevel.Trace);
            });

            return builder.Build();
        }
    }
}