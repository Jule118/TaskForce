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
	/// Client class for Network communication
	/// </summary>
	public abstract class Client
	{
		private static bool _Listen = false;
		private static bool _TryReconnect = false;
		private BinaryFormatter _BFormatter = new BinaryFormatter();
		private TcpClient _Client;
		private IPEndPoint _Endpoint;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="port">The port number for the network communication</param>
		/// <param name="serverIP">The server ip</param>
		protected Client(int port, string serverIP)
		{
			_Endpoint = new IPEndPoint(IPAddress.Parse(serverIP), port);
		}

		/// <summary>
		/// Established a new connection to the server
		/// </summary>
		/// <returns>Returns true after a sucessful connection</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public bool Connect()
		{
			try
			{
				if (_Listen || _TryReconnect)
					throw new ProtocolViolationException("The Client is already connected. Please disconnect before reconnect");

				_Listen = true;
				_Client = new TcpClient(AddressFamily.InterNetwork);
				_Client.Connect(_Endpoint);

				Thread th = new Thread(Listen);
				th.Name = "ClientListen";
				th.IsBackground = true;
				th.Start();

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Ends the connection with the server
		/// </summary>
		public void Disconnect()
		{
			_Listen = false;
			_TryReconnect = false;
		}

		/// <summary>
		/// Methode which will be invoke by incoming messages
		/// </summary>
		/// <param name="package"></param>
		protected abstract void DataReceived(NetworkPackage package);

		/// <summary>
		/// Sends an object to the server
		/// </summary>
		/// <param name="package">The package to send</param>
		protected void Send(NetworkPackage package)
		{
			ThreadPool.QueueUserWorkItem(Send, new SendParameters(_Client, package));
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private void Listen()
		{ //Thread: ClientListen
			while (_Listen)
			{
				try
				{
					NetworkPackage package = _BFormatter.Deserialize(_Client.GetStream()) as NetworkPackage;

					if (package == null) throw new ArgumentException("package must be of type NetworkPackage");

					DataReceived(package);
				}
				catch (Exception)
				{
					//Connection was killed
					//Close TcpClient and stopp Listen-Thread
					_Client.Close();
					_Listen = false;
					_TryReconnect = true;

					Thread th = new Thread(Reconnect);
					th.IsBackground = true;
					th.Name = "Reconnect";
					th.Start();
				}
			}
		}

		private void Reconnect()
		{
			while (_TryReconnect)
			{
				Thread.Sleep(5000);

				_TryReconnect = !Connect();
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private void Send(object para)
		{ //Thread: WorkerThread
			try
			{
				if (para == null) throw new ArgumentNullException("para");

				SendParameters data = para as SendParameters;

				if (data == null) throw new ArgumentException("para must be of type SendParameters");

				BinaryFormatter bFormatter = new BinaryFormatter();

				bFormatter.Serialize(data.Client.GetStream(), data.Package);
			}
			catch (Exception)
			{ /* Do Nothing the Connection will be destroyde in the Listen Thread */ }
		}
	}
}