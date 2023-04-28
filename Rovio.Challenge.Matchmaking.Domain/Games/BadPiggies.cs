using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Utils.Constants;

namespace Rovio.Challenge.Matchmaking.Domain.Games;

/// <summary>
/// Concrete game bad piggies.
/// </summary>
public class BadPiggies : Game
{
    public BadPiggies()
    {
        Name = GameTitles.BadPiggies;
    }
}