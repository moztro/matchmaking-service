using Rovio.Challenge.Matchmaking.Domain.Settings;
using Rovio.Challenge.Matchmaking.Database.Extensions;
using Rovio.Challenge.Matchmaking.Api.Extensions;
using Rovio.Challenge.Matchmaking.Engine.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddSqlliteDatabase(configuration);
builder.Services.AddMatchmaking(configuration, builder.Environment.EnvironmentName);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.EnsureDatabaseSetup();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

