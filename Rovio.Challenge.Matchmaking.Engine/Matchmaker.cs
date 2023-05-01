using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Queues;
using Rovio.Challenge.Matchmaking.Managers;
using Rovio.Challenge.Matchmaking.Queues.Models;
using Rovio.Challenge.Matchmaking.Engine.Rules;
using Rovio.Challenge.Matchmaking.Engine.Utils;

namespace Rovio.Challenge.Matchmaking.Engine;

public interface IMatchmaker
{
    /// <summary>
    /// Adds a player to queueing for a game.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    Task AddPlayerToLobby(Player player);
    /// <summary>
    /// Start the process for finding matched sessions for players in queue.
    /// </summary>
    /// <returns></returns>
    Session StartMatchmakingProcess();
    /// <summary>
    /// Gets a player from the game queue.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Player GetPlayerFromQueue(string username);
}

/// <summary>
/// Base class for running the matchmaking for a game
/// </summary>
/// <typeparam name="T"></typeparam>
public class Matchmaker<T> where T : Game
{
    public readonly BaseQueue<T> Queue;
    private readonly ISessionManager _sessionsManager;
    private readonly LatencyMatchmakingRule _latencyRule;
    private readonly QueueingTimeMatchmakingRule _queueingTimeRule;
    private readonly IRetrier _retrier;

    public Matchmaker(
        BaseQueue<T> queue,
        ISessionManager sessionsManager,
        LatencyMatchmakingRule latencyRule,
        QueueingTimeMatchmakingRule queueingTimeRule,
        IRetrier retrier)
    {
        Queue = queue;
        _sessionsManager = sessionsManager;
        _latencyRule = latencyRule;
        _queueingTimeRule = queueingTimeRule;
        _retrier = retrier;
    }
    
    public (DequeuedPlayer, List<Session>) DequePlayerAndFindSessionsAvailable()
    {
        var sessions = new List<Session>();
        var dequeuedPlayer = Queue.PeekPlayer();
        if (dequeuedPlayer == null)
            return (null, sessions);
        sessions = _sessionsManager.GetAvailableSessionsInPlayersRegion(dequeuedPlayer.Player, dequeuedPlayer.Game);

        return (dequeuedPlayer, sessions);
    }

    public List<ReadySession> GetSessionsBasedOnRules()
    {
        var (player, sessions) = DequePlayerAndFindSessionsAvailable();
        var avgLatency = sessions.Any() ? sessions.Average(s => s.Players.Any() ? s.Players.Average(p => p.Latency) : 0) : 0;

        var matchedSessions = new List<ReadySession>();
        foreach(var session in sessions)
        {
            // TO-DO: with each try should increment target property, in this case if latency
            // does not have a match, it should be able to increase the value to allow matching
            // to be flexible with each retry.
            var matches =_retrier.Run<bool>(
                () => _latencyRule.Match(player.Player.Latency, avgLatency),
                TimeSpan.FromMilliseconds(1000)
            );

            if (matches && _sessionsManager.IsSessionReady(session))
            {
                var readySession = new ReadySession();
                readySession.Session = session;
                readySession.Players.Add(player.ToDequeuedPlayer());
                matchedSessions.Add(readySession);
            }
        }

        var avgQueueingTimeInSecs = matchedSessions.Any() ?
            matchedSessions.Average(s => s.Players.Any() ? s.Players.Average(p => p.QueueingTime.TotalSeconds) : 0) : 0;
        foreach(var session in matchedSessions.ToList())
        {
            // TO-DO: with each try should increment target property, in this case if queueing time
            // does not have a match, it should be able to increase the value to allow matching
            // to be flexible with each retry.
            var matches = _retrier.Run<bool>(
                () => _queueingTimeRule.Match(player.QueueingTime, TimeSpan.FromSeconds(avgQueueingTimeInSecs)),
                TimeSpan.FromMilliseconds(1000)
            );

            // if session does not match or happen to become unready, remove it
            if (!(matches && _sessionsManager.IsSessionReady(session.Session)))
            {
                matchedSessions.Remove(session);
            }
        }

        // If matching was success, then dequeue the player.
        if(matchedSessions.Any())
            Queue.DequeuePlayer();

        return matchedSessions;
    }
}

