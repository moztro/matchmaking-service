using System;
using System.ComponentModel.DataAnnotations;
using Rovio.Challenge.Matchmaking.Domain.Enums;

namespace Rovio.Challenge.Matchmaking.Domain.Models
{
	/// <summary>
	/// Person who wants to join a game.
	/// </summary>
	public class Player
	{
		public Player() { }
		public Player(string username)
		{
			Username = username;
		}

		/// <summary>
		/// Player identifier.
		/// </summary>
		[Key]
		[MaxLength(25)]
		public string Username { get; set; }
		/// <summary>
		/// Player's latency.
		/// </summary>
		public int Latency { get; set; }
		/// <summary>
		/// The geographical region to which the players belongs to.
		/// </summary>
		public Region Region { get; set; }
	}
}

