using Rovio.Challenge.Matchmaking.Api.Controllers;
using Rovio.Challenge.Matchmaking.UnitTests.Infrastructure;

namespace Rovio.Challenge.Matchmaking.UnitTests;

public class UnitTest1
{
    private readonly Startup<GameController> _application;

    public UnitTest1()
    {
        _application = new Startup<GameController>();
    }

    public void Dispose()
    {
        _application.Dispose();
    }

    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }
}
