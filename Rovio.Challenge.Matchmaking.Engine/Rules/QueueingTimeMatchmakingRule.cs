namespace Rovio.Challenge.Matchmaking.Engine.Rules;

public interface IQueueingTimeMatchmakingRule
{ }

/// <summary>
/// Matching rule for queuing time.
/// </summary>
public class QueueingTimeMatchmakingRule : IQueueingTimeMatchmakingRule, IMatchmakingRule<TimeSpan>
{
    private static TimeSpan _initialWaitingTimeDistance = TimeSpan.FromSeconds(10);
    public TimeSpan AllowedDistance { get; set; } = _initialWaitingTimeDistance;

    public bool Match(TimeSpan value, TimeSpan target)
    {
        var diff = target - value;

        return Math.Abs(diff.TotalSeconds) <= AllowedDistance.TotalSeconds;
    }
}