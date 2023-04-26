using System;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Engine.Models
{
    /// <summary>
    /// If a player its being dequeued should be ready for a game
    /// </summary>
    public class DequeuedPlayer : QueuedPlayer
	{
        public DequeuedPlayer(Player player, Game game, DateTimeOffset queuedAt) : base(player, game, queuedAt)
        { }

        public bool ReadyForGame { get; set; } = true;
	}
}

