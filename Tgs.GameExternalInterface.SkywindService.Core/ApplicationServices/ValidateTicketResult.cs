namespace Tgs.GameExternalInterface.SkywindService.Core.ApplicationServices;
public record ValidateTicketResult(
	string CustSessionId,
	string CustId,
	string CurrencyCode,
	bool TestCust,
	string Country,
	string GameGroup);
