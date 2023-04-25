using System;
namespace Rovio.Challenge.Matchmaking.Domain.Models
{
    /// <summary>
    /// A videogame (bad piggies, angry birds, etc.)
    ///
    /// Objects created from this Game class assumes that:
    /// 1. The game supports online multiplayer.
    /// 2. Its available to all regions under Rovio.Challenge.Matchmaking.Domain.Enums.Region enumeration.
    /// </summary>
    public class Game
	{
		public string Name { get; set; }
	}
}

