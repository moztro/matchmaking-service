using System;
using Rovio.Challenge.Matchmaking.Domain.Models;
using Rovio.Challenge.Matchmaking.Utils.Constants;

namespace Rovio.Challenge.Matchmaking.Database.DbContexts
{
	public class SqlLiteDataSeeder
	{
		private static List<Game> seedGames = new List<Game>
		{
			new Game { Name = GameTitles.AngryBirds},
			new Game { Name = GameTitles.BadPiggies}
		};

		/// <summary>
		/// Seed the database with some initial games data.
		/// </summary>
		/// <param name="db"></param>
		public static void Seed(SqlLiteContext db)
		{
			if(!db.Games.Any())
			{
				db.Games.AddRange(seedGames);
				db.SaveChanges();
			}
		}
	}
}

