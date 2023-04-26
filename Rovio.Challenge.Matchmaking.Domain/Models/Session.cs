using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rovio.Challenge.Matchmaking.Domain.Models
{
	/// <summary>
	/// Represents an active game session.
	/// </summary>
	public class Session
	{
		/// <summary>
		/// Session identificator.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		/// <summary>
		/// The game which this session belongs to.
		/// </summary>
		public Game Game { get; set; }

		/// <summary>
		/// The players who have joined the game session.
		/// </summary>
		public IEnumerable<Player> Players { get; set; } = Enumerable.Empty<Player>();
	}
}

