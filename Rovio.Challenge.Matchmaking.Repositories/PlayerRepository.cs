using System;
using Rovio.Challenge.Matchmaking.Database.DbContexts;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Repositories
{
	public class PlayerRepository : BaseRepository<Player>
	{
		public PlayerRepository(SqlLiteContext context)
			:base(context)
		{ }
	}
}

