using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.GameExternalInterface.SkywindService.Core;

namespace Tgs.GameExternalInterface.SkywindService.Repositories;

public class GameQueryCacheRepository : IGameQuery
{
	private readonly ILogger<GameQueryCacheRepository> _logger;
	private readonly IMemoryCache _memoryCache;
	private readonly IGameQuery _gameQuery;
	private readonly GameExternalInterfaceSettingsOptions _gameExternalInterfaceSettings;

	public GameQueryCacheRepository(
		ILogger<GameQueryCacheRepository> logger,
		IOptions<GameExternalInterfaceSettingsOptions> options,
		IMemoryCache memoryCache,
		IGameQuery gameQuery)
	{
		_gameExternalInterfaceSettings = options.Value;
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
		_gameQuery = gameQuery ?? throw new ArgumentNullException(nameof(gameQuery));
	}

	public async ValueTask<Guid> GetGameProviderIdAsync(
		CancellationToken cancellationToken = default)
	{
		var key = $"gp.gameexternalinterface:skywind:{_gameExternalInterfaceSettings.GameProviderCode}";

		if (!_memoryCache.TryGetValue(key, out Guid gameProviderId))
		{
			gameProviderId = await _gameQuery.GetGameProviderIdAsync(cancellationToken).ConfigureAwait(false);

			_ = _memoryCache.Set(key, gameProviderId);

			_logger.LogInformation("LogOn:{logOn} GameProviderId:{gameProviderId}", DateTimeOffset.UtcNow.ToString("O"), gameProviderId);
		}

		return gameProviderId;
	}

	public async ValueTask<SkywindTransferConfiguration> GetSkywindTransferConfigurationAsync(
		CancellationToken cancellationToken = default)
	{
		var key = $"gp.gameexternalinterface:skywind:{_gameExternalInterfaceSettings.GameProviderConfig}";

		if (!_memoryCache.TryGetValue(key, out SkywindTransferConfiguration configuration))
		{
			configuration = await _gameQuery.GetSkywindTransferConfigurationAsync(cancellationToken).ConfigureAwait(false);

			_ = _memoryCache.Set(key, configuration, TimeSpan.FromMinutes(1));

			_logger.LogInformation("LogOn:{logOn} Configuration:{configuration}", DateTimeOffset.UtcNow.ToString("O"), JsonSerializer.Serialize(configuration));
		}

		return configuration;
	}
}
