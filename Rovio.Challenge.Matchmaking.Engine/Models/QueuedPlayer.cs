using System;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Engine.Models
{
	/// <summary>
	/// Represents a player that requested to join a game, and get queued to wait for a session.
	/// </summary>
	public class QueuedPlayer
	{
		public QueuedPlayer(Player player, Game game, DateTimeOffset queuedAt)
		{
			Player = player;
			Game = game;
			QueuedAt = queuedAt;
		}

		/// <summary>
		/// The player queued to wait for a game to join.
		/// </summary>
		public Player Player { get; set; }
		/// <summary>
		/// The game which the player was queued for.
		/// </summary>
		/// <value></value>
		public Game Game { get; set; }
		/// <summary>
		/// The date/time a player requested to join a game.
		/// </summary>
		public DateTimeOffset QueuedAt { get; set; }

		public DequeuedPlayer ToDequeuedPlayer()
		{
			return new DequeuedPlayer(Player, Game, QueuedAt);
		}
	}
}

