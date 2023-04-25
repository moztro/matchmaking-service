using System;
using System.Text.Json.Serialization;

namespace Rovio.Challenge.Matchmaking.Api.Models
{
    /// <summary>
    /// Payload response for /api/v1/game/{gameId}/join
    /// </summary>
    public class JoinGameResponse
	{
        public JoinGameResponse(string sessionId)
        {
            SessionId = sessionId;
        }

        /// <summary>
        /// The session game id the player joined.
        /// </summary>
        public string SessionId { get; set; }
	}
}

