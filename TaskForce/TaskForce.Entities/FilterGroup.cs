using System;
using System.Collections.Generic;
using System.Text;
using TaskForce.Database.Core;

namespace TaskForce.Entities
{
	[Serializable]
	public class FilterGroup : ORMapper<FilterGroup>
	{
		[DBField("ID")]
		public int ID { get; set; }

		[DBField("Name")]
		public string Name { get; set; }

		[DBField("Account")]
		public int? Account { get; set; }
	}
}