using System;
using System.Collections.Generic;
using System.Text;

namespace TaskForce.Database.Core
{
	/// <summary>
	/// Specifies the name of the field in the table that the property maps to
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DBFieldAttribute : Attribute
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fieldName">name of the field that the property will be mapped to</param>
		public DBFieldAttribute(string fieldName)
		{
			FieldName = fieldName;
		}

		/// <summary>
		/// The field name in the database table
		/// </summary>
		public string FieldName { get; private set; }
	}
}