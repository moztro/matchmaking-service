using System;
using Microsoft.Extensions.DependencyInjection;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Repositories.Extensions
{
	public static class RepositoryExtensions
	{
		public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<BaseRepository<Game>, GameRepository>();
            services.AddScoped<BaseRepository<Player>, PlayerRepository>();
            services.AddScoped<BaseRepository<Server>, ServerRepository>();
            services.AddScoped<BaseRepository<Session>, SessionRepository>();

            return services;
		}
	}
}

