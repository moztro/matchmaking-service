using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Utils.Constants;

namespace Rovio.Challenge.Matchmaking.Domain.Games;

/// <summary>
/// Concrete game angry birds
/// </summary>
public class AngryBirds : Game
{
    public AngryBirds()
    {
        Name = GameTitles.AngryBirds;
    }
}