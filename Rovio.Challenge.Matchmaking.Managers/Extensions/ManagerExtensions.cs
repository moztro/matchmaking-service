using Microsoft.Extensions.DependencyInjection;
using Rovio.Challenge.Matchmaking.Repositories.Extensions;

namespace Rovio.Challenge.Matchmaking.Managers.Extensions;

public static class ManagerExtensions
{
    public static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddTransient<ISessionManager, SessionManager>();

        return services;
    }
}