using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.GameExternalInterface.SkywindService.Core.Exceptions;
using Tgs.GameToken.Client;
using Tgs.ServiceUtility;

namespace Tgs.GameExternalInterface.SkywindService.Core.ApplicationServices;

internal class ValidateTicketGetService : IGetService<ValidateTicketQuery, ValidateTicketResult>
{
    private readonly IGameTokenClient _gameTokenClient;
    private readonly ILogger<ValidateTicketGetService> _logger;

    public ValidateTicketGetService(
        ILogger<ValidateTicketGetService> logger,
        IGameTokenClient gameTokenClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gameTokenClient = gameTokenClient ?? throw new ArgumentNullException(nameof(gameTokenClient));
    }

    public async ValueTask<ValidateTicketResult> GetAsync(ValidateTicketQuery query, CancellationToken cancellationToken = default)
    {
        var gameAuthInfo = await _gameTokenClient.GetAuthInfoAsync(
            auth: query.Ticket,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (gameAuthInfo is null)
        {
            _logger.LogError("Authentication failed for this game({token})", query.Ticket);
            throw new PlayerNotFoundException();
        }

        var info = JsonSerializer.Deserialize<ValidateTicketInfo>(gameAuthInfo.Info);

        if (info is null)
        {
            _logger.LogError("The authentication information does not exist.({gameAuthInfo})", gameAuthInfo);
            throw new PlayerNotFoundException();
        }

        string sessionToken;
        try
        {
            sessionToken = await _gameTokenClient.CreateTokenAsync(
               playerCode: gameAuthInfo.PlayerCode,
               cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "This failed to generate sessionToken.({ playerCode})", gameAuthInfo.PlayerCode);
            throw new PlayerNotFoundException();
        }

        return new ValidateTicketResult(
            CustSessionId: sessionToken,
            CustId: info.PlayerCode,
            CurrencyCode: info.CurrencyCode,
            TestCust: info.TestCust,
            Country: info.Country,
            GameGroup: string.Empty);
    }
}
