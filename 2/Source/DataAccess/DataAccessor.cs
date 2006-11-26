using System;
using System.Collections;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

using BLToolkit.Aspects;
using BLToolkit.Common;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace BLToolkit.DataAccess
{
	[DataAccessor]
	public abstract class DataAccessor : DataAccessBase
	{
		#region Constructors

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessor()
		{
		}

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessor(DbManager dbManager)
			: base(dbManager)
		{
		}

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessor(DbManager dbManager, bool dispose)
			: base(dbManager, dispose)
		{
		}

		#endregion

		#region CreateInstance

		public static DataAccessor CreateInstance(Type type)
		{
			return (DataAccessor)TypeAccessor.CreateInstance(type);
		}

		public static DataAccessor CreateInstance(Type type, DbManager dbManager)
		{
			return CreateInstance(type, dbManager, false);
		}

		public static DataAccessor CreateInstance(Type type, DbManager dbManager, bool dispose)
		{
			DataAccessor da = CreateInstance(type);

			da.SetDbManager(dbManager, dispose);

			return da;
		}

#if FW2
		[System.Diagnostics.DebuggerStepThrough]
		public static T CreateInstance<T>() where T : DataAccessor
		{
			return TypeAccessor<T>.CreateInstanceEx();
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static T CreateInstance<T>(DbManager dbManager)
			where T : DataAccessor
		{
			return CreateInstance<T>(dbManager, false);
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static T CreateInstance<T>(DbManager dbManager, bool dispose)
			where T : DataAccessor
		{
			T da = TypeAccessor<T>.CreateInstanceEx();

			da.SetDbManager(dbManager, dispose);

			return da;
		}
#endif

		#endregion

		#region Protected Members

		[NoInterception]
		protected virtual string GetSpParameterName(DbManager dbManager, string parameterName)
		{
			return (string)dbManager.DataProvider.Convert(parameterName, ConvertType.NameToParameter);
		}

		[NoInterception]
		protected virtual IDbDataParameter[] PrepareParameters(object[] parameters)
		{
			// Little optimization.
			// Check if we have only one single ref parameter.
			//
			object refParam = null;

			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i] != null)
				{
					if (refParam != null)
					{
						refParam = null;
						break;
					}

					refParam = parameters[i];
				}
			}

			if (refParam != null)
				return (IDbDataParameter[])refParam;

			ArrayList list = new ArrayList(parameters.Length);
			Hashtable hash = new Hashtable(parameters.Length);

			foreach (object o in parameters)
			{
				IDbDataParameter p = o as IDbDataParameter;

				if (p != null)
				{
					if (!hash.Contains(p.ParameterName))
					{
						list.Add(p);
						hash.Add(p.ParameterName, p);
					}
				}
				else if (o is IDbDataParameter[])
				{
					foreach (IDbDataParameter dbp in (IDbDataParameter[])o)
					{
						if (!hash.Contains(dbp.ParameterName))
						{
							list.Add(dbp);
							hash.Add(dbp.ParameterName, dbp);
						}
					}
				}
			}

			IDbDataParameter[] retParams = new IDbDataParameter[list.Count];

			list.CopyTo(retParams);

			return retParams;
		}

		protected void ExecuteDictionary(
			DbManager             db,
			IDictionary           dictionary,
			Type                  objectType,
			Type                  keyType,
			string                methodName)
		{
			bool       isIndex = TypeHelper.IsSameOrParent(typeof(CompoundValue), keyType);
			MemberMapper[] mms = new SqlQuery(Extensions).GetKeyFieldList(db, objectType);

			if (mms.Length == 0)
				throw new DataAccessException(string.Format(
					"Index is not defined for the method '{0}.{1}'.",
					GetType().Name, methodName));

			if (mms.Length > 1 && keyType != typeof(object) && !isIndex)
				throw new DataAccessException(string.Format(
					"Key type for the method '{0}.{1}' can be of type object or CompoundValue.",
					GetType().Name, methodName));

			if (isIndex || mms.Length > 1)
			{
				string[] fields = new string[mms.Length];

				for (int i = 0; i < mms.Length; i++)
					fields[i] = mms[i].MemberName;

				db.ExecuteDictionary(dictionary, new MapIndex(fields), objectType, null);
			}
			else
			{
				db.ExecuteDictionary(dictionary, mms[0].MemberName, objectType, null);
			}
		}

		protected void ExecuteScalarDictionary(
			DbManager             db,
			IDictionary           dictionary,
			Type                  objectType,
			Type                  keyType,
			string                methodName,
			NameOrIndexParameter  scalarField,
			Type                  elementType)
		{
			bool       isIndex = TypeHelper.IsSameOrParent(typeof(CompoundValue), keyType);
			MemberMapper[] mms = new SqlQuery(Extensions).GetKeyFieldList(db, objectType);

			if (mms.Length == 0)
				throw new DataAccessException(string.Format(
					"Index is not defined for the method '{0}.{1}'.",
					GetType().Name, methodName));

			if (mms.Length > 1 && keyType != typeof(object) && !isIndex)
				throw new DataAccessException(string.Format(
					"Key type for the method '{0}.{1}' can be of type object or CompoundValue.",
					GetType().Name, methodName));

			if (isIndex || mms.Length > 1)
			{
				string[] fields = new string[mms.Length];

				for (int i = 0; i < mms.Length; i++)
					fields[i] = mms[i].Name;

				db.ExecuteScalarDictionary(dictionary, new MapIndex(fields), scalarField, elementType);
			}
			else
			{
				db.ExecuteScalarDictionary(
					dictionary,
					mms[0].Name,
					keyType,
					scalarField,
					elementType);
			}
		}

		#endregion

		#region Convert

		#region Primitive Types

		[CLSCompliant(false)]
		[NoInterception]
		protected virtual SByte ConvertToSByte(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSByte(value);
		}

		[NoInterception]
		protected virtual Int16 ConvertToInt16(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToInt16(value);
		}

		[NoInterception]
		protected virtual Int32 ConvertToInt32(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToInt32(value);
		}

		[NoInterception]
		protected virtual Int64 ConvertToInt64(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToInt64(value);
		}

		[NoInterception]
		protected virtual Byte ConvertToByte(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToByte(value);
		}

		[CLSCompliant(false)]
		[NoInterception]
		protected virtual UInt16 ConvertToUInt16(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToUInt16(value);
		}

		[CLSCompliant(false)]
		[NoInterception]
		protected virtual UInt32 ConvertToUInt32(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToUInt32(value);
		}

		[CLSCompliant(false)]
		[NoInterception]
		protected virtual UInt64 ConvertToUInt64(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToUInt64(value);
		}

		[NoInterception]
		protected virtual Char ConvertToChar(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToChar(value);
		}

		[NoInterception]
		protected virtual Single ConvertToSingle(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSingle(value);
		}

		[NoInterception]
		protected virtual Double ConvertToDouble(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToDouble(value);
		}

		[NoInterception]
		protected virtual Boolean ConvertToBoolean(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToBoolean(value);
		}

		#endregion

		#region Simple Types

		[NoInterception]
		protected virtual String ConvertToString(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToString(value);
		}

		[NoInterception]
		protected virtual DateTime ConvertToDateTime(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToDateTime(value);
		}

		[NoInterception]
		protected virtual Decimal ConvertToDecimal(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToDecimal(value);
		}

		[NoInterception]
		protected virtual Guid ConvertToGuid(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToGuid(value);
		}

		[NoInterception]
		protected virtual Stream ConvertToStream(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToStream(value);
		}

		[NoInterception]
		protected virtual XmlReader ConvertToXmlReader(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToXmlReader(value);
		}

		#endregion

#if FW2

		#region Nullable Types

		[NoInterception]
		protected virtual Int16? ConvertToNullableInt16(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableInt16(value);
		}

		[NoInterception]
		protected virtual Int32? ConvertToNullableInt32(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableInt32(value);
		}

		[NoInterception]
		protected virtual Int64? ConvertToNullableInt64(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableInt64(value);
		}

		[NoInterception]
		protected virtual Byte? ConvertToNullableByte(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableByte(value);
		}

		[CLSCompliant(false)]
		[NoInterception]
		protected virtual UInt16? ConvertToNullableUInt16(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableUInt16(value);
		}

		[CLSCompliant(false)]
		[NoInterception]
		protected virtual UInt32? ConvertToNullableUInt32(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableUInt32(value);
		}

		[CLSCompliant(false)]
		[NoInterception]
		protected virtual UInt64? ConvertToNullableUInt64(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableUInt64(value);
		}

		[NoInterception]
		protected virtual Char? ConvertToNullableChar(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableChar(value);
		}

		[NoInterception]
		protected virtual Double? ConvertToNullableDouble(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableDouble(value);
		}

		[NoInterception]
		protected virtual Single? ConvertToNullableSingle(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableSingle(value);
		}

		[NoInterception]
		protected virtual Boolean? ConvertToNullableBoolean(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableBoolean(value);
		}

		[NoInterception]
		protected virtual DateTime? ConvertToNullableDateTime(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableDateTime(value);
		}

		[NoInterception]
		protected virtual Decimal? ConvertToNullableDecimal(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableDecimal(value);
		}

		[NoInterception]
		protected virtual Guid? ConvertToNullableGuid(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableGuid(value);
		}

		#endregion

#endif

		#region SqlTypes

		[NoInterception]
		protected virtual SqlByte ConvertToSqlByte(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlByte(value);
		}

		[NoInterception]
		protected virtual SqlInt16 ConvertToSqlInt16(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlInt16(value);
		}

		[NoInterception]
		protected virtual SqlInt32 ConvertToSqlInt32(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlInt32(value);
		}

		[NoInterception]
		protected virtual SqlInt64 ConvertToSqlInt64(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlInt64(value);
		}

		[NoInterception]
		protected virtual SqlSingle ConvertToSqlSingle(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlSingle(value);
		}

		[NoInterception]
		protected virtual SqlBoolean ConvertToSqlBoolean(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlBoolean(value);
		}

		[NoInterception]
		protected virtual SqlDouble ConvertToSqlDouble(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlDouble(value);
		}

		[NoInterception]
		protected virtual SqlDateTime ConvertToSqlDateTime(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlDateTime(value);
		}

		[NoInterception]
		protected virtual SqlDecimal ConvertToSqlDecimal(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlDecimal(value);
		}

		[NoInterception]
		protected virtual SqlMoney ConvertToSqlMoney(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlMoney(value);
		}

		[NoInterception]
		protected virtual SqlGuid ConvertToSqlGuid(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlGuid(value);
		}

		[NoInterception]
		protected virtual SqlString ConvertToSqlString(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlString(value);
		}

		#endregion

		#region General case

		[NoInterception]
		protected virtual object ConvertChangeType(
			DbManager db,
			object    value,
			Type      conversionType,
			object    parameter)
		{
			return db.MappingSchema.ConvertChangeType(value, conversionType);
		}

		#endregion
		
		#endregion
	}
}
