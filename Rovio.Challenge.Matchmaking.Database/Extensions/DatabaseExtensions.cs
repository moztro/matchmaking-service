using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rovio.Challenge.Matchmaking.Database.DbContexts;

namespace Rovio.Challenge.Matchmaking.Database.Extensions
{
	public static class DatabaseExtensions
	{
		public const string DefaultSqlliteConnectionString = "Data Source=sqllite.db";

		/// <summary>
		/// Adds support for Sqllite database.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		/// <returns></returns>
        public static IServiceCollection AddSqlliteDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("sqlliteConnectionString") ?? DefaultSqlliteConnectionString;
			services.AddSqlite<SqlLiteContext>(connectionString);
			return services;
		}
	}
}

