using Microsoft.Extensions.Options;
using Moq;
using Rovio.Challenge.Matchmaking.Database.DbContexts;
using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Domain.Settings;
using Rovio.Challenge.Matchmaking.Engine;
using Rovio.Challenge.Matchmaking.Engine.Rules;
using Rovio.Challenge.Matchmaking.Engine.Utils;
using Rovio.Challenge.Matchmaking.Managers;
using Rovio.Challenge.Matchmaking.Queues;
using Rovio.Challenge.Matchmaking.Queues.Models;
using Rovio.Challenge.Matchmaking.Repositories;

namespace Rovio.Challenge.Matchmaking.Tests.UnitTests;

public class MatchmakingUnitTests
{
    private Mock<BaseQueue<Game>> _queue;
    private Mock<SessionManager> _sessionManager;
    private Mock<LatencyMatchmakingRule> _latencyRule;
    private Mock<QueueingTimeMatchmakingRule> _queueingRule;
    private Mock<IRetrier> _retrier;
    private Matchmaker<Game> _matchmaker;
    private List<Player> _players = new List<Player>
    {
        new Player("playerA") { Latency = 50 },
        new Player("playerB") { Latency = 100 },
        new Player("playerC") { Latency = 10 },
        new Player("playerD") { Latency = 25 }
    };

    public MatchmakingUnitTests()
    {
        _queue = new Mock<BaseQueue<Game>>();
        var db = new SqlLiteContext();
        var serverRepository = new Mock<BaseRepository<Server>>(db);
        var sessionRepository = new Mock<BaseRepository<Session>>(db);
        var gameRepository = new Mock<BaseRepository<Game>>(db);
        var playerRepository = new Mock<BaseRepository<Player>>(db);
        var gameSettings = new GameSessionSettings()
        {
            MinPlayers = 2,
            MaxPlayers = 10
        };
        _sessionManager = new Mock<SessionManager>(serverRepository.Object, sessionRepository.Object, gameRepository.Object, playerRepository.Object, new OptionsWrapper<GameSessionSettings>(gameSettings));
        _latencyRule = new Mock<LatencyMatchmakingRule>();
        _queueingRule = new Mock<QueueingTimeMatchmakingRule>();
        _retrier = new Mock<IRetrier>();

        _matchmaker = new Matchmaker<Game>(
            _queue.Object,
            _sessionManager.Object,
            _latencyRule.Object,
            _queueingRule.Object,
            _retrier.Object
        );
    }

    [Fact]
    public void WhenAddPlayerToLobby_ThenLookupForPlayerInQueue_ShouldSucceed()
    {
        var playerA = _players[0];
        var game = new Game();
        var sessions = new List<Session>
        {
            new Session(){ Id = Guid.NewGuid(), Players = new List<Player>{ playerA } }
        };
        _queue.Setup(x => x.PeekPlayer()).Returns(new DequeuedPlayer(playerA, game, DateTimeOffset.UtcNow));
        _sessionManager.Setup(x => x.GetAvailableSessionsInPlayersRegion(It.IsAny<Player>(), It.IsAny<Game>())).Returns(sessions);

        _matchmaker.Queue.QueuePlayer(playerA, game);

        var (dequed, availableSessions) = _matchmaker.DequePlayerAndFindSessionsAvailable();

        Assert.NotNull(dequed);
        Assert.NotEmpty(availableSessions);
        Assert.Equal(availableSessions[0].Players[0].Username, playerA.Username);
    }

    [Fact]
    public void WhenLookForMatchingSessions_ApplyRules_ThenFindAMatch_ReturnAvailableSessions()
    {
        var playerA = _players[0];
        var game = new Game();
        var sessions = new List<Session>
        {
            new Session(){ Id = Guid.NewGuid() }
        };
        _queue.Setup(x => x.PeekPlayer()).Returns(new DequeuedPlayer(playerA, game, DateTimeOffset.UtcNow));
        _sessionManager.Setup(x => x.GetAvailableSessionsInPlayersRegion(It.IsAny<Player>(), It.IsAny<Game>())).Returns(sessions);
        _sessionManager.Setup(x => x.AddPlayerToSession(It.IsAny<Player>(), It.IsAny<Session>())).Returns(true);
        _retrier.Setup(x => x.Run<bool>(It.IsAny<Func<bool>>(), It.IsAny<TimeSpan>(), It.IsAny<int>())).Returns(true);
        _latencyRule.Setup(x => x.Match(It.IsAny<double>(), It.IsAny<double>())).Returns(true);
        _queueingRule.Setup(x => x.Match(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Returns(true);
        _sessionManager.Setup(x => x.IsSessionReady(It.IsAny<Session>())).Returns(true);

        _matchmaker.Queue.QueuePlayer(playerA, game);

        var matchingSessions = _matchmaker.GetSessionsBasedOnRules();

        Assert.NotEmpty(matchingSessions);
    }

    [Fact]
    public void WhenLookForMatchingSessions_ApplyRules_ThenNoMatches_ReturnEmptySessions()
    {
        var playerA = _players[0];
        var game = new Game();
        var sessions = new List<Session>
        {
            new Session(){ Id = Guid.NewGuid() }
        };
        _queue.Setup(x => x.DequeuePlayer()).Returns(new DequeuedPlayer(playerA, game, DateTimeOffset.UtcNow));
        _sessionManager.Setup(x => x.GetAvailableSessionsInPlayersRegion(It.IsAny<Player>(), It.IsAny<Game>())).Returns(sessions);
        _retrier.Setup(x => x.Run<bool>(It.IsAny<Func<bool>>(), It.IsAny<TimeSpan>(), It.IsAny<int>())).Returns(false);
        _latencyRule.Setup(x => x.Match(It.IsAny<double>(), It.IsAny<double>())).Returns(false);
        _queueingRule.Setup(x => x.Match(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Returns(false);
        _sessionManager.Setup(x => x.IsSessionReady(It.IsAny<Session>())).Returns(true);

        _matchmaker.Queue.QueuePlayer(playerA, game);

        var matchingSessions = _matchmaker.GetSessionsBasedOnRules();

        Assert.Empty(matchingSessions);
    }
}