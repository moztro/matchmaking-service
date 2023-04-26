using System;
using Microsoft.EntityFrameworkCore;
using Rovio.Challenge.Matchmaking.Database.Extensions;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Database.DbContexts
{
	public class SqlLiteContext: DbContext
	{
		public DbSet<Player> Players { get; set; }
		public DbSet<Game> Games { get; set; }
		public DbSet<Session> Sessions { get; set; }
		public DbSet<Server> Servers { get; set; }

		public SqlLiteContext() { }
		public SqlLiteContext(DbContextOptions<SqlLiteContext> options)
			: base(options)
		{ }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite(DatabaseExtensions.DefaultSqlliteConnectionString);
			}
			base.OnConfiguring(optionsBuilder);
        }
    }
}

