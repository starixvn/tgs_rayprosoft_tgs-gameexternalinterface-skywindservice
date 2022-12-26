namespace Tgs.GameExternalInterface.SkywindService.Core.Exceptions;

public class PlayerNotFoundException : Exception
{
    public PlayerNotFoundException() : base("Game is not found or disabled.")
    {
    }

    public PlayerNotFoundException(string message) : base(message)
    {
    }
}
