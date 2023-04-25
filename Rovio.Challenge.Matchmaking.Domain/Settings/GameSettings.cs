using System;
namespace Rovio.Challenge.Matchmaking.Domain.Settings
{
	/// <summary>
	/// Determine the game session settings such as min or max players.
	/// </summary>
	public class GameSessionSettings
	{
		public int MinPlayers { get; set; }
		public int MaxPlayers { get; set; }
	}
}

