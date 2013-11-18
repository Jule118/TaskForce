using System;
using System.Collections.Generic;
using System.Text;
using TaskForce.Entities;
using TaskForce.Network.Core;

namespace TaskForce.Network
{
	/// <summary>
	/// Helper class for the client sided network communication of TaskForce
	/// </summary>
	public class TaskForceClient : Client
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="serverIP">The IP of the server</param>
		public TaskForceClient(string serverIP)
			: base(1099, serverIP)
		{ }

		/// <summary>
		/// Represents the method that will handle a FilterRefreshed event.
		/// </summary>
		/// <param name="protocol"></param>
		public delegate void FilterRefresgedHandler(IList<Filter> filterList);

		/// <summary>
		/// Occurs after a RefreshForbiddendFilterList command received.
		/// </summary>
		public event FilterRefresgedHandler ForbiddenFilterListRefreshed;

		/// <summary>
		/// Occurs after a RefreshProtectedFilterList command received.
		/// </summary>
		public event FilterRefresgedHandler ProtectedFilterListRefreshed;

		/// <summary>
		/// Returns the server ip
		/// </summary>
		/// <returns></returns>
		public static string GetServerIP()
		{
			return "127.0.0.1";
		}

		/// <summary>
		/// Sends an Protocol object to the server
		/// </summary>
		/// <param name="protocol">The Protocol to send</param>
		public void SaveProtocol(Protocol protocol)
		{
			NetworkPackage pack = new NetworkPackage(Command.SaveProtocol, protocol);

			Send(pack);
		}

		/// <summary>
		/// Methode which will be invoke by incoming messages
		/// </summary>
		/// <param name="package">The The received package</param>
		protected override void DataReceived(NetworkPackage package)
		{
			switch (package.Cmd)
			{
				case Command.RefreshProtectedFilterList:
					IList<Filter> protectedList = package.Value as IList<Filter>;
					if (protectedList == null) throw new ArgumentException("Value must be of type 'IList<Filter>'");

					ProtectedFilterListRefreshed(protectedList);
					break;

				case Command.RefreshForbiddenFilterList:
					IList<Filter> forbiddenList = package.Value as IList<Filter>;
					if (forbiddenList == null) throw new ArgumentException("Value must be of type 'IList<Filter>'");

					ForbiddenFilterListRefreshed(forbiddenList);
					break;

				default:
					throw new ArgumentException(string.Format("Coudn't handle '{0}'", package.Cmd));
			}
		}
	}
}