using System.Net;
using System.Text;
using Newtonsoft.Json;
using Rovio.Challenge.Matchmaking.Api.Controllers;
using Rovio.Challenge.Matchmaking.Api.Models;
using Rovio.Challenge.Matchmaking.Tests.Infrastructure;

namespace Rovio.Challenge.Matchmaking.Tests;

public class ApiIntegrationTests
{
    private readonly Startup<GameController> _application;
    private readonly HttpClient _client;

    public ApiIntegrationTests()
    {
        _application = new Startup<GameController>();
        _client = _application.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _application.Dispose();
    }

    [Theory]
    [InlineData("angry-birds")]
    [InlineData("bad-piggies")]
    public async Task WhenJoinAGame_WithCorrectRequest_ReturnsOk(string gameId)
    {
        var request = new JoinGameRequest()
        {
            PlayerUsername = "playerA",
            PlayerLatency = 100,
            PlayersRegion = Domain.Enums.Region.Europe
        };

        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"api/v1/game/{gameId}/join", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task WhenJoinAGame_WithWrongGameId_ReturnsOk()
    {
        var request = new JoinGameRequest()
        {
            PlayerUsername = "playerA",
            PlayerLatency = 100,
            PlayersRegion = Domain.Enums.Region.Europe
        };

        var wrongGameId = "wrong-game-id";
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"api/v1/game/{wrongGameId}/join", content);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var expectedMsg = $"No matchmaking implementation for {wrongGameId}";
        var responseMsg = await ((HttpResponseMessage)response).Content.ReadAsStringAsync();
        Assert.True(responseMsg.Contains(expectedMsg));
    }

    [Fact]
    public async Task WhenCheckPlayerStatusAndNotInQueue_ReturnsNotFound()
    {
        var gameId = "angry-birds";
        var player = "playerA";
        await WhenJoinAGame_WithCorrectRequest_ReturnsOk(gameId);

        var response = await _client.GetAsync($"api/v1/game/{gameId}/player/{player}/status");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
