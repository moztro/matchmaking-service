using Rovio.Challenge.Matchmaking.Api.Models;
using Rovio.Challenge.Matchmaking.Domain.Models;

namespace Rovio.Challenge.Matchmaking.Api.Extensions
{
	public static class PlayerModelExtensions
	{
		public static Player ToPlayer(this JoinGameRequest joinRequest)
		{
			if (joinRequest == null)
				throw new ArgumentNullException(nameof(joinRequest));

			return new Player
			{
				Username = joinRequest.PlayerUsername,
				Latency = joinRequest.PlayerLatency,
				Region = joinRequest.PlayersRegion
			};
		}
	}
}

