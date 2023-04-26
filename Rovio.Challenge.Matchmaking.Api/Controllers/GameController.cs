using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rovio.Challenge.Matchmaking.Api.Models;
using Rovio.Challenge.Matchmaking.Domain.Settings;

namespace Rovio.Challenge.Matchmaking.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly GameSessionSettings _sessionSettings;

    public GameController(ILogger<GameController> logger, IOptions<GameSessionSettings> sessionSettings)
    {
        _logger = logger;
        _sessionSettings = sessionSettings.Value;
    }

    [HttpPost("{gameId}/join")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult Join(
        [FromRoute] string gameId,
        [FromBody] JoinGameRequest joinRequest)
    {
        return Ok(new JoinGameResponse("new-session"));
    }
}

