using System;
using System.Data.SqlTypes;

namespace BLToolkit.Mapping
{
	public abstract class MapDataSourceDestinationBase : IMapDataSource, IMapDataDestination
	{
		#region IMapDataSource Members

		public abstract int    Count { get; }
		public abstract Type   GetFieldType (int index);
		public abstract string GetName      (int index);
		public abstract object GetValue     (object o, int index);
		public abstract object GetValue     (object o, string name);

		public virtual bool    IsNull       (object o, int index) { return GetValue(o, index) == null; }
		public virtual bool    SupportsValue(int index)           { return true; }

		// Simple type getters.
		//
		[CLSCompliant(false)]
		public virtual SByte    GetSByte   (object o, int index) { return Map.DefaultSchema.ConvertToSByte   (GetValue(o, index)); }
		public virtual Int16    GetInt16   (object o, int index) { return Map.DefaultSchema.ConvertToInt16   (GetValue(o, index)); }
		public virtual Int32    GetInt32   (object o, int index) { return Map.DefaultSchema.ConvertToInt32   (GetValue(o, index)); }
		public virtual Int64    GetInt64   (object o, int index) { return Map.DefaultSchema.ConvertToInt64   (GetValue(o, index)); }

		public virtual Byte     GetByte    (object o, int index) { return Map.DefaultSchema.ConvertToByte    (GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt16   GetUInt16  (object o, int index) { return Map.DefaultSchema.ConvertToUInt16  (GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt32   GetUInt32  (object o, int index) { return Map.DefaultSchema.ConvertToUInt32  (GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt64   GetUInt64  (object o, int index) { return Map.DefaultSchema.ConvertToUInt64  (GetValue(o, index)); }

		public virtual Boolean  GetBoolean (object o, int index) { return Map.DefaultSchema.ConvertToBoolean (GetValue(o, index)); }
		public virtual Char     GetChar    (object o, int index) { return Map.DefaultSchema.ConvertToChar    (GetValue(o, index)); }
		public virtual Single   GetSingle  (object o, int index) { return Map.DefaultSchema.ConvertToSingle  (GetValue(o, index)); }
		public virtual Double   GetDouble  (object o, int index) { return Map.DefaultSchema.ConvertToDouble  (GetValue(o, index)); }
		public virtual Decimal  GetDecimal (object o, int index) { return Map.DefaultSchema.ConvertToDecimal (GetValue(o, index)); }
		public virtual Guid     GetGuid    (object o, int index) { return Map.DefaultSchema.ConvertToGuid    (GetValue(o, index)); }
		public virtual DateTime GetDateTime(object o, int index) { return Map.DefaultSchema.ConvertToDateTime(GetValue(o, index)); }

#if FW2
		// Nullable type getters.
		//
		[CLSCompliant(false)]
		public virtual SByte?    GetNullableSByte   (object o, int index) { return Map.DefaultSchema.ConvertToNullableSByte   (GetValue(o, index)); }
		public virtual Int16?    GetNullableInt16   (object o, int index) { return Map.DefaultSchema.ConvertToNullableInt16   (GetValue(o, index)); }
		public virtual Int32?    GetNullableInt32   (object o, int index) { return Map.DefaultSchema.ConvertToNullableInt32   (GetValue(o, index)); }
		public virtual Int64?    GetNullableInt64   (object o, int index) { return Map.DefaultSchema.ConvertToNullableInt64   (GetValue(o, index)); }

		public virtual Byte?     GetNullableByte    (object o, int index) { return Map.DefaultSchema.ConvertToNullableByte    (GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt16?   GetNullableUInt16  (object o, int index) { return Map.DefaultSchema.ConvertToNullableUInt16  (GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt32?   GetNullableUInt32  (object o, int index) { return Map.DefaultSchema.ConvertToNullableUInt32  (GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt64?   GetNullableUInt64  (object o, int index) { return Map.DefaultSchema.ConvertToNullableUInt64  (GetValue(o, index)); }

		public virtual Boolean?  GetNullableBoolean (object o, int index) { return Map.DefaultSchema.ConvertToNullableBoolean (GetValue(o, index)); }
		public virtual Char?     GetNullableChar    (object o, int index) { return Map.DefaultSchema.ConvertToNullableChar    (GetValue(o, index)); }
		public virtual Single?   GetNullableSingle  (object o, int index) { return Map.DefaultSchema.ConvertToNullableSingle  (GetValue(o, index)); }
		public virtual Double?   GetNullableDouble  (object o, int index) { return Map.DefaultSchema.ConvertToNullableDouble  (GetValue(o, index)); }
		public virtual Decimal?  GetNullableDecimal (object o, int index) { return Map.DefaultSchema.ConvertToNullableDecimal (GetValue(o, index)); }
		public virtual Guid?     GetNullableGuid    (object o, int index) { return Map.DefaultSchema.ConvertToNullableGuid    (GetValue(o, index)); }
		public virtual DateTime? GetNullableDateTime(object o, int index) { return Map.DefaultSchema.ConvertToNullableDateTime(GetValue(o, index)); }
#endif

		// SQL type getters.
		//
		public virtual SqlByte     GetSqlByte     (object o, int index) { return Map.DefaultSchema.ConvertToSqlByte    (GetValue(o, index)); }
		public virtual SqlInt16    GetSqlInt16    (object o, int index) { return Map.DefaultSchema.ConvertToSqlInt16   (GetValue(o, index)); }
		public virtual SqlInt32    GetSqlInt32    (object o, int index) { return Map.DefaultSchema.ConvertToSqlInt32   (GetValue(o, index)); }
		public virtual SqlInt64    GetSqlInt64    (object o, int index) { return Map.DefaultSchema.ConvertToSqlInt64   (GetValue(o, index)); }
		public virtual SqlSingle   GetSqlSingle   (object o, int index) { return Map.DefaultSchema.ConvertToSqlSingle  (GetValue(o, index)); }
		public virtual SqlBoolean  GetSqlBoolean  (object o, int index) { return Map.DefaultSchema.ConvertToSqlBoolean (GetValue(o, index)); }
		public virtual SqlDouble   GetSqlDouble   (object o, int index) { return Map.DefaultSchema.ConvertToSqlDouble  (GetValue(o, index)); }
		public virtual SqlDateTime GetSqlDateTime (object o, int index) { return Map.DefaultSchema.ConvertToSqlDateTime(GetValue(o, index)); }
		public virtual SqlDecimal  GetSqlDecimal  (object o, int index) { return Map.DefaultSchema.ConvertToSqlDecimal (GetValue(o, index)); }
		public virtual SqlMoney    GetSqlMoney    (object o, int index) { return Map.DefaultSchema.ConvertToSqlMoney   (GetValue(o, index)); }
		public virtual SqlGuid     GetSqlGuid     (object o, int index) { return Map.DefaultSchema.ConvertToSqlGuid    (GetValue(o, index)); }
		public virtual SqlString   GetSqlString   (object o, int index) { return Map.DefaultSchema.ConvertToSqlString  (GetValue(o, index)); }

		#endregion

		#region IMapDataDestination Members

		public abstract int  GetOrdinal(string name);
		public abstract void SetValue  (object o, int index, object value);
		public abstract void SetValue  (object o, string name, object value);

		public virtual  void SetNull   (object o, int index)                { SetValue(o, index, null); }

		// Simple types setters.
		//
		[CLSCompliant(false)]
		public virtual void SetSByte   (object o, int index, SByte    value) { SetValue(o, index, value); }
		public virtual void SetInt16   (object o, int index, Int16    value) { SetValue(o, index, value); }
		public virtual void SetInt32   (object o, int index, Int32    value) { SetValue(o, index, value); }
		public virtual void SetInt64   (object o, int index, Int64    value) { SetValue(o, index, value); }

		public virtual void SetByte    (object o, int index, Byte     value) { SetValue(o, index, value); }
		[CLSCompliant(false)]
		public virtual void SetUInt16  (object o, int index, UInt16   value) { SetValue(o, index, value); }
		[CLSCompliant(false)]
		public virtual void SetUInt32  (object o, int index, UInt32   value) { SetValue(o, index, value); }
		[CLSCompliant(false)]
		public virtual void SetUInt64  (object o, int index, UInt64   value) { SetValue(o, index, value); }

		public virtual void SetBoolean (object o, int index, Boolean  value) { SetValue(o, index, value); }
		public virtual void SetChar    (object o, int index, Char     value) { SetValue(o, index, value); }
		public virtual void SetSingle  (object o, int index, Single   value) { SetValue(o, index, value); }
		public virtual void SetDouble  (object o, int index, Double   value) { SetValue(o, index, value); }
		public virtual void SetDecimal (object o, int index, Decimal  value) { SetValue(o, index, value); }
		public virtual void SetGuid    (object o, int index, Guid     value) { SetValue(o, index, value); }
		public virtual void SetDateTime(object o, int index, DateTime value) { SetValue(o, index, value); }

#if FW2
		// Nullable types setters.
		//
		[CLSCompliant(false)]
		public virtual void SetNullableSByte   (object o, int index, SByte?    value) { SetValue(o, index, value); }
		public virtual void SetNullableInt16   (object o, int index, Int16?    value) { SetValue(o, index, value); }
		public virtual void SetNullableInt32   (object o, int index, Int32?    value) { SetValue(o, index, value); }
		public virtual void SetNullableInt64   (object o, int index, Int64?    value) { SetValue(o, index, value); }

		public virtual void SetNullableByte    (object o, int index, Byte?     value) { SetValue(o, index, value); }
		[CLSCompliant(false)]
		public virtual void SetNullableUInt16  (object o, int index, UInt16?   value) { SetValue(o, index, value); }
		[CLSCompliant(false)]
		public virtual void SetNullableUInt32  (object o, int index, UInt32?   value) { SetValue(o, index, value); }
		[CLSCompliant(false)]
		public virtual void SetNullableUInt64  (object o, int index, UInt64?   value) { SetValue(o, index, value); }

		public virtual void SetNullableBoolean (object o, int index, Boolean?  value) { SetValue(o, index, value); }
		public virtual void SetNullableChar    (object o, int index, Char?     value) { SetValue(o, index, value); }
		public virtual void SetNullableSingle  (object o, int index, Single?   value) { SetValue(o, index, value); }
		public virtual void SetNullableDouble  (object o, int index, Double?   value) { SetValue(o, index, value); }
		public virtual void SetNullableDecimal (object o, int index, Decimal?  value) { SetValue(o, index, value); }
		public virtual void SetNullableGuid    (object o, int index, Guid?     value) { SetValue(o, index, value); }
		public virtual void SetNullableDateTime(object o, int index, DateTime? value) { SetValue(o, index, value); }
#endif

		// SQL type setters.
		//
		public virtual void SetSqlByte    (object o, int index, SqlByte     value) { SetValue(o, index, value); }
		public virtual void SetSqlInt16   (object o, int index, SqlInt16    value) { SetValue(o, index, value); }
		public virtual void SetSqlInt32   (object o, int index, SqlInt32    value) { SetValue(o, index, value); }
		public virtual void SetSqlInt64   (object o, int index, SqlInt64    value) { SetValue(o, index, value); }
		public virtual void SetSqlSingle  (object o, int index, SqlSingle   value) { SetValue(o, index, value); }
		public virtual void SetSqlBoolean (object o, int index, SqlBoolean  value) { SetValue(o, index, value); }
		public virtual void SetSqlDouble  (object o, int index, SqlDouble   value) { SetValue(o, index, value); }
		public virtual void SetSqlDateTime(object o, int index, SqlDateTime value) { SetValue(o, index, value); }
		public virtual void SetSqlDecimal (object o, int index, SqlDecimal  value) { SetValue(o, index, value); }
		public virtual void SetSqlMoney   (object o, int index, SqlMoney    value) { SetValue(o, index, value); }
		public virtual void SetSqlGuid    (object o, int index, SqlGuid     value) { SetValue(o, index, value); }
		public virtual void SetSqlString  (object o, int index, SqlString   value) { SetValue(o, index, value); }

		#endregion
	}
}
