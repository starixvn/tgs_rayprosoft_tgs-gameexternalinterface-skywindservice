using Tgs.GameExternalInterface.SkywindService.Core;
using Tgs.GameExternalInterface.SkywindService.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameExternalInterfaceRepository(this IServiceCollection services)
		=> services
			.AddTransient<IGameQuery, GameQueryRepository>()
			.Decorate<IGameQuery>(
				(innerRepository, sp) =>
					ActivatorUtilities.CreateInstance<GameQueryCacheRepository>(sp, innerRepository)
			);
}
