namespace Tgs.GameExternalInterface.SkywindService.Core;
public record ValidateTicketInfo
{
    public string PlayerCode { get; set; } = default!;
    public string CurrencyCode { get; set; } = default!;
    public bool TestCust { get; set; } = default!;
    public string Country { get; set; } = default!;
}
