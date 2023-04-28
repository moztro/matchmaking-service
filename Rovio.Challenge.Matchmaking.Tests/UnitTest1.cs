using Rovio.Challenge.Matchmaking.Api.Controllers;
using Rovio.Challenge.Matchmaking.Tests.Infrastructure;

namespace Rovio.Challenge.Matchmaking.Tests;

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
