# matchmaking-service
Matchmaking solves a particular kind of problem for online games. Let's think in online games like Fortnite, LOL, battle bay, etc. What happen if you start playing any of these games and play against a very skilled player? Pretty sure you'll bite the dust, get bored of losing, and finally leaving the game. Here comes the matchmaking to match you with players having similar skills.

## what's the matchmaking purpose?
- Matchmaking will allow players to join game sessions (either new or current).
- Players will request to join a game (via REST service) and they'll be queued to start a session.
- Players will join a game session with other players with the same or almost the same skill level. Level is dictated by:
  - Player's latency. How close each of the players are in terms of latency?
  - Queuing time. How long does it take to find a game session for a player to join?

## design considerations
### features
- Enqueue a player. Starts with players requesting to join a game session.
- Assign player to session. Returns a game session with the player added to it.
- Join an ongoing session. Players can join a session after it has started.

### out of scope
- Skill match. It won't consider player's skill in order to match.
- Allow/Deny. To block or allow certain players to join a session, its out of initial scope.
- Auto-cancellation. Players cannot cancel themselves a join request after it has started.

## diagram
- A player requests to join a game thru REST API.
- The player is added to the concrete game queue (since doesn't make sense players wait on the line before other players that are not playing the same game).
- Matchmaker pops players from the queue and look for a game session match.
- Matchmaker finds a matching session for the player and return such session.
<img width="1408" alt="image" src="https://user-images.githubusercontent.com/2914519/235269323-e5afe504-0aa8-4c6a-af3b-70e6f2cc5b47.png">

## how to build/test/run?
At project root run

`dotnet build` for building the solution. <br />
`dotnet run` for running the project in localhost.

Navigate to test project at <br />
`cd Rovio.Challenge.Matchmaking.Tests`<br />
And run <br />
`dotnet test` for running unit and integration tests.

## considerations on deploying
As a .NET project, it uses appsettings.json files and environment variables in order to deploy. So consider the following when deploying for specific environment:

1. Set the environment variable `ASPNETCORE_ENVIRONMENT` as your target environment, i.e. Staging, Production, etc.
2. Consider to create specific environment setting files for: <br />
    1. `appsettings.json` (configures logging level and DB connection strings).
    2. `gamesettings.json` (configures game settings like min and max players per session).
3. When adding specific environment setting files use the naming convention like `appsettings.{YOUR_ENVIRONMENT}.json` so the app picks up the right configuration files.

## considerations on scaling
1. Queues are game specific.
    1. This is for players to not wait for a session along with players in a different game.
    2. If new games are introduced, it will need to create new queue classes extending from `Rovio.Challenge.Matchmaking.Queues.BaseQueue<Game>` in order for that game to have its own queueing.
2. Matchmaking are NOT game specific (but can extend to have games implement its own matchmaking logic).
    1. Matchmaking rules for latency and queueing time are not game specific due to they're not naturally tied to games.
    2. Matchmaking rules can be extended to be game specific by creating concrete rules inherit from `Rovio.Challenge.Matchmaking.Engine.Matchmaker<Game>` and implementing `Rovio.Challenge.Matchmaking.Engine.IMatchmaker.StartMatchmakingProcess()` and add its own rules services/logic.
3. Matchmaking rules knows nothing regarding Games (but can be extended to be).
    1. Rules are not game specific, but rather specific to the actual value they're intended to validate.
    2. For creating new rules, you can do it by implementing `Rovio.Challenge.Matchmaking.Engine.Rules.IMatchmakingRule<struct>` to have it validated to the specific parametrized values. 
    3. `IMatchmakingRule<struct>` has struct constraint since the values to validate where merely non-reference ones (numbers, seconds, etc.).
    4. `IMatchmakingRule` has a property `AllowedDistance` that can be tweaked in order to increase/decrease the valid range for the values to be validated. Initial value is 10 but child classes can implement its own custom distance.
4. Database logic can be swaped to the desired db engine.
    1. Data logic is implemented at `Rovio.Challenge.Matchmaking.Database` and its isolated from the Repository logic, so it can be easily swaped to a different DB engine without heavily affecting the rest of the application logic (rules, queues, matchmaking, api, etc).
