using System;
using Rovio.Challenge.Matchmaking.Database.DbContexts;

namespace Rovio.Challenge.Matchmaking.Api.Extensions
{
	public static class ConfigurationExtensions
	{
		/// <summary>
		/// Ensures the database is created and seed.
		/// </summary>
		/// <param name="app"></param>
		public static void EnsureDatabaseSetup(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var serviceProvider = scope.ServiceProvider;
			var db = serviceProvider.GetRequiredService<SqlLiteContext>();
			db.Database.EnsureCreated();
			SqlLiteDataSeeder.Seed(db);
		}
	}
}

