using Tgs.GameExternalInterface.SkywindService.Core;

namespace Tgs.GameExternalInterface.SkywindService.DependencyInjection;

public static class CacheRegisterExtensions
{
	public static async Task<WebApplication> UseCacheRegister(this WebApplication app)
	{
		using (var scope = app.Services.CreateScope())
		{
			var sp = scope.ServiceProvider;
			var query = scope.ServiceProvider.GetRequiredService<IGameQuery>();
			_ = await query.GetGameProviderIdAsync().ConfigureAwait(false);
			_ = await query.GetSkywindTransferConfigurationAsync().ConfigureAwait(false);
		}

		return app;
	}
}
