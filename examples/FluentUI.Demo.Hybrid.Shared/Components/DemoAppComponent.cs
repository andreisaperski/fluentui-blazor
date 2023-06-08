using FluentUI.Demo.Hybrid.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

using DemoApp = FluentUI.Demo.Shared.App;

namespace FluentUI.Demo.Hybrid.Shared.Components;

public class DemoAppComponent : ComponentBase, IDisposable
{
    private IJSObjectReference _jsModule = default!;
    private DotNetObjectReference<DemoAppComponent>? _dotNetHelper;

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;

    [Inject]
    private PageTitleSyncService TitleSyncService { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await EnsureJsModelInitializedAsync();
        _dotNetHelper = DotNetObjectReference.Create(this);
        var title = await _jsModule.InvokeAsync<string>("getTitleAndStartWatching", _dotNetHelper);

        await TitleSyncService.OnTitleChangedAsync(title);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<DemoApp>(0);
        builder.CloseComponent();
    }

    private async Task EnsureJsModelInitializedAsync()
    {
        _jsModule =
            _jsModule ??
            await JsRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./_content/FluentUI.Demo.Hybrid.Shared/js/titleWatcher.js");
    }

    [JSInvokable]
    public async Task OnTitleChanged(string title)
    {
        await TitleSyncService.OnTitleChangedAsync(title);
    }

    public void Dispose()
    {
        _dotNetHelper?.Dispose();
    }
}