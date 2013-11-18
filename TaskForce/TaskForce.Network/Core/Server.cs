using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace TaskForce.Network.Core
{
	/// <summary>
	/// Server class for Network communication
	/// </summary>
	public abstract class Server
	{
		private static bool _Listen = true;
		private int _Port;
		private TcpListener _Server;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="port">The port number for the network communication</param>
		protected Server(int port)
		{
			_Port = port;
			Clients = new List<TcpClient>();
		}

		/// <summary>
		/// List of all connected clients
		/// </summary>
		protected IList<TcpClient> Clients { get; set; }

		/// <summary>
		/// Starts the server
		/// </summary>
		public void Start()
		{
			_Listen = true;
			Thread th = new Thread(ListenConnections);
			th.Name = "ListenConnections";
			th.IsBackground = true;
			th.Start();
		}

		/// <summary>
		/// Stops the server
		/// </summary>
		public void Stop()
		{
			_Listen = false;
			_Server.Stop();
		}

		/// <summary>
		/// Methode which will be invoke by incoming messages
		/// </summary>
		/// <param name="package">The The received package</param>
		protected abstract void DataReceived(NetworkPackage package);

		/// <summary>
		/// Sends an object to the client
		/// </summary>
		/// <param name="client">The receiver</param>
		/// <param name="package">The package to send</param>
		protected void Send(TcpClient client, NetworkPackage package)
		{
			ThreadPool.QueueUserWorkItem(Send, new SendParameters(client, package));
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private void Listen(object para)
		{ //Thread: ListenConnections -> ClientListen
			if (para == null) throw new ArgumentNullException("para");

			ListenParameters data = para as ListenParameters;

			if (data == null) throw new ArgumentException("para must be of type ListenParameters");

			while (_Listen)
			{
				try
				{
					BinaryFormatter bFormatter = new BinaryFormatter();
					NetworkPackage package = bFormatter.Deserialize(data.Client.GetStream()) as NetworkPackage;

					if (package == null) throw new ArgumentException("package must be of type NetworkPackage");

					DataReceived(package);
				}
				catch (Exception)
				{
					//Connection was killed
					//Close TcpClient, Remove TcpClient from the ClientList and stopp the Listen-Thread
					data.Client.Close();
					Clients.Remove(data.Client);
					break;
				}
			}
		}

		private void ListenConnections()
		{ //Thread: ListenConnections
			_Server = new TcpListener(IPAddress.Any, _Port);
			_Server.Start();

			while (_Listen)
			{
				TcpClient client = _Server.AcceptTcpClient();
				Clients.Add(client);

				StartClientThread(client);
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private void Send(object para)
		{
			SendParameters data;

			if (para == null) throw new ArgumentNullException("para");

			data = para as SendParameters;

			if (data == null) throw new ArgumentException("para must be of type SendParameters");

			try
			{
				BinaryFormatter bFormatter = new BinaryFormatter();

				bFormatter.Serialize(data.Client.GetStream(), data.Package);
			}
			catch (Exception)
			{ /* Do Nothing the Connection will be destroyde in the Listen Thread */ }
		}

		private void StartClientThread(TcpClient client)
		{
			Thread th = new Thread(Listen);
			th.Name = "ClientListen " + Clients.Count;
			th.IsBackground = true;
			th.Start(new ListenParameters(client));
		}
	}
}