using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TaskForce.Database.Core
{
	public class DBParameter
	{
		private ParameterDirection _Direction = ParameterDirection.Input;
		private string _ParameterName = String.Empty;
		private object _Value = null;

		/// <summary>
		/// Gets or sets a value that indicates whether the parameter is input-only,
		//  output-only, bidirectional, or a stored procedure return value parameter.
		/// </summary>
		public ParameterDirection Direction
		{
			get
			{
				return _Direction;
			}
			set
			{
				_Direction = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the System.Data.Common.DbParameter.
		/// </summary>
		public string ParameterName
		{
			get
			{
				return _ParameterName;
			}
			set
			{
				_ParameterName = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of the parameter.
		/// </summary>
		public object Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value = value;
			}
		}
	}
}