using Rovio.Challenge.Matchmaking.Engine.Rules;

namespace Rovio.Challenge.Matchmaking.Tests.UnitTests;

public class RulesUnitTests
{
    private LatencyMatchmakingRule _latencyRule;
    private QueueingTimeMatchmakingRule _queuingRule;
    
    public RulesUnitTests()
    {
        _latencyRule = new LatencyMatchmakingRule();
        _queuingRule = new QueueingTimeMatchmakingRule();
    }

    [Theory]
    [InlineData(100, 90)]
    [InlineData(90, 80)]
    [InlineData(80, 75)]
    public void WhenValidateLatency_ValueComplyWithAllowedDistance_ReturnTrue(double value, double target)
    {
        // default allowed distance is 10
        // passed in values should differ lower or equal to allowed distance
        Assert.True(_latencyRule.Match(value, target));
    }

    [Theory]
    [InlineData(100, 1)]
    [InlineData(50, 10)]
    [InlineData(1, 100)]
    public void WhenValidateLatency_ValueNotComplyWithAllowedDistance_ReturnFalse(double value, double target)
    {
        // default allowed distance is 10
        // passed in values should differ lower or equal to allowed distance
        Assert.False(_latencyRule.Match(value, target));
    }

    [Theory]
    [InlineData(100, 100, 0)]
    [InlineData(11, 100, 90)]
    [InlineData(25, 50, 75)]
    public void WhenValidateLatency_SetCustomDistance_ValueComplyWithAllowedDistance_ReturnTrue(double customDistance, double value, double target)
    {
        // default allowed distance is {customDistance}
        _latencyRule.AllowedDistance = customDistance;
        // passed in values should differ lower or equal to allowed distance
        Assert.True(_latencyRule.Match(value, target));
    }
}