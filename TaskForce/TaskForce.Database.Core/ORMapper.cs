using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace TaskForce.Database.Core
{
	/// <summary>
	/// Helper class to build Businessobjects out of the Database
	/// </summary>
	[Serializable]
	public abstract class ORMapper<T>
	{
		/// <summary>
		/// Fills the Object with the data from the reader
		/// </summary>
		public static Func<IDataReader, T> Create = rdr =>
		{
			// create an instance of the type provided
			T item = Activator.CreateInstance<T>();
			string missingFields = string.Empty;

			foreach (PropertyCached property in _Cache)
			{
				try
				{
					if (rdr[property.DBFieldName] == DBNull.Value)
					{
						if (property.IsNullable)
						{
							property.Prop.SetValue(item,
								null,
								null);
						}
						else
							throw new DBException(string.Format("The type '{0}' can't be null", property.Prop.PropertyType.FullName));
					}
					else
					{
						//set the value of the property
						if (IsSameType(property, (rdr[property.DBFieldName].GetType())))
						{
							property.Prop.SetValue(item,
								rdr[property.DBFieldName] == DBNull.Value ?
									null : rdr[property.DBFieldName],
								null);
						}
						else
						{
							try
							{
								//chane the type of the data in table to that of the property and set the value
								property.Prop.SetValue(item,
									Convert.ChangeType(rdr[property.DBFieldName] == DBNull.Value ?
										null : rdr[property.DBFieldName],
									property.Prop.PropertyType),
									null);
							}
							catch (Exception)
							{
								throw new DBException(string.Format("The types '{0}' (Class) and '{1}' (DB) aren't compatible", property.Prop.PropertyType.FullName, rdr[property.DBFieldName].GetType().FullName));
							}
						}
					}
				}
				catch (Exception)
				{
					if (string.IsNullOrEmpty(missingFields))
						missingFields += property.DBFieldName;
					else
						missingFields += " / " + property.DBFieldName;
				}
			}

			if (!string.IsNullOrEmpty(missingFields))
				throw new DBException(string.Format(@"The DBField parameter '{0}' of the object '{1}' does not exist in the database",
					missingFields.Remove(missingFields.Length - 2, 1)));

			return item;
		};

		private static List<PropertyCached> _Cache;

		private static Type _Type = typeof(T);

		/// <summary>
		/// Constructor
		/// </summary>
		protected ORMapper()
		{
			Map();
		}

		/// <summary>
		/// Gets or sets the spezific property of the object
		/// </summary>
		/// <param name="dbFieldName">The dbFieldName of the property</param>
		/// <returns></returns>
		protected object this[string dbFieldName]
		{
			get { return _Cache.Find(c => c.DBFieldName == dbFieldName).Prop.GetValue(this, null); }
			set { _Cache.Find(c => c.DBFieldName == dbFieldName).Prop.SetValue(this, value, null); }
		}

		private static bool IsSameType(PropertyCached propertyCached, Type dbType)
		{
			if (propertyCached.IsNullable)
			{
				return Nullable.GetUnderlyingType(propertyCached.Prop.PropertyType) == dbType;
			}
			else
			{
				return (propertyCached.Prop.PropertyType == dbType) || (propertyCached.IsFlag && dbType == typeof(int));
			}
		}

		/// <summary>
		/// Maps the properties with the db fields
		/// </summary>
		private static void Map()
		{
			_Cache = new List<PropertyCached>();
			PropertyInfo[] properties = _Type.GetProperties();

			foreach (PropertyInfo property in properties)
			{
				//for each property declared in the tyoe provided check if the property
				//decorated with the DBField attribute
				if (Attribute.IsDefined(property, typeof(DBFieldAttribute)))
				{
					PropertyCached newDBProp = new PropertyCached();
					newDBProp.Prop = property;

					#region Get DBField

					DBFieldAttribute attribute = (DBFieldAttribute)Attribute.GetCustomAttribute(property, typeof(DBFieldAttribute));
					newDBProp.DBFieldName = attribute.FieldName;

					#endregion Get DBField

					#region Check IsNullable

					if (newDBProp.Prop.PropertyType.IsGenericType && newDBProp.Prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
						newDBProp.IsNullable = true;
					else
						newDBProp.IsNullable = false;

					#endregion Check IsNullable

					#region Check IsFlag

					newDBProp.IsFlag = Attribute.IsDefined(newDBProp.Prop.PropertyType, typeof(FlagsAttribute));

					#endregion Check IsFlag

					_Cache.Add(newDBProp);
				}
			}
		}

		private class PropertyCached
		{
			public string DBFieldName { get; set; }

			public bool IsFlag { get; set; }

			public bool IsNullable { get; set; }

			public PropertyInfo Prop { get; set; }
		}
	}
}