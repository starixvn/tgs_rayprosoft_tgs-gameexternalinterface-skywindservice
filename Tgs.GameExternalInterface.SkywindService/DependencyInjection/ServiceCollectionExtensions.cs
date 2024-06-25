using Tgs.GameCommand.Client;
using Tgs.GameExternalInterface.SkywindService.Core;
using Tgs.GameToken.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameExternalInterfaceService(
		this IServiceCollection services,
		Action<GameExternalInterfaceSettingsOptions, IServiceProvider> gameIdnLiveOptions,
		Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
		Action<GameTokenClientOptions, IServiceProvider> gameTokenClientOptions
		)
		=> services
			.AddMemoryCache()
			.AddGameExternalInterfaceCore(
				gameSkywindSettingsOptions: gameIdnLiveOptions,
				gameCommandClientOptions: gameCommandClientOptions,
				gameTokenClientOptions: gameTokenClientOptions)
			.AddGameExternalInterfaceRepository();
}
