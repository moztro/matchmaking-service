namespace Rovio.Challenge.Matchmaking.Domain.Exceptions;

public class SessionNotFoundException : RovioException
{
    private const string message = "Could not find a suitable session to join";

    public SessionNotFoundException(): base(message) {}
    public SessionNotFoundException(Exception innerException): base(message, innerException) {}
}