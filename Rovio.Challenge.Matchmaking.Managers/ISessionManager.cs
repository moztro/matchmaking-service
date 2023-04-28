using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Managers;

public interface ISessionManager
{
    /// <summary>
    /// Gets a list of available sessions for the player's region.
    /// </summary>
    /// <returns></returns>
    List<Session> GetAvailableSessionsInPlayersRegion(Player player, Game game);
}

