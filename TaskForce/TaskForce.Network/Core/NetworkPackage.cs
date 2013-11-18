using System;
using System.Collections.Generic;
using System.Text;

namespace TaskForce.Network.Core
{
	/// <summary>
	/// Hekper class to send packages over the network
	/// </summary>
	[Serializable]
	public class NetworkPackage
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cmd">The package command to specify the value content</param>
		/// <param name="value">The object to send</param>
		public NetworkPackage(Command cmd, object value)
		{
			Cmd = cmd;
			Value = value;
		}

		/// <summary>
		/// The package command to specify the value content
		/// </summary>
		public Command Cmd { get; set; }

		/// <summary>
		/// The object to send
		/// </summary>
		public object Value { get; set; }
	}
}