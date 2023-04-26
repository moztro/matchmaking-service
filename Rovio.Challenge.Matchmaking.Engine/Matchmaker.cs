using System.Collections.Concurrent;
using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Domain.Settings;
using Rovio.Challenge.Matchmaking.Engine.Models;
using Microsoft.Extensions.Options;
using Rovio.Challenge.Matchmaking.Repositories;
using Rovio.Challenge.Matchmaking.Utils;

namespace Rovio.Challenge.Matchmaking.Engine;

public abstract class Matchmaker
{
    private ConcurrentQueue<QueuedPlayer> _lobbyQueue = new ();
    private Task _matchmakingProcess = null;
    private readonly GameSessionSettings _sessionSettings;
    private readonly ServerRepository _serverRepository;
    private readonly SessionRepository _sessionRepository;

    public Matchmaker(
        IOptions<GameSessionSettings> sessionSettings,
        ServerRepository serverRepository,
        SessionRepository sessionRepository)
    {
        _sessionSettings = sessionSettings.Value;
        _serverRepository = serverRepository;
        _sessionRepository = sessionRepository;
    }

    /// <summary>
    /// Add a player to the queue to wait for a game.
    /// </summary>
    /// <param name="player"></param>
    public void QueuePlayer(Player player, Game game)
    {
        _lobbyQueue.Enqueue(new QueuedPlayer(player, game, DateTimeOffset.UtcNow));
        Task.Run(()=> StartMatchmakingProcess());
    }

    /// <summary>
    /// Dequeues players that are ready for a game.
    /// </summary>
    /// <returns></returns>
    public DequeuedPlayer DequeuePlayer()
    {
        if (_lobbyQueue.TryDequeue(out QueuedPlayer player))
            return player.ToDequeuedPlayer();
        return null;
    }

    /// <summary>
    /// Get a player if it does exists in the queue, null if does not.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Player GetPlayerFromQueue(string username)
    {
        var dequeued = _lobbyQueue.FirstOrDefault(q => q.Player.Username == username);
        return dequeued?.Player;
    }

    /// <summary>
    /// Start the process for finding matched sessions for players in queue.
    /// </summary>
    /// <returns></returns>
    public Task StartMatchmakingProcess()
    {
        var dequeuedPlayer = DequeuePlayer();
        var server = GetGameServer(dequeuedPlayer.Player);
        var session = GetSessionForPlayer(server, dequeuedPlayer);

        // add player to the session
        session.Players.Add(dequeuedPlayer.Player);
        _sessionRepository.Update(session);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Look for servers based on player's location,
    /// if no servers available, creates one.
    /// </summary>
    /// <param name="player"></param>
    /// <returns>The game server.</returns>
    public Server GetGameServer(Player player)
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

    public Session GetSessionForPlayer(Server server, DequeuedPlayer dequeuedPlayer)
    {
        // TO-DO: need to pick the session based on game settings and actually make a match with similar players.
        var session = _sessionRepository.Get(s => s.Game.Name == dequeuedPlayer.Game.Name).FirstOrDefault();
        // If there is no session for the player, creates one
        if(session == null)
        {
            var newSession = new Session()
            {
                Game = dequeuedPlayer.Game,
                Server = server
            };
            _sessionRepository.Insert(newSession);
            session = newSession;
        }
        return session;
    }
}

