using Rovio.Challenge.Matchmaking.Queues;
using Rovio.Challenge.Matchmaking.Managers;
using Rovio.Challenge.Matchmaking.Domain.Games;

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
        ISessionManager sessionManager)
        :base(queue, sessionManager)
    { }

    public Task StartMatchmakingProcess()
    {
        base.BaseStartMatchmakingProcess();
        
        return Task.CompletedTask;
    }
}

