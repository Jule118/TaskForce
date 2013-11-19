using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskForce.Entities;
using TaskForce.Network;

namespace ClientTest
{
	/*
	*
	*
	*			DEMO/Test Project
	*
	*/

	internal class Program
	{
		private static TaskForceClient _Client;

		private static void _Client_ProtectedFilterListRefreshed(IList<Filter> filterList)
		{
			IList<Filter> filter = filterList;
		}

		private static void Main(string[] args)
		{
			_Client = new TaskForceClient("127.0.0.1");
			_Client.ProtectedFilterListRefreshed += _Client_ProtectedFilterListRefreshed;
			_Client.Connect();

			_Client.SaveProtocol(new Protocol("127.0.0.1", 1));

			Console.ReadLine();

			_Client.Disconnect();

			Console.ReadLine();

			_Client.Connect();

			Console.ReadLine();
		}
	}
}