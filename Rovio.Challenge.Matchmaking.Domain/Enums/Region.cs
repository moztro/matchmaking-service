using System;
namespace Rovio.Challenge.Matchmaking.Domain.Enums
{
    /// <summary>
    /// Represents a particular region in the world.
    ///
    /// Limiting to 4 regions now, but can be extended as per https://str.com/resourcesglossary/world-regions.
    /// </summary>
    public enum Region
	{
		Americas,
		Europe,
		Asia,
		Africa
	}
}

