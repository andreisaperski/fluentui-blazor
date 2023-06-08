using System.Configuration;
using System.Data;
using System.Windows;
using FluentUI.Demo.Hybrid.Shared.Services;
using FluentUI.Demo.Shared.SampleData;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace FluentUI.Demo.Hybrid.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        Setup();

        base.OnStartup(e);
    }

    private void Setup()
    {
#if DEBUG
        var builder = WebApplication.CreateBuilder();
        StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

        var serviceCollection = builder.Services;
        serviceCollection.AddTransient<IStaticAssetService, ManifestBasedStaticAssetService>(
            sp => new ManifestBasedStaticAssetService(
                builder.Environment.WebRootFileProvider,
                sp.GetRequiredService<ILogger<ManifestBasedStaticAssetService>>()));
#else
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IStaticAssetService, FileBasedStaticAssetService>();
        serviceCollection.AddSingleton<ILoggerProvider, DebugLoggerProvider>();
#endif

        serviceCollection.AddWpfBlazorWebView();
        serviceCollection.AddFluentUIComponents(options =>
        {
            options.HostingModel = BlazorHostingModel.Hybrid;
            options.IconConfiguration = ConfigurationGenerator.GetIconConfiguration();
            options.EmojiConfiguration = ConfigurationGenerator.GetEmojiConfiguration();
        });

#if DEBUG
        serviceCollection.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        serviceCollection.AddHttpClient();
        serviceCollection.AddScoped<DataSource>();
        serviceCollection.AddSingleton<PageTitleSyncService>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
}