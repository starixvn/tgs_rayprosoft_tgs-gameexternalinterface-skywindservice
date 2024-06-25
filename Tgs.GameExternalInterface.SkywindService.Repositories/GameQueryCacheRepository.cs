using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Tgs.GameExternalInterface.SkywindService.Core;

namespace Tgs.GameExternalInterface.SkywindService.Repositories;

public class GameQueryCacheRepository(
	IOptions<GameExternalInterfaceSettingsOptions> options,
	IMemoryCache memoryCache,
	IGameQuery gameQuery) : IGameQuery
{
	private readonly GameExternalInterfaceSettingsOptions _gameExternalInterfaceSettings = options.Value;

	public async ValueTask<Guid> GetGameProviderIdAsync(
		CancellationToken cancellationToken = default)
	{
		var key = $"gp.gameexternalinterface:skywind:{_gameExternalInterfaceSettings.GameProviderCode}";

		if (!memoryCache.TryGetValue(key, out Guid gameProviderId))
		{
			gameProviderId = await gameQuery.GetGameProviderIdAsync(cancellationToken).ConfigureAwait(false);

			return memoryCache
				.Set(
				key: key,
				value: gameProviderId);
		}

		return gameProviderId;
	}

	public async ValueTask<SkywindTransferConfiguration> GetSkywindTransferConfigurationAsync(
		CancellationToken cancellationToken = default)
	{
		var key = $"gp.gameexternalinterface:skywind:{_gameExternalInterfaceSettings.GameProviderConfig}";

		if (!memoryCache.TryGetValue(key, out SkywindTransferConfiguration? configuration))
		{
			configuration = await gameQuery.GetSkywindTransferConfigurationAsync(cancellationToken).ConfigureAwait(false);

			return memoryCache
				.Set(
				key: key,
				value: configuration,
				absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(1));
		}

		return configuration!;
	}
}
