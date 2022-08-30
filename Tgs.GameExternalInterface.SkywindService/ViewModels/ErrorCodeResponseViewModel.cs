using System.Text.Json.Serialization;

namespace Tgs.GameExternalInterface.SkywindService.ViewModels;

public class ErrorCodeResponseViewModel
{
	[JsonPropertyName("error_code")]
	public int ErrorCode { get; set; }
}
