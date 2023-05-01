namespace Rovio.Challenge.Matchmaking.Engine.Rules;

/// <summary>
/// Contract for the matchmaking rules.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMatchmakingRule<T> where T : struct
{
    /// <summary>
    /// Determines what the distance diff between Match args should be in order to considered a match.
    /// </summary>
    /// <value></value>
    T AllowedDistance { get; set; }

    /// <summary>
    /// Determines if args are a match based on concrete implementation and allowed distance property.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    bool Match(T value, T target);
}