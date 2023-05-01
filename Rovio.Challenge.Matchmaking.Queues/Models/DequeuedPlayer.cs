using System;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Queues.Models
{
    /// <summary>
    /// If a player its being dequeued should be ready for a game
    /// </summary>
    public class DequeuedPlayer : QueuedPlayer
	{
        public DequeuedPlayer(Player player, Game game, DateTimeOffset queuedAt) : base(player, game, queuedAt)
        { 
            // when a player is dequeed, we calculate the waiting time
            // the player waited in the lobby.
            QueueingTime = DateTimeOffset.UtcNow - queuedAt;
        }

        public bool ReadyForGame { get; set; } = true;
        public TimeSpan QueueingTime { get; set; }
	}
}

