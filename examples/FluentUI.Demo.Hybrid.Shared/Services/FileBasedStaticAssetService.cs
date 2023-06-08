using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace FluentUI.Demo.Hybrid.Shared.Services;

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
        string? result = null;
        if (string.IsNullOrEmpty(result))
        {
            result = await ReadData(assetUrl);
        }
        return result;
    }

    private async Task<string?> ReadData(string file)
    {
        try
        {
            await using var stream = File.OpenRead($"wwwroot/{file}");
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,$"Failed to load asset '{file}'");
            return null;
        }
    }
}