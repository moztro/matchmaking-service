using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Rovio.Challenge.Matchmaking.Api.Models
{
	/// <summary>
	/// Payload request for /api/v1/game/{gameId}/join
	/// </summary>
	public class JoinGameRequest
	{
		/// <summary>
		/// The player's identifier
		/// </summary>
		[Required]
		public string PlayerUsername { get; set; }

		/// <summary>
		/// The player's latency
		/// </summary>
		[Required]
        public int PlayerLatency { get; set; }
	}
}

