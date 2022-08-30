using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tgs.GameToken.Client;
using Tgs.GameExternalInterface.SkywindService.ViewModels;
using Tgs.GameExternalInterface.SkywindService.Core;
using Tgs.GameExternalInterface.SkywindService.Common.Protos;

namespace Tgs.GameExternalInterface.SkywindService.Attributes;

public class ValidateTicketAuthorizationFilter : IAsyncAuthorizationFilter
{
	private readonly ILogger<ValidateTicketAuthorizationFilter> _logger;
	private readonly IGameTokenClient _gameTokenClient;
	private readonly IGameQuery _gameQuery;

	public ValidateTicketAuthorizationFilter(
		ILogger<ValidateTicketAuthorizationFilter> logger,
		IGameTokenClient gameTokenClient,
		IGameQuery gameQuery
		)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_gameTokenClient = gameTokenClient ?? throw new ArgumentNullException(nameof(gameTokenClient));
		_gameQuery = gameQuery ?? throw new ArgumentNullException(nameof(gameQuery));
	}

	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		var request = context.HttpContext.Request;

		var pathString = request.Path;

		_logger.LogDebug("LogOn:{logOn} Path:{path}", DateTime.UtcNow.ToString("O"), pathString);

		if (!pathString.HasValue
			|| !pathString.Value.Contains("api/validate_ticket", StringComparison.InvariantCultureIgnoreCase)
			|| !pathString.Value.Contains("api/validate_terminal_ticket", StringComparison.InvariantCultureIgnoreCase))
		{
			return;
		}

		request.EnableBuffering();

		var requestViewModel = await request.ReadFromJsonAsync<ValidateTicketRequestViewModel>();

		_ = request.Body.Seek(0, SeekOrigin.Begin);

		_logger.LogDebug("LogOn:{logOn} RequestViewModel:{requestViewModel}", DateTime.UtcNow.ToString("O"), requestViewModel);

		var configuration = await _gameQuery
				.GetSkywindTransferConfigurationAsync()
				.ConfigureAwait(false);

		if (requestViewModel is not null &&
			configuration.MerchId.Equals(requestViewModel.MerchId)
			&& configuration.MerchPwd.Equals(requestViewModel.MerchPwd)
			&& await _gameTokenClient
						.CheckAuthValidAsync(
							auth: requestViewModel.Ticket,
							cancellationToken: context.HttpContext.RequestAborted)
						.ConfigureAwait(false))
		{
			return;
		}

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
