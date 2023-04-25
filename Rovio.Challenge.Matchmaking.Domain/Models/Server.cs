﻿using System;
using Rovio.Challenge.Matchmaking.Domain.Enums;

namespace Rovio.Challenge.Matchmaking.Domain.Models
{
	/// <summary>
	/// The server hosting game sessions.
	/// </summary>
	public class Server
	{
		/// <summary>
		/// The IP address for the server.
		/// </summary>
		public string Ip { get; set; }

		/// <summary>
		/// The world region where this server is hosted.
		/// </summary>
		public Region Region { get; set; }

		/// <summary>
		/// The sessions being hosted in the server.
		/// </summary>
		public IEnumerable<Session> Sessions { get; set; }
	}
}

