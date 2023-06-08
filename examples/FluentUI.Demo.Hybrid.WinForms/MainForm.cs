using FluentUI.Demo.Hybrid.Shared.Components;
using FluentUI.Demo.Hybrid.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;

namespace FluentUI.Demo.Hybrid.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var pageTitleSyncService = Program.ServiceProvider.GetRequiredService<PageTitleSyncService>();
            pageTitleSyncService.TitleChanged =
                EventCallback.Factory.Create<string>(this, OnPageTitleChangedAsync);

            blazorWebView.HostPage = "wwwroot\\index.html";
            blazorWebView.Services = Program.ServiceProvider;
            blazorWebView.RootComponents.Add<DemoAppComponent>("#app");
            blazorWebView.RootComponents.Add<HeadOutlet>("head::after");
        }

        private Task OnPageTitleChangedAsync(string pageTitle)
        {
            pageTitle = !string.IsNullOrEmpty(pageTitle) ?
                pageTitle :
                "Fluent UI Blazor Components";
            Text = $"{pageTitle} - Hybrid (WinForms)";

            return Task.CompletedTask;
        }
    }
}
