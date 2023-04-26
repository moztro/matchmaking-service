using System;
using System.Net;

namespace Rovio.Challenge.Matchmaking.Utils
{
	public class IpGenerator
	{
		/// <summary>
		/// Randomly generates an ip address.
		/// </summary>
		/// <param name="ipv6">True if you want an ipv6 format, otherwise ipv4.</param>
		/// <returns></returns>
		public static string Generate(bool ipv6 = false)
		{
			var data = new byte[4];
			new Random().NextBytes(data);
			var ip = new IPAddress(data);
			if (ipv6)
				ip = ip.MapToIPv6();
			return ip.ToString();
		}
	}
}

