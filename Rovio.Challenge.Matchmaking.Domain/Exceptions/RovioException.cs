namespace Rovio.Challenge.Matchmaking.Domain.Exceptions;

public class RovioException : Exception
{
    public RovioException(){}
    public RovioException(string message): base(message) {}
    public RovioException(string message, Exception innerException): base(message, innerException){}
}