using System.Windows;
using FluentUI.Demo.Hybrid.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Hybrid.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Resources.Add("services", App.ServiceProvider);

            var pageTitleSyncService = App.ServiceProvider.GetRequiredService<PageTitleSyncService>();
            pageTitleSyncService.TitleChanged =
                EventCallback.Factory.Create<string>(this, OnPageTitleChangedAsync);
        }

        private Task OnPageTitleChangedAsync(string pageTitle)
        {
            pageTitle = !string.IsNullOrEmpty(pageTitle) ?
                pageTitle :
                "Fluent UI Blazor Components";
            Title = $"{pageTitle} - Hybrid (WPF)";

            return Task.CompletedTask;
        }
    }
}