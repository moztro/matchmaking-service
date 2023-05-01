namespace Rovio.Challenge.Matchmaking.Engine.Utils;

/// <summary>
/// Class for making retries
/// </summary>
public interface IRetrier
{
    /// <summary>
    /// Runs a method and will retry for maxAttempts times.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="retryInterval"></param>
    /// <param name="maxAttempts"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T Run<T>(Func<T> action, TimeSpan retryInterval, int maxAttempts = 3);
}

public class Retrier : IRetrier
{
    public T Run<T>(Func<T> action, TimeSpan retryInterval, int maxAttempts = 3)
    {
        var exceptions = new List<Exception>();

        for (int attempted = 0; attempted < maxAttempts; attempted++)
        {
            try
            {
                if (attempted > 0)
                {
                    Thread.Sleep(retryInterval);
                }
                return action();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }
        throw new AggregateException(exceptions);
    }
}