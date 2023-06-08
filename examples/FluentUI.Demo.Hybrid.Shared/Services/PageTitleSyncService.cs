using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Hybrid.Shared.Services;

public class PageTitleSyncService
{
    public EventCallback<string> TitleChanged { get; set; }

    public async Task OnTitleChangedAsync(string title)
    {
        if (TitleChanged.HasDelegate)
        {
            await TitleChanged.InvokeAsync(title);
        }
    }
}