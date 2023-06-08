using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace FluentUI.Demo.Hybrid.Shared.Services;

public class ManifestBasedStaticAssetService : IStaticAssetService
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger _logger;

    public ManifestBasedStaticAssetService(
        IFileProvider fileProvider,
        ILogger<ManifestBasedStaticAssetService> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<string?> GetAsync(string assetUrl, bool useCache = true)
    {
        try
        {
            assetUrl = assetUrl.TrimStart('.').TrimStart('/');
            var fileInfo = _fileProvider.GetFileInfo(assetUrl);
            using var reader = new StreamReader(fileInfo.PhysicalPath);
            return await reader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, $"Failed to load asset '{assetUrl}'");
            return null;
        }
    }
}
