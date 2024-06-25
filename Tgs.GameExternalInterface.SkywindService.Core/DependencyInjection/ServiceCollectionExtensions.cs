using System.Reflection;
using Tgs.GameCommand.Client;
using Tgs.GameExternalInterface.SkywindService.Core;
using Tgs.GameToken.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameExternalInterfaceCore(
		this IServiceCollection services,
		Action<GameExternalInterfaceSettingsOptions, IServiceProvider> gameSkywindSettingsOptions,
		Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
		Action<GameTokenClientOptions, IServiceProvider> gameTokenClientOptions
		)
		=> services
			.AddOptions<GameExternalInterfaceSettingsOptions>()
			.Configure(gameSkywindSettingsOptions)
			.Services
			.RegisterAllServiceUtilityTypes(
				Assembly.GetExecutingAssembly(),
				ServiceLifetime.Transient)
			.AddGameCommandClient(gameCommandClientOptions)
			.AddGameTokenClient(gameTokenClientOptions)
			.AddSingleton<ISystemClock, SystemClock>();
}
