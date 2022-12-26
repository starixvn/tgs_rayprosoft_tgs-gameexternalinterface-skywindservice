using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Tgs.GameExternalInterface.SkywindService.Core;
using Tgs.GameProviders.QueryServer.Client;
using Tgs.GameToken.Client;
using Tgs.GameToken.Clients.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameExternalInterfaceCore(
        this IServiceCollection services,
        Action<GameExternalInterfaceSettingsOptions, IServiceProvider> gameSkywindSettingsOptions,
        Action<GameProvidersQueryServerClientOptions, IServiceProvider> gameProvidersQueryServerClientOptions,
        Action<GameTokenClientOptions, IServiceProvider> gameTokenClientOptions
        )
        => services
            .AddOptions<GameExternalInterfaceSettingsOptions>()
            .Configure(gameSkywindSettingsOptions)
            .Services
            .RegisterAllServiceUtilityTypes(
                Assembly.GetExecutingAssembly(),
                ServiceLifetime.Transient)
            .AddGameProvidersQueryServerClient(gameProvidersQueryServerClientOptions)
            .AddGameTokenClient(gameTokenClientOptions)
            .AddSingleton<ISystemClock, SystemClock>();
}
