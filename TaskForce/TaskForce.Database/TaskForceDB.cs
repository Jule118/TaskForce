using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TaskForce.Database.Core;
using TaskForce.Entities;

namespace TaskForce.Database
{
	/// <summary>
	/// Helperclass to communicate with an TaskForce database
	/// </summary>
	public class TaskForceDB : DB
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dbProvider">The DBPrivider which should be used</param>
		/// <param name="connectionStr">The ConnectionString to the Database</param>
		public TaskForceDB(DbProvider dbProvider, string connectionStr)
			: base(dbProvider, connectionStr)
		{ }

		/// <summary>
		/// Deletes the account with the given loginname
		/// </summary>
		/// <param name="loginName">LoginName of the account which should be deleted</param>
		public void DelAccount(string loginName)
		{
			ExecuteStoredProcedure("DelAccount",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@LoginName", Value = loginName }}).Close();
		}

		/// <summary>
		/// Deletes the filter with the given ID
		/// </summary>
		/// <param name="filter">ID of the filter which should be deleted</param>
		public void DelFilter(int filter)
		{
			ExecuteStoredProcedure("DelFilter",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@Filter", Value = filter }}).Close();
		}

		/// <summary>
		/// Deletes the filtergroup with the given ID
		/// </summary>
		/// <param name="filterGroup">ID of the filtergroup which should be deleted</param>
		public void DelFilterGroup(int filterGroup)
		{
			ExecuteStoredProcedure("DelFilterGroup",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@FilterGroup", Value = filterGroup }}).Close();
		}

		/// <summary>
		/// Gets the account with the given loginname
		/// </summary>
		/// <param name="loginName">The loginname of the account</param>
		/// <returns></returns>
		public Account GetAccount(string loginName)
		{
			Account acc = null;

			using (IDataReader rdr = ExecuteStoredProcedure("GetAccount",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@LoginName", Value = loginName }}))
			{
				while (rdr.Read())
					acc = Account.Create(rdr);
			}

			return acc;
		}

		/// <summary>
		/// Inserts a new account
		/// </summary>
		/// <param name="loginName">Loginame of the new account</param>
		public void InsAccount(string loginName)
		{
			ExecuteStoredProcedure("InsAccount",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@LoginName", Value = loginName }}).Close();
		}

		/// <summary>
		/// Inserts a new filter
		/// </summary>
		/// <param name="account">The ID of the creater (0 Admin)</param>
		/// <param name="name">A name for the filter</param>
		/// <param name="filterType">The filtertype of the filter</param>
		/// <param name="processName">The processname of the filter</param>
		/// <param name="filterGroup">The filtergroup where the filter should be displayed</param>
		public void InsFilter(int account, string name, Filter.FilterType filterType, string processName, int? filterGroup)
		{
			ExecuteStoredProcedure("InsFilter",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@Account", Value = account },
					new DBParameter() { ParameterName = "@Name", Value = name },
					new DBParameter() { ParameterName = "@FilterType", Value = (int)filterType },
					new DBParameter() { ParameterName = "@ProcessName", Value = processName },
					new DBParameter() { ParameterName = "@FilterGroup", Value = filterGroup }}).Close();
		}

		/// <summary>
		/// Inserts a new filtergroup
		/// </summary>
		/// <param name="account">The ID of the creater (0 Admin)</param>
		/// <param name="name">The name of the filtergroup</param>
		public void InsFilterGroup(int account, string name)
		{
			ExecuteStoredProcedure("InsFilterGroup",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@Account", Value = account },
					new DBParameter() { ParameterName = "@Name", Value = name }}).Close();
		}

		/// <summary>
		/// Inserts a new protocol
		/// </summary>
		/// <param name="account">The ID of the creater</param>
		/// <param name="filter">The violated filter</param>
		/// <param name="clientIP">The ip of the client</param>
		public void InsProtocol(int account, int filter, string clientIP)
		{
			ExecuteStoredProcedure("InsProtocol",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@Account", Value = account },
					new DBParameter() { ParameterName = "@Filter", Value = filter },
					new DBParameter() { ParameterName = "@ClientIP", Value = clientIP }}).Close();
		}

		/// <summary>
		/// Gets the filtergroup list of the given account
		/// </summary>
		/// <param name="account">The account</param>
		/// <returns></returns>
		public IEnumerable<FilterGroup> LstFilterGroup(int account)
		{
			using (IDataReader rdr = ExecuteStoredProcedure("LstFilterGroup",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@Account", Value = account }}))
			{
				while (rdr.Read())
					yield return FilterGroup.Create(rdr);
			}
		}

		/// <summary>
		/// Gets the forbiddenfilter list of the given account
		/// </summary>
		/// <param name="account">The account</param>
		/// <returns></returns>
		public IEnumerable<Filter> LstForbiddenFilter(int account)
		{
			using (IDataReader rdr = ExecuteStoredProcedure("LstForbiddenFilter",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@Account", Value = account }}))
			{
				while (rdr.Read())
					yield return Filter.Create(rdr);
			}
		}

		/// <summary>
		/// Gets the protectedfilter list of the given account
		/// </summary>
		/// <param name="account">The account</param>
		/// <returns></returns>
		public IEnumerable<Filter> LstProtectedFilter(int account)
		{
			using (IDataReader rdr = ExecuteStoredProcedure("LstProtectedFilter",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@Account", Value = account }}))
			{
				while (rdr.Read())
					yield return Filter.Create(rdr);
			}
		}

		/// <summary>
		/// Gets the filtergroup list of the given account
		/// </summary>
		/// <param name="account">The account</param>
		/// <param name="from">The lower datelimiter</param>
		/// <param name="to">The upper datelimiter</param>
		/// <returns></returns>
		public IEnumerable<Protocol> LstProtocol(int account, DateTime? from, DateTime? to)
		{
			using (IDataReader rdr = ExecuteStoredProcedure("LstProtocol",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@Account", Value = account },
					new DBParameter() { ParameterName = "@From", Value = from },
					new DBParameter() { ParameterName = "@To", Value = to }}))
			{
				while (rdr.Read())
					yield return Protocol.Create(rdr);
			}
		}

		/// <summary>
		/// Updates the active status of the filter
		/// </summary>
		/// <param name="account">The account</param>
		/// <param name="filter">The filter which should be updated</param>
		/// <param name="status">The new status of the filter</param>
		public void UpdFilterStatus(int account, int filter, bool status)
		{
			ExecuteStoredProcedure("InsProtocol",
				new List<DBParameter>(){
					new DBParameter() { ParameterName = "@Account", Value = account },
					new DBParameter() { ParameterName = "@Filter", Value = filter },
					new DBParameter() { ParameterName = "@Status", Value = status }}).Close();
		}
	}
}