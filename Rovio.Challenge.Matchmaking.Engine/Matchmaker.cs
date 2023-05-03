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
        var avgQueueingTimeInSecs = Queue.Queue.Any() ?
            Queue.Queue.Average(p => p.ToDequeuedPlayer().QueueingTime.TotalSeconds) : 0;

        // If player is already in the session, dequeued and look for next player in queue
        if (sessions.Any(s => s.Players.Any(p => p.Username == player.Player.Username)))
        {
            Queue.DequeuePlayer();
            return GetSessionsBasedOnRules();
        }

        var matchedSessions = new List<ReadySession>();
        foreach(var session in sessions)
        {
            // TO-DO: with each try should increment target property, in this case if latency
            // does not have a match, it should be able to increase the value to allow matching
            // to be flexible with each retry.
            var matchLatency =_retrier.Run<bool>(
                () => _latencyRule.Match(player.Player.Latency, avgLatency),
                TimeSpan.FromMilliseconds(1000)
            );
            var matchQueueing = _retrier.Run<bool>(
                () => _queueingTimeRule.Match(player.QueueingTime, TimeSpan.FromSeconds(avgQueueingTimeInSecs)),
                TimeSpan.FromMilliseconds(1000)
            );

            // If matching was success, then dequeue the player and add to session.
            if (matchLatency && matchQueueing)
            {
                _sessionsManager.AddPlayerToSession(player.Player, session);
                Queue.DequeuePlayer();
            } 

            if(_sessionsManager.IsSessionReady(session))
            {
                var readySession = new ReadySession();
                readySession.Session = session;
                readySession.Players.AddRange(session.Players);
                matchedSessions.Add(readySession);
            }
        }

        return matchedSessions;
    }
}

