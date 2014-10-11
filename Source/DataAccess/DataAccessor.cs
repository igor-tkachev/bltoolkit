using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
#if !SILVERLIGHT
using System.Xml.Linq;
#endif

namespace BLToolkit.DataAccess
{
	using Aspects;
	using Common;
	using Data;
	using Data.DataProvider;
	using Mapping;
	using Patterns;
	using Properties;
	using Reflection;
	using TypeBuilder;

	[DataAccessor, DebuggerStepThrough]
	public abstract class DataAccessor : DataAccessorBase
	{
		#region Constructors

		protected DataAccessor()
		{
		}

		protected DataAccessor(DbManager dbManager)
			: base(dbManager)
		{
		}

		protected DataAccessor(DbManager dbManager, bool dispose)
			: base(dbManager, dispose)
		{
		}

		#endregion

		#region CreateInstance

		public static DataAccessor CreateInstance(Type type)
		{
			return (DataAccessor)Activator.CreateInstance(TypeFactory.GetType(type));
		}

		public static DataAccessor CreateInstance(Type type, InitContext context)
		{
			return (DataAccessor)Activator.CreateInstance(TypeFactory.GetType(type), context);
		}

		public static DataAccessor CreateInstance(Type type, DbManager dbManager)
		{
			return CreateInstance(type, dbManager, false);
		}

		public static DataAccessor CreateInstance(
			Type type,
			InitContext context,
			DbManager dbManager)
		{
			return CreateInstance(type, context, dbManager, false);
		}

		public static DataAccessor CreateInstance(Type type, DbManager dbManager, bool dispose)
		{
			var da = CreateInstance(type);

			da.SetDbManager(dbManager, dispose);

			return da;
		}

		public static DataAccessor CreateInstance(
			Type type,
			InitContext context,
			DbManager dbManager,
			bool dispose)
		{
			var da = CreateInstance(type, context);

			da.SetDbManager(dbManager, dispose);

			return da;
		}

		public static T CreateInstance<T>() where T : DataAccessor
		{
			return TypeFactory.CreateInstance<T>();
		}

		public static T CreateInstance<T>(DbManager dbManager)
			where T : DataAccessor
		{
			return CreateInstance<T>(dbManager, false);
		}

		public static T CreateInstance<T>(DbManager dbManager, bool dispose)
			where T : DataAccessor
		{
			var da = TypeFactory.CreateInstance<T>();

			da.SetDbManager(dbManager, dispose);

			return da;
		}

		#endregion

		#region Protected Members

		#region Parameters

		[NoInterception]
		protected virtual string GetQueryParameterName(DbManager db, string paramName)
		{
			return (string)db.DataProvider.Convert(paramName, ConvertType.NameToQueryParameter);
		}

		[NoInterception]
		protected virtual string GetSpParameterName(DbManager db, string paramName)
		{
			return (string)db.DataProvider.Convert(paramName, db.GetConvertTypeToParameter());
		}

		[NoInterception]
		protected virtual IDbDataParameter[] PrepareParameters(DbManager db, object[] parameters)
		{
			return db.PrepareParameters(parameters);
		}

		[NoInterception]
		protected virtual IDbDataParameter GetParameter(DbManager db, string paramName)
		{
			var p = db.Parameter(paramName);

			if (p == null)
			{
				// This usually means that the parameter name is incorrect.
				//
				throw new DataAccessException(string.Format(
					Resources.DataAccessot_ParameterNotFound, paramName));
			}

			// Input parameter mapping make no sence.
			//
			Debug.WriteLineIf(p.Direction == ParameterDirection.Input,
				string.Format("'{0}.{1}' is an input parameter.",
					db.Command.CommandText, paramName));

			return p;
		}

		[NoInterception]
		protected virtual IDbDataParameter[] CreateParameters(
			DbManager                 db,
			object                    obj,
			string[]                  outputParameters,
			string[]                  inputOutputParameters,
			string[]                  ignoreParameters,
			params IDbDataParameter[] commandParameters)
		{
			return db.CreateParameters(obj, outputParameters,
				inputOutputParameters, ignoreParameters, commandParameters);
		}

		[NoInterception]
		protected virtual IDbDataParameter[] CreateParameters(
			DbManager                 db,
			DataRow                   dataRow,
			string[]                  outputParameters,
			string[]                  inputOutputParameters,
			string[]                  ignoreParameters,
			params IDbDataParameter[] commandParameters)
		{
			return db.CreateParameters(dataRow, outputParameters,
				inputOutputParameters, ignoreParameters, commandParameters);
		}

		[NoInterception]
		protected virtual string PrepareSqlQuery(DbManager db, int queryID, int uniqueID, string sqlQuery)
		{
			return sqlQuery;
		}

		#endregion

		#region ExecuteDictionary

		protected void ExecuteDictionary(
			DbManager             db,
			IDictionary           dictionary,
			Type                  objectType,
			Type                  keyType,
			string                methodName)
		{
			var isIndex = TypeHelper.IsSameOrParent(typeof(CompoundValue), keyType);
			var mms     = new SqlQuery(Extensions).GetKeyFieldList(db, objectType);

			if (mms.Length == 0)
				throw new DataAccessException(string.Format(
					Resources.DataAccessor_UnknownIndex,
					GetType().Name, methodName));

			if (mms.Length > 1 && keyType != typeof(object) && !isIndex)
				throw new DataAccessException(string.Format(
					Resources.DataAccessor_InvalidKeyType,
					GetType().Name, methodName));

			if (isIndex || mms.Length > 1)
			{
				var fields = new string[mms.Length];

				for (var i = 0; i < mms.Length; i++)
					fields[i] = mms[i].MemberName;

				db.ExecuteDictionary(dictionary, new MapIndex(fields), objectType, null);
			}
			else
			{
				db.ExecuteDictionary(dictionary, mms[0].MemberName, objectType, null);
			}
		}

		protected void ExecuteDictionary<TValue>(
			DbManager                          db,
			IDictionary<CompoundValue, TValue> dictionary,
			Type                               objectType,
			string                             methodName)
		{
			var mms = new SqlQuery(Extensions).GetKeyFieldList(db, objectType);

			if (mms.Length == 0)
				throw new DataAccessException(string.Format(
					Resources.DataAccessor_UnknownIndex,
					GetType().Name, methodName));

			var fields = new string[mms.Length];

			for (var i = 0; i < mms.Length; i++)
				fields[i] = mms[i].MemberName;

			db.ExecuteDictionary(dictionary, new MapIndex(fields), objectType, null);
		}

		protected void ExecuteDictionary<TKey, TValue>(
			DbManager                 db,
			IDictionary<TKey, TValue> dictionary,
			Type                      objectType,
			string                    methodName)
		{
			var mms = new SqlQuery(Extensions).GetKeyFieldList(db, objectType);

			if (mms.Length == 0)
				throw new DataAccessException(string.Format(
					Resources.DataAccessor_UnknownIndex,
					GetType().Name, methodName));

			if (mms.Length != 1)
				throw new DataAccessException(string.Format(
					Resources.DataAccessor_IndexIsComplex,
					GetType().Name, methodName));

			db.ExecuteDictionary(dictionary, mms[0].MemberName, objectType, null);
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
			var isIndex = TypeHelper.IsSameOrParent(typeof(CompoundValue), keyType);
			var mms     = new SqlQuery(Extensions).GetKeyFieldList(db, objectType);

			if (mms.Length == 0)
				throw new DataAccessException(string.Format(
					Resources.DataAccessor_UnknownIndex,
					GetType().Name, methodName));

			if (mms.Length > 1 && keyType != typeof(object) && !isIndex)
				throw new DataAccessException(string.Format(
					Resources.DataAccessor_InvalidKeyType,
					GetType().Name, methodName));

			if (isIndex || mms.Length > 1)
			{
				var fields = new string[mms.Length];

				for (var i = 0; i < mms.Length; i++)
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

		#region ExecuteEnumerable

		protected IEnumerable<T> ExecuteEnumerable<T>(DbManager db, Type objectType, bool disposeDbManager)
		{
			try
			{
				using (var rd = db.ExecuteReader())
				{
					if (rd.Read())
					{
						var dest   = MappingSchema.GetObjectMapper(objectType);
						var source = MappingSchema.CreateDataReaderMapper(rd);

						var ctx = new InitContext
						{
							MappingSchema = MappingSchema,
							ObjectMapper  = dest,
							DataSource    = source,
							SourceObject  = rd
						};

						var index   = MappingSchema.GetIndex(source, dest);
						var mappers = ctx.MappingSchema.GetValueMappers(source, dest, index);

						do
						{
							var destObject = (T)dest.CreateInstance(ctx);

							if (ctx.StopMapping)
								yield return destObject;

							var smDest = destObject as ISupportMapping;

							if (smDest != null)
							{
								smDest.BeginMapping(ctx);

								if (ctx.StopMapping)
									yield return destObject;
							}

							MappingSchema.MapInternal(source, rd, dest, destObject, index, mappers);

							if (smDest != null)
								smDest.EndMapping(ctx);

							yield return destObject;
						} while (rd.Read());
					}
				}
			}
			finally
			{
				if (disposeDbManager)
					db.Dispose();
			}
		}

		protected IEnumerable ExecuteEnumerable(DbManager db, Type objectType, bool disposeDbManager)
		{
			var ms = db.MappingSchema;

			if (disposeDbManager)
			{
				using (db)
				using (var rd = db.ExecuteReader())
					while (rd.Read())
						yield return ms.MapDataReaderToObject(rd, objectType);
			}
			else
			{
				using (var rd = db.ExecuteReader())
					while (rd.Read())
						yield return ms.MapDataReaderToObject(rd, objectType);
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
		protected virtual DateTimeOffset ConvertToDateTimeOffset(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToDateTimeOffset(value);
		}

		[NoInterception]
		protected virtual System.Data.Linq.Binary ConvertToLinqBinary(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToLinqBinary(value);
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

		[NoInterception]
		protected virtual XmlDocument ConvertToXmlDocument(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToXmlDocument(value);
		}

#if !SILVERLIGHT
		[NoInterception]
        protected virtual XElement ConvertToXElement(DbManager db, object value, object parameter)
        {
            return db.MappingSchema.ConvertToXElement(value);
        }
#endif

        [NoInterception]
		protected virtual Byte[] ConvertToByteArray(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToByteArray(value);
		}

		[NoInterception]
		protected virtual Char[] ConvertToCharArray(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToCharArray(value);
		}

		#endregion

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

		#region IsNull

		/// <summary>
		/// Reserved for internal BLToolkit use.
		/// </summary>
		public interface INullableInternal
		{
			bool IsNull { [MustImplement(false, false)] get; }
		}

		[NoInterception]
		protected virtual bool IsNull(
			DbManager db,
			object    value,
			object    parameter)
		{
			// Speed up for scalar and nullable types.
			//
			switch (System.Convert.GetTypeCode(value))
			{
				// null, DBNull.Value, Nullable<T> without a value.
				//
				case TypeCode.Empty:
				case TypeCode.DBNull:
					return true;

				case TypeCode.Object:
					break;

				// int, byte, string, DateTime and other primitives except Guid.
				// Also Nullable<T> with a value.
				//
				default:
					return false;
			}

			// Speed up for SqlTypes.
			//
			var nullable = value as INullable;
			if (nullable != null)
				return nullable.IsNull;

			// All other types which have 'IsNull' property but does not implement 'INullable' interface.
			// For example: 'Oracle.DataAccess.Types.OracleDecimal'.
			//
			// For types without 'IsNull' property the return value is always false.
			//
			var nullableInternal = (INullableInternal)DuckTyping.Implement(typeof(INullableInternal), value);

			return nullableInternal.IsNull;
		}

		#endregion

		protected virtual SqlQueryAttribute GetSqlQueryAttribute(MethodInfo methodInfo)
		{
			var attrs = methodInfo.GetCustomAttributes(typeof(SqlQueryAttribute), true);
			return (SqlQueryAttribute)attrs[0];
		}

		#endregion
	}
}
