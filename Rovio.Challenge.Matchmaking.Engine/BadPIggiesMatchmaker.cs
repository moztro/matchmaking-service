using Rovio.Challenge.Matchmaking.Queues;
using Rovio.Challenge.Matchmaking.Managers;
using Rovio.Challenge.Matchmaking.Domain.Games;
using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Engine.Rules;
using Rovio.Challenge.Matchmaking.Engine.Utils;
using Rovio.Challenge.Matchmaking.Domain.Exceptions;

namespace Rovio.Challenge.Matchmaking.Engine;

public interface IBadPiggiesMatchmaker : IMatchmaker
{}
/// <summary>
/// Concrete matchmaker class for bad piggies, allowing this concrete
/// matchmaker to add more game specific matching rules if desired.
/// </summary>
public class BadPiggiesMatchmaker : Matchmaker<BadPiggies>, IBadPiggiesMatchmaker
{
    public BadPiggiesMatchmaker(
        BaseQueue<BadPiggies> queue,
        ISessionManager sessionManager,
        LatencyMatchmakingRule latencyRule,
        QueueingTimeMatchmakingRule queueingTimeRule,
        IRetrier retrier)
        :base(queue, sessionManager, latencyRule, queueingTimeRule, retrier)
    { }

    public Task AddPlayerToLobby(Player player)
    {
        var queue = ((BaseQueue<BadPiggies>)base.Queue);
        queue.QueuePlayer(player, new BadPiggies());
        return Task.CompletedTask;
    }
    
    public Session StartMatchmakingProcess()
    {
        var sessions = base.GetSessionsBasedOnRules();
        if (!sessions.Any())
            throw new SessionNotFoundException();

        // returns the first session available, if this concrete matchmaker
        // needs to it can apply more rules on top if this result to better
        // suit the game specific matchmaking rules.
        return sessions.Select(s => s.Session).FirstOrDefault();
    }
}

