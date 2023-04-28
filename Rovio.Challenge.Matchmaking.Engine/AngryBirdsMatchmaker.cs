using Rovio.Challenge.Matchmaking.Queues;
using Rovio.Challenge.Matchmaking.Managers;
using Rovio.Challenge.Matchmaking.Domain.Games;

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
        ISessionManager sessionManager)
        :base(queue, sessionManager)
    { }

    public Task StartMatchmakingProcess()
    {
        base.BaseStartMatchmakingProcess();

        return Task.CompletedTask;
    }
}

