namespace Tgs.GameExternalInterface.SkywindService.Core;

public interface IGameQuery
{
	public ValueTask<Guid> GetGameProviderIdAsync(
		CancellationToken cancellationToken = default);

	public ValueTask<SkywindTransferConfiguration> GetSkywindTransferConfigurationAsync(
		CancellationToken cancellationToken = default);
}
