using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace FluentUI.Demo.Hybrid.MAUI.Services;

public class FileBasedStaticAssetService : IStaticAssetService
{
    private readonly ILogger _logger;

    public FileBasedStaticAssetService(
        ILogger<FileBasedStaticAssetService> logger)
    {
        _logger = logger;
    }

    public async Task<string?> GetAsync(string assetUrl, bool useCache = false)
    {
        return await ReadData(assetUrl);
    }

    private async Task<string?> ReadData(string file)
    {
        try
        {
            file = file.TrimStart('.').TrimStart('/');
            await using var stream = await FileSystem.OpenAppPackageFileAsync($"wwwroot/{file}");
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, $"Failed to load asset '{file}'");
            return null;
        }
    }
}
