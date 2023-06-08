using FluentUI.Demo.Shared.SampleData;
using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Fast.Components.FluentUI.Infrastructure;
using FluentUI.Demo.Hybrid.Shared.Services;

using FileBasedStaticAssetService = FluentUI.Demo.Hybrid.MAUI.Services.FileBasedStaticAssetService;

namespace FluentUI.Demo.Hybrid.MAUI;

public static class MauiProgram
{
    internal static IServiceProvider ServiceProvider { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        Setup(builder.Services);

        var app = builder.Build();

        ServiceProvider = app.Services;

        return app;
    }

    private static void Setup(IServiceCollection serviceCollection)
    {

        serviceCollection.AddSingleton<ILoggerProvider, DebugLoggerProvider>();
        serviceCollection.AddTransient<IStaticAssetService, FileBasedStaticAssetService>();

        serviceCollection.AddMauiBlazorWebView();
        serviceCollection.AddFluentUIComponents(options =>
        {
            options.HostingModel = BlazorHostingModel.Hybrid;
            options.IconConfiguration = ConfigurationGenerator.GetIconConfiguration();
            options.EmojiConfiguration = ConfigurationGenerator.GetEmojiConfiguration();
        });

#if DEBUG
        serviceCollection.AddBlazorWebViewDeveloperTools();
#endif

        serviceCollection.AddHttpClient();
        serviceCollection.AddScoped<DataSource>();
        serviceCollection.AddSingleton<PageTitleSyncService>();
    }
}