using System.Collections.Concurrent;
using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Queues.Models;

namespace Rovio.Challenge.Matchmaking.Queues;

/// <summary>
/// The waiting queue to which players are queued while waiting for a session.
/// Queue is per game.
/// </summary>
public class BaseQueue<T> where T : Game
{
    public ConcurrentQueue<QueuedPlayer> Queue { get; set; } = new ConcurrentQueue<QueuedPlayer>();

    /// <summary>
    /// Add a player to the queue to wait for a game.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="game"></param>
    public virtual void QueuePlayer(Player player, T game)
    {
        Queue.Enqueue(new QueuedPlayer(player, game, DateTimeOffset.UtcNow));
    }

    /// <summary>
    /// Dequeues players that are ready for a game.
    /// </summary>
    /// <returns></returns>
    public virtual DequeuedPlayer DequeuePlayer()
    {
        if (Queue.TryDequeue(out QueuedPlayer player))
            return player.ToDequeuedPlayer();
        return null;
    }

    public virtual DequeuedPlayer PeekPlayer()
    {
        if (Queue.TryPeek(out QueuedPlayer player))
            return player.ToDequeuedPlayer();
        return null;
    }

    /// <summary>
    /// Get a player if it does exists in the queue, null if does not.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public virtual Player GetPlayerFromQueue(string username)
    {
        var dequeued = Queue.FirstOrDefault(q => q.Player.Username == username);
        return dequeued?.Player;
    }
}

