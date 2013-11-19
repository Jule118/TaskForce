using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using TaskForce.Entities;
using TaskForce.Network.Core;

namespace TaskForce.Network
{
	/// <summary>
	/// Helper class for the server sided network communication of TaskForce
	/// </summary>
	public class TaskForceServer : Server
	{
		private IList<Filter> _LastForbiddenFilterList = new List<Filter>();
		private IList<Filter> _LastProtectedFilterList = new List<Filter>();

		/// <summary>
		/// constructor
		/// </summary>
		public TaskForceServer()
			: base(1099)
		{ }

		/// <summary>
		/// Represents the method that will handle the ProtocolReceived event.
		/// </summary>
		/// <param name="protocol"></param>
		public delegate void ProtocolReceivedHandler(Protocol protocol);

		/// <summary>
		/// Occurs after a SendProtocol command received.
		/// </summary>
		public event ProtocolReceivedHandler ProtocolReceived;

		/// <summary>
		/// Forces the client to refresh his Forbbiden FilterList with the given filter list
		/// </summary>
		/// <param name="forbiddenFilterList">List of the new ProtectedFilters</param>
		public void RefreshForbiddenFilterLists(IList<Filter> forbiddenFilterList)
		{
			_LastForbiddenFilterList = forbiddenFilterList;

			NetworkPackage pack = new NetworkPackage(Command.RefreshForbiddenFilterList, forbiddenFilterList);

			foreach (TcpClient client in Clients)
				Send(client, pack);
		}

		/// <summary>
		/// Forces all clients to refresh his ProtectedFilterList with the given filter list
		/// </summary>
		/// <param name="protectedFilterList">List of the new ForbiddenFilters</param>
		public void RefreshProtectedFilterLists(IList<Filter> protectedFilterList)
		{
			_LastProtectedFilterList = protectedFilterList;

			NetworkPackage pack = new NetworkPackage(Command.RefreshProtectedFilterList, protectedFilterList);

			foreach (TcpClient client in Clients)
				Send(client, pack);
		}

		/// <summary>
		/// Methode which will be invoke by incoming messages
		/// </summary>
		/// <param name="package">The The received package</param>
		protected override void DataReceived(NetworkPackage package)
		{
			switch (package.Cmd)
			{
				case Command.SaveProtocol:
					Protocol protocol = package.Value as Protocol;
					if (protocol == null) throw new ArgumentException("Value must be of type 'Protocol'");

					ProtocolReceived((Protocol)package.Value);
					break;

				default:
					throw new ArgumentException(string.Format("Coudn't handle '{0}'", package.Cmd));
			}
		}

		/// <summary>
		/// Will be executed after a new client connected
		/// </summary>
		/// <param name="newClient">The new connected client</param>
		protected override void OnNewClient(TcpClient newClient)
		{
			NetworkPackage pack;

			pack = new NetworkPackage(Command.RefreshProtectedFilterList, _LastProtectedFilterList);
			Send(newClient, pack);

			pack = new NetworkPackage(Command.RefreshForbiddenFilterList, _LastForbiddenFilterList);
			Send(newClient, pack);
		}
	}
}