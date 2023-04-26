using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Rovio.Challenge.Matchmaking.Database.DbContexts;
using Rovio.Challenge.Matchmaking.Engine.Extensions;
using Microsoft.Extensions.Configuration;

namespace Rovio.Challenge.Matchmaking.UnitTests.Infrastructure;

internal class Startup<T> : WebApplicationFactory<T> where T : class
{
    private SqliteConnection _connection = null;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        builder.ConfigureServices((builder, services) =>
        {
            services.RemoveAll(typeof(DbContextOptions<SqlLiteContext>));
            services.AddDbContext<SqlLiteContext>(options => { options.UseSqlite(_connection); });

            services.AddMatchmaking((ConfigurationManager)builder.Configuration, "development");
        });

        return base.CreateHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        _connection?.Dispose();
        base.Dispose(disposing);
    }
}