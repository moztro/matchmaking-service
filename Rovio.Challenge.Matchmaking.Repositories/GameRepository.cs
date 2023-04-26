using System;
using Rovio.Challenge.Matchmaking.Database.DbContexts;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Repositories
{
	public class GameRepository : BaseRepository<Game>
	{
		public GameRepository(SqlLiteContext context)
			:base(context)
		{ }
	}
}

