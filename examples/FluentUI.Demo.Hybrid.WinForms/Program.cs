using FluentUI.Demo.Hybrid.Shared.Services;
using FluentUI.Demo.Shared.SampleData;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace FluentUI.Demo.Hybrid.WinForms
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Setup();

            Application.Run(new MainForm());
        }

        private static void Setup()
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

            serviceCollection.AddWindowsFormsBlazorWebView();
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
}