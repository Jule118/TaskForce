using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace TaskForce.Database.Core
{
	[Serializable]
	public class DBException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the DBException class.
		/// </summary>
		public DBException()
			: base()
		{ }

		/// <summary>
		/// Initializes a new instance of the DBException class with a specified error
		/// message.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public DBException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the DBException class with a specified error message
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current
		/// exception.</param>
		public DBException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		/// <summary>
		/// Initializes a new instance of the DBException class with a specified error message
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="sql">The original SQL-Query which caused the error.</param>
		/// <param name="param">The parameter of the SQL-Query.</param>
		/// <param name="innerException">The exception that is the cause of the current
		/// exception.</param>
		public DBException(string sql, IList<DBParameter> param, Exception innerException)
			: base("DB Error", innerException)
		{
			SQL = sql;
			Params = param;
		}

		/// <summary>
		/// Initialize a new instance of the ORMapperException class with Serialization data.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected DBException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }

		/// <summary>
		/// The parameter of the SQL-Query
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public IList<DBParameter> Params { get; private set; }

		/// <summary>
		/// The original SQL-Query which caused the error
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public string SQL { get; private set; }

		// GetObjectData performs a custom serialization.
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info,
			StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			info.AddValue("SQL", SQL);

			base.GetObjectData(info, context);
		}
	}
}