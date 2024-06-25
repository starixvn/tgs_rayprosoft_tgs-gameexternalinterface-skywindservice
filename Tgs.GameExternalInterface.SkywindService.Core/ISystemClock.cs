namespace Tgs.GameExternalInterface.SkywindService.Core;

public interface ISystemClock
{
	DateTimeOffset CurrentTime { get; }
}
