using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tgs.GameExternalInterface.SkywindService.Common.Protos;
using Tgs.GameExternalInterface.SkywindService.Core;
using Tgs.GameExternalInterface.SkywindService.ViewModels;
using Tgs.GameToken.Client;

namespace Tgs.GameExternalInterface.SkywindService.Attributes;

public class ValidateTicketAuthorizationFilter(
    ILogger<ValidateTicketAuthorizationFilter> logger,
    IGameTokenClient gameTokenClient,
    IGameQuery gameQuery) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;

        var pathString = request.Path;

        logger.LogDebug("LogOn:{logOn} Path:{path}", DateTime.UtcNow.ToString("O"), pathString);

        if (!pathString.HasValue)
        {
            return;
        }

        if (!pathString.Value.Contains("api/validate_ticket", StringComparison.InvariantCultureIgnoreCase)
            && !pathString.Value.Contains("api/validate_terminal_ticket", StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        request.EnableBuffering();

        var requestViewModel = await request.ReadFromJsonAsync<ValidateTicketRequestViewModel>().ConfigureAwait(false);

        _ = request.Body.Seek(0, SeekOrigin.Begin);

        logger.LogDebug("LogOn:{logOn} RequestViewModel:{requestViewModel}", DateTime.UtcNow.ToString("O"), requestViewModel);

        var configuration = await gameQuery
                .GetSkywindTransferConfigurationAsync()
                .ConfigureAwait(false);

        if (requestViewModel is not null &&
            configuration.MerchId.Equals(requestViewModel.MerchId)
            && configuration.MerchPwd.Equals(requestViewModel.MerchPwd)
            && await gameTokenClient
            .CheckAuthValidAsync(
                auth: requestViewModel.Ticket,
                cancellationToken: context.HttpContext.RequestAborted)
            .ConfigureAwait(false))
        {
            return;
        }

        logger.LogWarning("LogOn:{logOn} Message:Auth Token verification failed. Info:RequestViewModel{requestViewModel}",
            DateTime.UtcNow.ToString("O"),
            requestViewModel);

        var model = new ValidateTicketResponseViewModel()
        {
            ErrorCode = (int)GameErrorCode.GameTokenExpired
        };

        var result = new JsonResult(value: model)
        {
            StatusCode = StatusCodes.Status200OK
        };

        context.Result = result;
    }
}
