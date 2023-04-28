using Microsoft.Extensions.DependencyInjection;
using Rovio.Challenge.Matchmaking.Domain.Settings;
using Microsoft.Extensions.Configuration;
using Rovio.Challenge.Matchmaking.Queues;
using Rovio.Challenge.Matchmaking.Domain.Games;
using Rovio.Challenge.Matchmaking.Managers.Extensions;
using Rovio.Challenge.Matchmaking.Utils.Constants;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Engine.Extensions
{
	public static class MatchmakerExtensions
	{
		public delegate IMatchmaker MatchmakingServiceResolver(string gameId);

		public static IServiceCollection AddMatchmaking(this IServiceCollection services, ConfigurationManager configuration, string environment)
		{
			configuration
				.AddJsonFile("gamesettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"gamesettings.{environment}.json", optional: true);

			services.Configure<GameSessionSettings>(configuration.GetSection("GameSessionSettings"));

			services.AddManagers();
			services.AddScoped<BaseQueue<AngryBirds>, AngryBirdsQueue>();
			services.AddScoped<BaseQueue<BadPiggies>, BadPiggiesQueue>();
			services.AddScoped<IAngryBirdsMatchmaker, AngryBirdsMatchmaker>();
			services.AddScoped<IBadPiggiesMatchmaker, BadPiggiesMatchmaker>();

			services.AddSingleton<MatchmakingServiceResolver>(gameId => 
			{
				var provider = services.BuildServiceProvider();

				switch (gameId)
				{
					case GameTitles.AngryBirds: return provider.GetRequiredService<IAngryBirdsMatchmaker>();
					case GameTitles.BadPiggies: return provider.GetRequiredService<IBadPiggiesMatchmaker>();
					default: throw new NotImplementedException($"No matchmaking implementation for ${gameId}");
				};
			});

			return services;
		}
	}
}

