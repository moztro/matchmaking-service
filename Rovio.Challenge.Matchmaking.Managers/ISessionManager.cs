using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Managers;

public interface ISessionManager
{
    /// <summary>
    /// Gets a list of available sessions for the player's region.
    /// </summary>
    /// <returns></returns>
    List<Session> GetAvailableSessionsInPlayersRegion(Player player, Game game);

    /// <summary>
    /// Determines if a session is ready for a player to join based on game settings.
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    bool IsSessionReady(Session session);

    /// <summary>
    /// Add a player to a session.
    /// </summary>
    /// <param name="player"></param>
    bool AddPlayerToSession(Player player, Session session);
}

