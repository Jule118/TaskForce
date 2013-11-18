using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TaskForce.Network.Core
{
	/// <summary>
	/// Helper class to forward all required objects to the listen thread
	/// </summary>
	internal class ListenParameters
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="client">The client for the communication</param>
		public ListenParameters(TcpClient client)
		{
			Client = client;
		}

		/// <summary>
		/// The client for the communication
		/// </summary>
		public TcpClient Client { get; set; }
	}
}