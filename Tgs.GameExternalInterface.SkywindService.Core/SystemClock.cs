namespace Tgs.GameExternalInterface.SkywindService.Core;

internal class SystemClock : ISystemClock
{
	public DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;
}
