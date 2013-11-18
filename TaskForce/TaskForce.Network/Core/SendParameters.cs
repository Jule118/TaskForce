using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TaskForce.Network.Core
{
	/// <summary>
	/// Helper class to forward all required objects to the send thread
	/// </summary>
	internal class SendParameters
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="client">The client for the communication</param>
		/// <param name="package">The package to send</param>
		public SendParameters(TcpClient client, NetworkPackage package)
		{
			Package = package;
			Client = client;
		}

		/// <summary>
		/// The client for the communication
		/// </summary>
		public TcpClient Client { get; set; }

		/// <summary>
		/// The package to send
		/// </summary>
		public NetworkPackage Package { get; set; }
	}
}