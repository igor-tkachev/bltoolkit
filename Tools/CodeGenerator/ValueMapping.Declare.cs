using System;
using System.Data.SqlTypes;

using KeyValue = System.Collections.Generic.KeyValuePair<System.Type, System.Type>;
using Table    = System.Collections.Generic.Dictionary<System.Collections.Generic.KeyValuePair<System.Type, System.Type>, BLToolkit.Mapping.IValueMapper>;

namespace BLToolkit.Mapping
{
	[CLSCompliant(false)]
	public interface IMapDataSource
	{
		int      Count { get; }

		Type     GetFieldType (int index);
		string   GetName      (int index);
		int      GetOrdinal   (string name);
		object   GetValue     (object o, int index);
		object   GetValue     (object o, string name);

		bool     IsNull       (object o, int index);

		bool     SupportsTypedValues(int index);

		// Simple type getters.
		//
		[CLSCompliant(false)]
		SByte    GetSByte     (object o, int index);
		Int16    GetInt16     (object o, int index);
		Int32    GetInt32     (object o, int index);
		Int64    GetInt64     (object o, int index);

		Byte     GetByte      (object o, int index);
		[CLSCompliant(false)]
		UInt16   GetUInt16    (object o, int index);
		[CLSCompliant(false)]
		UInt32   GetUInt32    (object o, int index);
		[CLSCompliant(false)]
		UInt64   GetUInt64    (object o, int index);

		Boolean  GetBoolean   (object o, int index);
		Char     GetChar      (object o, int index);
		Single   GetSingle    (object o, int index);
		Double   GetDouble    (object o, int index);
		Decimal  GetDecimal   (object o, int index);
		DateTime GetDateTime  (object o, int index);
		Guid     GetGuid      (object o, int index);

#if FW2
		// Simple type getters.
		//
		[CLSCompliant(false)]
		SByte?    GetNullableSByte   (object o, int index);
		Int16?    GetNullableInt16   (object o, int index);
		Int32?    GetNullableInt32   (object o, int index);
		Int64?    GetNullableInt64   (object o, int index);

		Byte?     GetNullableByte    (object o, int index);
		[CLSCompliant(false)]
		UInt16?   GetNullableUInt16  (object o, int index);
		[CLSCompliant(false)]
		UInt32?   GetNullableUInt32  (object o, int index);
		[CLSCompliant(false)]
		UInt64?   GetNullableUInt64  (object o, int index);

		Boolean?  GetNullableBoolean (object o, int index);
		Char?     GetNullableChar    (object o, int index);
		Single?   GetNullableSingle  (object o, int index);
		Double?   GetNullableDouble  (object o, int index);
		Decimal?  GetNullableDecimal (object o, int index);
		DateTime? GetNullableDateTime(object o, int index);
		Guid?     GetNullableGuid    (object o, int index);
#endif

		// SQL type getters.
		//
		SqlByte     GetSqlByte     (object o, int index);
		SqlInt16    GetSqlInt16    (object o, int index);
		SqlInt32    GetSqlInt32    (object o, int index);
		SqlInt64    GetSqlInt64    (object o, int index);
		SqlSingle   GetSqlSingle   (object o, int index);
		SqlBoolean  GetSqlBoolean  (object o, int index);
		SqlDouble   GetSqlDouble   (object o, int index);
		SqlDateTime GetSqlDateTime (object o, int index);
		SqlDecimal  GetSqlDecimal  (object o, int index);
		SqlMoney    GetSqlMoney    (object o, int index);
		SqlGuid     GetSqlGuid     (object o, int index);
		SqlString   GetSqlString   (object o, int index);
	}

	[CLSCompliant(false)]
	public interface IMapDataDestination
	{
		Type GetFieldType (int index);
		int  GetOrdinal   (string name);
		void SetValue     (object o, int index,   object value);
		void SetValue     (object o, string name, object value);

		void SetNull      (object o, int index);

		bool SupportsTypedValues(int index);

		// Simple type setters.
		//
		[CLSCompliant(false)]
		void SetSByte     (object o, int index, SByte    value);
		void SetInt16     (object o, int index, Int16    value);
		void SetInt32     (object o, int index, Int32    value);
		void SetInt64     (object o, int index, Int64    value);

		void SetByte      (object o, int index, Byte     value);
		[CLSCompliant(false)]
		void SetUInt16    (object o, int index, UInt16   value);
		[CLSCompliant(false)]
		void SetUInt32    (object o, int index, UInt32   value);
		[CLSCompliant(false)]
		void SetUInt64    (object o, int index, UInt64   value);

		void SetBoolean   (object o, int index, Boolean  value);
		void SetChar      (object o, int index, Char     value);
		void SetSingle    (object o, int index, Single   value);
		void SetDouble    (object o, int index, Double   value);
		void SetDecimal   (object o, int index, Decimal  value);
		void SetGuid      (object o, int index, Guid     value);
		void SetDateTime  (object o, int index, DateTime value);

#if FW2
		// Simple type setters.
		//
		[CLSCompliant(false)]
		void SetNullableSByte     (object o, int index, SByte?    value);
		void SetNullableInt16     (object o, int index, Int16?    value);
		void SetNullableInt32     (object o, int index, Int32?    value);
		void SetNullableInt64     (object o, int index, Int64?    value);

		void SetNullableByte      (object o, int index, Byte?     value);
		[CLSCompliant(false)]
		void SetNullableUInt16    (object o, int index, UInt16?   value);
		[CLSCompliant(false)]
		void SetNullableUInt32    (object o, int index, UInt32?   value);
		[CLSCompliant(false)]
		void SetNullableUInt64    (object o, int index, UInt64?   value);

		void SetNullableBoolean   (object o, int index, Boolean?  value);
		void SetNullableChar      (object o, int index, Char?     value);
		void SetNullableSingle    (object o, int index, Single?   value);
		void SetNullableDouble    (object o, int index, Double?   value);
		void SetNullableDecimal   (object o, int index, Decimal?  value);
		void SetNullableGuid      (object o, int index, Guid?     value);
		void SetNullableDateTime  (object o, int index, DateTime? value);
#endif

		// SQL type setters.
		//
		void SetSqlByte    (object o, int index, SqlByte     value);
		void SetSqlInt16   (object o, int index, SqlInt16    value);
		void SetSqlInt32   (object o, int index, SqlInt32    value);
		void SetSqlInt64   (object o, int index, SqlInt64    value);
		void SetSqlSingle  (object o, int index, SqlSingle   value);
		void SetSqlBoolean (object o, int index, SqlBoolean  value);
		void SetSqlDouble  (object o, int index, SqlDouble   value);
		void SetSqlDateTime(object o, int index, SqlDateTime value);
		void SetSqlDecimal (object o, int index, SqlDecimal  value);
		void SetSqlMoney   (object o, int index, SqlMoney    value);
		void SetSqlGuid    (object o, int index, SqlGuid     value);
		void SetSqlString  (object o, int index, SqlString   value);
	}

	[CLSCompliant(false)]
	public interface IValueMapper
	{
		void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex);
	}

	public static partial class ValueMapping
	{
		#region Init

		private static Table _mappers = new Table();

		private static void AddSameType(Type type, IValueMapper mapper)
		{
			_mappers.Add(new KeyValue(type, type), mapper);
		}

		#endregion

		#region Default Mapper

		class DefaultValueMapper : IValueMapper
		{
			public void Map(
				IMapDataSource source, object sourceObject, int sourceIndex,
				IMapDataDestination dest, object destObject, int destIndex)
			{
				dest.SetValue(destObject, destIndex, source.GetValue(sourceObject, sourceIndex));

				//object o = source.GetValue(sourceObject, sourceIndex);

				//if (o == null) dest.SetNull (destObject, destIndex);
				//else           dest.SetValue(destObject, destIndex, o);
			}
		}

		private static IValueMapper _defaultMapper = new DefaultValueMapper();
		[CLSCompliant(false)]
		public static IValueMapper DefaultMapper
		{
			get { return _defaultMapper; }
			set { _defaultMapper = value; }
		}

		#endregion

		#region GetMapper

		private static object _sync = new object();

		[CLSCompliant(false)]
		public static IValueMapper GetMapper(Type t1, Type t2)
		{
			lock (_sync)
			{
				if (t1 == null) t1 = typeof(object);
				if (t2 == null) t2 = typeof(object);

				if (t1.IsEnum) t1 = Enum.GetUnderlyingType(t1);
				if (t2.IsEnum) t2 = Enum.GetUnderlyingType(t2);

				KeyValue key = new KeyValue(t1, t2);

				IValueMapper t;

#if FW2
				if (_mappers.TryGetValue(key, out t))
					return t;

#else
				t = (IValueMapper)_mappers[key];

				if (t != null)
					return t;

				t = _defaultMapper;
#endif
				_mappers.Add(key, t);

				return t;
			}
		}

		#endregion
	}
}
