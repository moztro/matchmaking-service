using System;
using Rovio.Challenge.Matchmaking.Database.DbContexts;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Repositories
{
	public class SessionRepository : BaseRepository<Session>
	{
		public SessionRepository(SqlLiteContext context)
			:base(context)
		{ }
	}
}

