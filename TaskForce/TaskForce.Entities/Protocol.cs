using System;
using System.Collections.Generic;
using System.Text;
using TaskForce.Database.Core;

namespace TaskForce.Entities
{
	[Serializable]
	public class Protocol : ORMapper<Protocol>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Protocol()
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="clientIP">The client IP where the violation occured</param>
		/// <param name="filter">The filter which was violated</param>
		public Protocol(string clientIP, int filter)
		{
			ClientIP = clientIP;
			Filter = filter;
		}

		[DBField("Account")]
		public int Account { get; set; }

		[DBField("ClientIP")]
		public string ClientIP { get; set; }

		[DBField("Created")]
		public DateTime Created { get; set; }

		[DBField("Filter")]
		public int Filter { get; set; }

		[DBField("ID")]
		public int ID { get; set; }
	}
}