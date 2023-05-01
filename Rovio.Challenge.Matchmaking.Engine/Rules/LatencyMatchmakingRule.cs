namespace Rovio.Challenge.Matchmaking.Engine.Rules;

public interface ILatencyMatchmakingRule
{ }

/// <summary>
/// Matching rule for player's latency.
/// </summary>
public class LatencyMatchmakingRule : ILatencyMatchmakingRule, IMatchmakingRule<double>
{
    private const double _initialLatencyDistance = 10;
    public double AllowedDistance { get; set; } = _initialLatencyDistance;

    public bool Match(double value, double target)
    {
        return (Math.Abs(target - value) <= AllowedDistance);
    }
}