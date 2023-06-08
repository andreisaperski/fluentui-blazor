using FluentUI.Demo.Hybrid.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Hybrid.MAUI;

public partial class App : Application
{
    private Window? _window;
    public App()
    {
        InitializeComponent();

        var pageTitleSyncService = MauiProgram.ServiceProvider.GetRequiredService<PageTitleSyncService>();
        pageTitleSyncService.TitleChanged =
            EventCallback.Factory.Create<string>(this, OnPageTitleChangedAsync);

        MainPage = new MainPage();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        _window = base.CreateWindow(activationState);

        return _window;
    }

    private Task OnPageTitleChangedAsync(string pageTitle)
    {
        if (_window != null)
        {
            pageTitle = !string.IsNullOrEmpty(pageTitle) ?
                pageTitle :
                "Fluent UI Blazor Components";
            _window.Title = $"{pageTitle} - Hybrid (MAUI)";
        }

        return Task.CompletedTask;
    }
}