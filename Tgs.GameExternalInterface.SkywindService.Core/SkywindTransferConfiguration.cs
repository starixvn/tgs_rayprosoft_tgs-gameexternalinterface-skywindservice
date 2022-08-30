namespace Tgs.GameExternalInterface.SkywindService.Core;
public record SkywindTransferConfiguration
{
	public Uri ApiUrl { get; init; } = default!;
	public Uri LiveLobbyUrl { get; init; } = default!;
	public string MerchId { get; init; } = default!;
	public string MerchPwd { get; init; } = default!;
	public string SecretKey { get; init; } = default!;
	public string Username { get; init; } = default!;
	public string Password { get; init; } = default!;
}
