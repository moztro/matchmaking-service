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
- Dequeue a player. When player decided to stop searching for a game to join.
- Assign player to session. Returns a game session with the player added to it.
- Join an ongoing session. Players can join a session after it has started.

### out of scope
- Skill match. It won't consider player's skill in order to match.
- Allow/Deny. To block or allow certain players to join a session, its out of initial scope.
- Auto-cancellation. Players cannot cancel themselves a join request after it has started.



  