using System.Text.Json.Serialization;

namespace Tgs.GameExternalInterface.SkywindService.ViewModels;

public record ValidateTicketRequestViewModel
{
	[JsonPropertyName("ticket")]
	public string Ticket { get; set; } = default!;
	[JsonPropertyName("merch_id ")]
	public string MerchId { get; set; } = default!;
	[JsonPropertyName("merch_pwd")]
	public string MerchPwd { get; set; } = default!;
	[JsonPropertyName("ip")]
	public string Ip { get; set; } = default!;
}
