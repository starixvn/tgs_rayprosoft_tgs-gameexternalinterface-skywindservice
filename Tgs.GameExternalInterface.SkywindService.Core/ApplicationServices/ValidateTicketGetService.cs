using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.GameExternalInterface.SkywindService.Core.Exceptions;
using Tgs.GameToken.Client;
using Tgs.ServiceUtility;

namespace Tgs.GameExternalInterface.SkywindService.Core.ApplicationServices;

internal class ValidateTicketGetService(
    ILogger<ValidateTicketGetService> logger,
    IGameTokenClient gameTokenClient) : IGetService<ValidateTicketQuery, ValidateTicketResult>
{
    public async ValueTask<ValidateTicketResult> GetAsync(ValidateTicketQuery query, CancellationToken cancellationToken = default)
    {
        var gameAuthInfo = await gameTokenClient.GetAuthInfoAsync(
            auth: query.Ticket,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (gameAuthInfo is null)
        {
            logger.LogError("Authentication failed for this game({token})", query.Ticket);
            throw new PlayerNotFoundException();
        }

        var info = JsonSerializer.Deserialize<ValidateTicketInfo>(gameAuthInfo.Info);

        if (info is null)
        {
            logger.LogError("The authentication information does not exist.({gameAuthInfo})", gameAuthInfo);
            throw new PlayerNotFoundException();
        }

        string sessionToken;
        try
        {
            sessionToken = await gameTokenClient
                .CreateTokenAsync(
                playerCode: gameAuthInfo.PlayerCode,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "This failed to generate sessionToken.({ playerCode})", gameAuthInfo.PlayerCode);
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
