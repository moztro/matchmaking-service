using System;
using Microsoft.Extensions.DependencyInjection;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Repositories.Extensions
{
	public static class RepositoryExtensions
	{
		public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<BaseRepository<Game>, GameRepository>();
            services.AddTransient<BaseRepository<Player>, PlayerRepository>();
            services.AddTransient<BaseRepository<Session>, SessionRepository>();
            services.AddTransient<BaseRepository<Server>, ServerRepository>();

            return services;
		}
	}
}

