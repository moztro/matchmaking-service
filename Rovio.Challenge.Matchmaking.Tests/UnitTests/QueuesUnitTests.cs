using Rovio.Challenge.Matchmaking.Domain.Games;
using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Queues;

namespace Rovio.Challenge.Matchmaking.Tests.UnitTests;

public class QueueUntTests
{
    private AngryBirdsQueue _angryBirdsQueue;
    
    public QueueUntTests()
    {
        _angryBirdsQueue = new AngryBirdsQueue();
    }

    [Fact]
    public void WhenEnquePlayerA_ThenDequeuePlayerA_Pass()
    {
        var playerA = new Player()
        {
            Username = "playerA"
        };

        _angryBirdsQueue.QueuePlayer(playerA, new AngryBirds());

        var dequeuedPlayer = _angryBirdsQueue.DequeuePlayer();

        Assert.Equal(playerA.Username, dequeuedPlayer.Player.Username);
    }

    [Fact]
    public void WhenEnquePlayerA_ThenPeekPlayerA_ShouldPlayerRemainInQueue()
    {
        var playerA = new Player()
        {
            Username = "playerA"
        };

        _angryBirdsQueue.QueuePlayer(playerA, new AngryBirds());

        var dequeuedPlayer = _angryBirdsQueue.PeekPlayer();

        Assert.Equal(playerA.Username, dequeuedPlayer.Player.Username);
        // player should remain in the queue
        Assert.NotNull(_angryBirdsQueue.GetPlayerFromQueue(playerA.Username));
    }

    [Fact]
    public void WhenDequeueEmptyQueue_ReturnsNull()
    {
        Assert.Null(_angryBirdsQueue.DequeuePlayer());
    }

    [Fact]
    public void WhenRequestSpecificPlayerFromQueue_ShouldReturnPlayerIfExists()
    {
        var playerA = new Player(){ Username = "playerA"};
        var playerB = new Player(){ Username = "playerB"};
        var playerC = new Player(){ Username = "playerC"};
        var game = new AngryBirds();

        _angryBirdsQueue.QueuePlayer(playerA, game);
        _angryBirdsQueue.QueuePlayer(playerB, game);
        _angryBirdsQueue.QueuePlayer(playerC, game);

        var expectedPlayer = "playerA";
        var dequed = _angryBirdsQueue.GetPlayerFromQueue(expectedPlayer);
        Assert.NotNull(dequed);
        Assert.Equal(expectedPlayer, dequed.Username);
    }
}