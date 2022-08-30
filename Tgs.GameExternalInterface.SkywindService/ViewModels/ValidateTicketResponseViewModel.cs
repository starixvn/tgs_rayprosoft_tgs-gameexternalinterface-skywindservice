using System.Text.Json.Serialization;

namespace Tgs.GameExternalInterface.SkywindService.ViewModels;

public record ValidateTicketResponseViewModel
{
	[JsonPropertyName("error_code")]
	public int ErrorCode { get; set; }
	[JsonPropertyName("cust_session_id")]
	public string CustSessionId { get; set; } = default!;
	[JsonPropertyName("cust_id")]
	public string CustId { get; set; } = default!;
	[JsonPropertyName("currency_code")]
	public string CurrencyCode { get; set; } = default!;
	[JsonPropertyName("test_cust")]
	public bool TestCust { get; set; }
	[JsonPropertyName("country")]
	public string Country { get; set; } = default!;
	[JsonPropertyName("game_group")]
	public string GameGroup { get; set; } = default!;
}