using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskForce.Database;
using TaskForce.Database.Core;
using TaskForce.Entities;
using TaskForce.Network;

namespace CServerTest
{
	/*
	*
	*
	*			DEMO/Test Project
	*
	*/

	internal class Program
	{
		private static TaskForceDB _DB;
		private static Account _Me;
		private static TaskForceServer _Server;

		private static void Main(string[] args)
		{
			_DB = new TaskForceDB(DB.DbProvider.SqlClient, "Server=Server12\\Server42;Database=TaskForce;Trusted_Connection=True;");

			_Me = _DB.GetAccount("OHM\\DSchapal");

			_Server = new TaskForceServer();
			_Server.ProtocolReceived += server_ProtocolReceived;
			_Server.Start();

			IList<Filter> filterList = _DB.LstProtectedFilter(_Me.ID).ToList();

			_Server.RefreshProtectedFilterLists(filterList);

			Console.ReadLine();
		}

		private static void server_ProtocolReceived(Protocol protocol)
		{
			_DB.InsProtocol(_Me.ID, protocol.Filter, protocol.ClientIP);
		}
	}
}