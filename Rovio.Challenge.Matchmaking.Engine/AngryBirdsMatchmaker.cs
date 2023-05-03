using Rovio.Challenge.Matchmaking.Queues;
using Rovio.Challenge.Matchmaking.Managers;
using Rovio.Challenge.Matchmaking.Domain.Games;
using Rovio.Challenge.Matchmaking.Engine.Rules;
using Rovio.Challenge.Matchmaking.Engine.Utils;
using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Queues.Models;
using Rovio.Challenge.Matchmaking.Domain.Exceptions;

namespace Rovio.Challenge.Matchmaking.Engine;

public interface IAngryBirdsMatchmaker : IMatchmaker
{}
/// <summary>
/// Concrete matchmaker class for angry birds, allowing this concrete
/// matchmaker to add more matching rules if desired.
/// </summary>
public class AngryBirdsMatchmaker : Matchmaker<AngryBirds>, IAngryBirdsMatchmaker
{
    public AngryBirdsMatchmaker(
        BaseQueue<AngryBirds> queue,
        ISessionManager sessionManager,
        LatencyMatchmakingRule latencyRule,
        QueueingTimeMatchmakingRule queueingTimeRule,
        IRetrier retrier)
        :base(queue, sessionManager, latencyRule, queueingTimeRule, retrier)
    { }

    public Task AddPlayerToLobby(Player player)
    {
        var queue = ((BaseQueue<AngryBirds>)base.Queue);
        queue.QueuePlayer(player, new AngryBirds());
        return Task.CompletedTask;
    }

    public Player GetPlayerFromQueue(string username)
    {
        return ((BaseQueue<AngryBirds>)base.Queue).GetPlayerFromQueue(username);
    }

    public Session StartMatchmakingProcess()
    {
        var sessions = base.GetSessionsBasedOnRules();

        // returns the first session available, if this concrete matchmaker
        // needs to it can apply more rules on top if this result to better
        // suit the game specific matchmaking rules.
        return sessions?.Select(s => s.Session)?.FirstOrDefault();
    }
}

