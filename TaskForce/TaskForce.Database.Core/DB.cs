using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace TaskForce.Database.Core
{
	/// <summary>
	/// Helper class to communicate with an database
	/// </summary>
	public class DB
	{
		private static DbProviderFactory _Factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
		private Dictionary<DbProvider, string> _DbProviderDic = new Dictionary<DbProvider, string>();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dbFactory">The DBPrivider which should be used</param>
		/// <param name="connectionString">The ConnectionString to the Database</param>
		public DB(DbProvider dbFactory, string connectionString)
		{
			try
			{
				InitializeDBProviderDictionary();

				ConnectionString = connectionString;
				_Factory = DbProviderFactories.GetFactory(_DbProviderDic[dbFactory]);
			}
			catch (Exception ex)
			{
				throw new DBException("Error getting DbFactory" + (string.IsNullOrEmpty(dbFactory.ToString()) ? "." : ": " + dbFactory.ToString()),
					ex);
			}
		}

		/// <summary>
		/// List of alle supported DBProvider
		/// </summary>
		public enum DbProvider
		{
			/// <summary>
			/// Odbc
			/// </summary>
			Odbc,

			/// <summary>
			/// OleDb
			/// </summary>
			OleDb,

			/// <summary>
			/// OracleClient
			/// </summary>
			OracleClient,

			/// <summary>
			/// SqlClient
			/// </summary>
			SqlClient,

			/// <summary>
			/// SqlServerCe
			/// </summary>
			SqlServerCe
		}

		/// <summary>
		/// ConnectionString to the Database
		/// </summary>
		public string ConnectionString { get; private set; }

		/// <summary>
		/// Executes a sql query against the configured database
		/// </summary>
		/// <param name="query">Query to execute</param>
		/// <param name="param">Parameter for the query</param>
		public void ExecuteQuery(string query, IList<DBParameter> param)
		{
			try
			{
				using (DbConnection connection = CreateConnection())
				{
					using (DbCommand command = CreateCommand(query, connection, param))
					{
						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				throw new DBException(query, ex);
			}
		}

		/// <summary>
		/// Executes a stored procedure on the configured database
		/// </summary>
		/// <param name="procedure">The name of the procedure</param>
		/// <param name="param">Parameter for the procedure</param>
		public IDataReader ExecuteStoredProcedure(string procedure, IList<DBParameter> param)
		{
			try
			{
				DbConnection connection = CreateConnection();

				using (DbCommand command = CreateCommand(procedure, connection, param))
				{
					command.CommandType = CommandType.StoredProcedure;
					return command.ExecuteReader();
				}
			}
			catch (Exception ex)
			{
				throw new DBException(procedure, param, ex);
			}
		}

		private static DbCommand CreateCommand(string sql, DbConnection conn, IList<DBParameter> param)
		{
			var command = _Factory.CreateCommand();
			command.Connection = conn;
			command.CommandText = sql;

			if (param != null)
			{
				foreach (DBParameter parm in param)
				{
					DbParameter para = command.CreateParameter();
					para.Direction = parm.Direction;
					para.ParameterName = parm.ParameterName;
					para.Value = parm.Value;
					command.Parameters.Add(para);
				}
			}

			return command;
		}

		private DbConnection CreateConnection()
		{
			var connection = _Factory.CreateConnection();
			connection.ConnectionString = ConnectionString;
			connection.Open();
			return connection;
		}

		private void InitializeDBProviderDictionary()
		{
			_DbProviderDic.Add(DbProvider.Odbc, "System.Data.Odbc");
			_DbProviderDic.Add(DbProvider.OleDb, "System.Data.OleDb");
			_DbProviderDic.Add(DbProvider.OracleClient, "System.Data.OracleClient");
			_DbProviderDic.Add(DbProvider.SqlClient, "System.Data.SqlClient");
			_DbProviderDic.Add(DbProvider.SqlServerCe, "System.Data.SqlServerCe");
		}
	}
}