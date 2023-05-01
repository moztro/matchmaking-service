using Microsoft.Extensions.Options;
using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Domain.Settings;
using Rovio.Challenge.Matchmaking.Repositories;
using Rovio.Challenge.Matchmaking.Utils;

namespace Rovio.Challenge.Matchmaking.Managers;

public class SessionManager : ISessionManager
{
    private readonly BaseRepository<Server> _serverRepository;
    private readonly BaseRepository<Session> _sessionRepository;
    private readonly GameSessionSettings _gameSettings;

    public SessionManager(
        BaseRepository<Server> serverRepository,
        BaseRepository<Session> sessionRepository,
        IOptions<GameSessionSettings> gameSettings
    )
    {
        _serverRepository = serverRepository;
        _sessionRepository = sessionRepository;
        _gameSettings = gameSettings.Value;
    }

    private Server GetGameServer(Player player)
    {
        var server = _serverRepository.Get(s => s.Region == player.Region).FirstOrDefault();
        if (server == null)
        {
            var newServer = new Server()
            {
                Ip = IpGenerator.Generate(),
                Region = player.Region
            };
            _serverRepository.Insert(newServer);
            server = newServer;
        }
        return server;
    }

    public virtual List<Session> GetAvailableSessionsInPlayersRegion(Player player, Game game)
    {
        var server = GetGameServer(player);

        var sessions = _sessionRepository.Get(s => s.Game.Name == game.Name).ToList();
        // If there is no session for the player, creates one
        if(!sessions.Any())
        {
            var newSession = new Session()
            {
                Game = game,
                Server = server,
                Players = new List<Player> { player }
            };
            _sessionRepository.Insert(newSession);
            sessions.Add(newSession);
        }
        return sessions;
    }

    public virtual bool IsSessionReady(Session session)
    {
        return session.Players.Any() &&
            session.Players.Count() >= _gameSettings.MinPlayers &&
            session.Players.Count() <= _gameSettings.MaxPlayers;
    }
}

