using System;
using System.Collections.Generic;
using System.Text;
using TaskForce.Database.Core;

namespace TaskForce.Entities
{
	[Serializable]
	public class Account: ORMapper<Account>
	{
		[DBField("ID")]
		public int ID { get; set; }
		[DBField("LoginName")]
		public string LoginName { get; set; }
		[DBField("LastLogin")]
		public DateTime LastLogin { get; set; }
	}
}