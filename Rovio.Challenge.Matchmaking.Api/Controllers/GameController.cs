using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rovio.Challenge.Matchmaking.Api.Models;
using Rovio.Challenge.Matchmaking.Engine;
using Rovio.Challenge.Matchmaking.Api.Extensions;

namespace Rovio.Challenge.Matchmaking.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly Matchmaker _matchmaker;

    public GameController(ILogger<GameController> logger, Matchmaker matchmaker)
    {
        _logger = logger;
        _matchmaker = matchmaker;
    }

    [HttpPost("{gameId}/join")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult Join(
        [FromRoute] string gameId,
        [FromBody] JoinGameRequest joinRequest)
    {
        var player = joinRequest.ToPlayer();
        return Ok(new JoinGameResponse($"{player.Username} requested to join a session"));
    }

    [HttpGet("{gameId}/player/{username}/status")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Status(string gameId, string username)
    {
        return Ok($"{username} is waiting at the lobby");
    }
}

