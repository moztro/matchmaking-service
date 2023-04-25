using Rovio.Challenge.Matchmaking.Domain.Settings;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

configuration
    .AddJsonFile("gamesettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"gamesettings.{builder.Environment.EnvironmentName}.json", optional: true);

services.Configure<GameSessionSettings>(configuration.GetSection("GameSessionSettings"));

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

