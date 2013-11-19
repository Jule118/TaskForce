using System;
using System.Collections.Generic;
using System.Text;
using TaskForce.Database.Core;

namespace TaskForce.Entities
{
	[Serializable]
	public class Filter : ORMapper<Filter>
	{
		[Flags]
		public enum FilterType
		{
			None = 0,
			Protected = 1,
			Forbidden = 2
		}

		[DBField("Account")]
		public int Account { get; set; }

		[DBField("Active")]
		public bool Active { get; set; }

		[DBField("FilterGroup")]
		public int? FilterGroup { get; set; }

		[DBField("ID")]
		public int ID { get; set; }

		[DBField("Name")]
		public string Name { get; set; }

		[DBField("ProcessName")]
		public string ProcessName { get; set; }

		[DBField("Type")]
		public FilterType Type { get; set; }
	}
}