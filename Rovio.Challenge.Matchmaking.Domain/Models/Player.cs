using System;
namespace Rovio.Challenge.Matchmaking.Domain.Models
{
	/// <summary>
	/// Person who wants to join a game.
	/// </summary>
	public class Player
	{
		public Player(string username)
		{
			Username = username;
		}

		/// <summary>
		/// Player identifier.
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// Player's latency.
		/// </summary>
		public int Latency { get; set; }
	}
}

