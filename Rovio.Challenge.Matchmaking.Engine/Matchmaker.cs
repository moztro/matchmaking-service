using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Queues;
using Rovio.Challenge.Matchmaking.Managers;

namespace Rovio.Challenge.Matchmaking.Engine;

public interface IMatchmaker
{
    /// <summary>
    /// Start the process for finding matched sessions for players in queue.
    /// </summary>
    /// <returns></returns>
    Task StartMatchmakingProcess();
}

/// <summary>
/// Base class for running the matchmaking for a game
/// </summary>
/// <typeparam name="T"></typeparam>
public class Matchmaker<T> where T : Game
{
    private readonly BaseQueue<T> _queue;
    private readonly ISessionManager _sessionManager;

    public Matchmaker(
        BaseQueue<T> queue,
        ISessionManager sessionManager)
    {
        _queue = queue;
        _sessionManager = sessionManager;
    }
    
    public Task BaseStartMatchmakingProcess()
    {
        var dequeuedPlayer = _queue.DequeuePlayer();
        if (dequeuedPlayer == null)
            return Task.CompletedTask;
        var sessions = _sessionManager.GetAvailableSessionsInPlayersRegion(dequeuedPlayer.Player, dequeuedPlayer.Game);

        // add player to the session
        // session.Players.Add(dequeuedPlayer.Player);
        // _sessionRepository.Update(session);

        return Task.CompletedTask;
    }
}

