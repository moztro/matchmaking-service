using Microsoft.Extensions.DependencyInjection;
using Rovio.Challenge.Matchmaking.Domain.Settings;
using Rovio.Challenge.Matchmaking.Repositories.Extensions;
using Microsoft.Extensions.Configuration;

namespace Rovio.Challenge.Matchmaking.Engine.Extensions
{
	public static class MatchmakerExtensions
	{
		public static IServiceCollection AddMatchmaking(this IServiceCollection services, ConfigurationManager configuration, string environment)
		{
			configuration
				.AddJsonFile("gamesettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"gamesettings.{environment}.json", optional: true);

			services.Configure<GameSessionSettings>(configuration.GetSection("GameSessionSettings"));

			services.AddRepositories();
			services.AddSingleton<Matchmaker>();

			return services;
		}
	}
}

