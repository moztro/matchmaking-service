using System;
using Rovio.Challenge.Matchmaking.Database.DbContexts;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Repositories
{
	public class ServerRepository : BaseRepository<Server>
	{
		public ServerRepository(SqlLiteContext context)
			:base(context)
		{ }
	}
}

