using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Queues.Models;

public class ReadySession
{
    /// <summary>
    /// The original session.
    /// </summary>
    /// <value></value>
    public Session Session { get; set; }

    /// <summary>
    /// Players who already joined the session.
    /// </summary>
    /// <typeparam name="DequeuedPlayer"></typeparam>
    /// <returns></returns>
    public List<Player> Players { get; set; } = new List<Player>();
}