using Tgs.GameExternalInterface.SkywindService.Core;
using Tgs.GameProviders.QueryServer.Client;
using Tgs.GameToken.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameExternalInterfaceService(
        this IServiceCollection services,
        Action<GameExternalInterfaceSettingsOptions, IServiceProvider> gameIdnLiveOptions,
        Action<GameProvidersQueryServerClientOptions, IServiceProvider> gameProvidersQueryServerClientOptions,
        Action<GameTokenClientOptions, IServiceProvider> gameTokenClientOptions
        )
        => services
            .AddMemoryCache()
            .AddGameExternalInterfaceCore(
                gameIdnLiveOptions,
                gameProvidersQueryServerClientOptions,
                gameTokenClientOptions)
            .AddGameExternalInterfaceRepository();
}
