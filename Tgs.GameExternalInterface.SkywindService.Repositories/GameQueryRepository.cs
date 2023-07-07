using Microsoft.Extensions.Options;
using Tgs.GameCommand.Client;
using Tgs.GameExternalInterface.SkywindService.Core;

namespace Tgs.GameExternalInterface.SkywindService.Repositories;

internal class GameQueryRepository : IGameQuery
{
    private readonly IGameCommandClient _client;
    private readonly GameExternalInterfaceSettingsOptions _gameExternalInterfaceSettings;

    public GameQueryRepository(
        IOptions<GameExternalInterfaceSettingsOptions> options,
        IGameCommandClient client
        )
    {
        _gameExternalInterfaceSettings = options.Value;
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<Guid> GetGameProviderIdAsync(
        CancellationToken cancellationToken = default)
        => await _client
        .GameProviderGetGameProviderIdAsync(
            gameProviderCode: _gameExternalInterfaceSettings.GameProviderCode,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false)
        ?? throw new Exception($"Game Provider does not exist.({_gameExternalInterfaceSettings.GameProviderCode})");

    public async ValueTask<SkywindTransferConfiguration> GetSkywindTransferConfigurationAsync(
        CancellationToken cancellationToken = default)
        => await _client
        .SettingGetConfigurationAsync<SkywindTransferConfiguration>(
            key: _gameExternalInterfaceSettings.GameProviderConfig,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false)
        ?? throw new Exception($"This game product setting does not exist.({_gameExternalInterfaceSettings.GameProviderConfig})");
}
