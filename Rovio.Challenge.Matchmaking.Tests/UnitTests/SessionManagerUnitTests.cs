using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using Moq;
using Rovio.Challenge.Matchmaking.Database.DbContexts;
using Rovio.Challenge.Matchmaking.Domain.Games;
using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Domain.Settings;
using Rovio.Challenge.Matchmaking.Managers;
using Rovio.Challenge.Matchmaking.Repositories;

namespace Rovio.Challenge.Matchmaking.Tests.UnitTests;

public class SessionManagerUnitTests
{
    private readonly Mock<BaseRepository<Server>> _serverRepository;
    private readonly Mock<BaseRepository<Session>> _sessionRepository;
    private readonly Mock<BaseRepository<Game>> _gameRepository;
    private readonly Mock<BaseRepository<Player>> _playerRepository;
    private readonly GameSessionSettings _gameSettings;
    private ISessionManager _sessionManager;
    
    public SessionManagerUnitTests()
    {
        var db = new SqlLiteContext();
        _serverRepository = new Mock<BaseRepository<Server>>(db);
        _sessionRepository = new Mock<BaseRepository<Session>>(db);
        _gameRepository = new Mock<BaseRepository<Game>>(db);
        _playerRepository = new Mock<BaseRepository<Player>>(db);
        _gameSettings = new GameSessionSettings()
        {
            MinPlayers = 2,
            MaxPlayers = 10
        };

        _sessionManager = new SessionManager(_serverRepository.Object, _sessionRepository.Object, _gameRepository.Object, _playerRepository.Object, new OptionsWrapper<GameSessionSettings>(_gameSettings));
    }

    [Fact]
    public void WhenRequestServer_WithAMatchRegionPlayer_ShouldReturnSessionsAndSucceed()
    {
        var servers = new List<Server>()
        {
            new Server(){ Region = Domain.Enums.Region.Americas}
        };
        var sessions = new List<Session>()
        {
            new Session() { Id = Guid.NewGuid() }
        };

        _serverRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Server, bool>>>(), null, null)).Returns(servers);
        _sessionRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Session, bool>>>(), null, null)).Returns(sessions);

        var player = new Player("playerA");
        var game = new AngryBirds();
        var availableSessions = _sessionManager.GetAvailableSessionsInPlayersRegion(player, game);

        Assert.NotNull(availableSessions);
        Assert.NotEmpty(availableSessions);
    }

    [Fact]
    public void WhenRequestServer_NonMatchingRegionPlayer_ShouldCreateSessionsAndSucceed()
    {
        _serverRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Server, bool>>>(), null, null)).Returns(Enumerable.Empty<Server>());
        _sessionRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Session, bool>>>(), null, null)).Returns(Enumerable.Empty<Session>());

        var player = new Player("playerA");
        var game = new AngryBirds();
        var availableSessions = _sessionManager.GetAvailableSessionsInPlayersRegion(player, game);

        _serverRepository.Verify(x => x.Insert(It.IsAny<Server>()), Times.Once);
        _sessionRepository.Verify(x => x.Insert(It.IsAny<Session>()), Times.Once);

        Assert.NotNull(availableSessions);
        Assert.NotEmpty(availableSessions);
    }

    [Fact]
    public void WhenCheckingForReadySession_DoesComplyWithSettings_ReturnTrue()
    {
        var session = new Session()
        {
            Players = new List<Player>
            {
                new Player("playerA"),
                new Player("playerB")
            }
        };

        Assert.True(_sessionManager.IsSessionReady(session));
    }

    [Fact]
    public void WhenCheckingForReadySession_DoesComplyWithSettings_ReturnFalse()
    {
        var session = new Session()
        {
            Players = new List<Player>
            {
                new Player("playerA")
            }
        };

        Assert.False(_sessionManager.IsSessionReady(session));
    }
}