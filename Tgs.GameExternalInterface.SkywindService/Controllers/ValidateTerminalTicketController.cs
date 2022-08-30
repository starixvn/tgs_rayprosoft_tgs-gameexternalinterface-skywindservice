using Microsoft.AspNetCore.Mvc;
using Tgs.GameExternalInterface.SkywindService.Common.Protos;
using Tgs.GameExternalInterface.SkywindService.Core.ApplicationServices;
using Tgs.GameExternalInterface.SkywindService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameExternalInterface.SkywindService.Controllers;

[Route("api/validate_terminal_ticket")]
[ApiController]
public class ValidateTerminalTicketController : ControllerBase
{
	[HttpPost]
	public async ValueTask<ValidateTicketResponseViewModel> AuthAsync(
		[FromServices] IGetService<ValidateTicketQuery, ValidateTicketResult> service,
		[FromBody] ValidateTicketRequestViewModel model)
	{
		var query = new ValidateTicketQuery(model.Ticket);
		var result = await service.GetAsync(query).ConfigureAwait(false);

		return new ValidateTicketResponseViewModel()
		{
			ErrorCode = (int)GameErrorCode.NoErrors,
			CustSessionId = result.CustSessionId,
			CustId = result.CustId,
			CurrencyCode = result.CurrencyCode,
			TestCust = result.TestCust,
			Country = result.Country,
			GameGroup = result.GameGroup
		};
	}
}
