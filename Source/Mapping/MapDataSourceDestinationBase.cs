using System;

namespace BLToolkit.Mapping
{
	public abstract class MapDataSourceDestinationBase : IMapDataSource, IMapDataDestination
	{
		#region IMapDataSource Members

		public abstract int    Count { get; }
		public abstract Type   GetFieldType(int index);
		public abstract string GetName     (int index);
		public abstract object GetValue    (object o, int index);
		public abstract object GetValue    (object o, string name);

		public virtual bool    IsNull    (object o, int index) { return GetValue(o, index) == null; }

		[CLSCompliant(false)]
		public virtual SByte   GetSByte  (object o, int index) { return Map.DefaultSchema.ConvertToSByte  (GetValue(o, index)); }
		public virtual Int16   GetInt16  (object o, int index) { return Map.DefaultSchema.ConvertToInt16  (GetValue(o, index)); }
		public virtual Int32   GetInt32  (object o, int index) { return Map.DefaultSchema.ConvertToInt32  (GetValue(o, index)); }
		public virtual Int64   GetInt64  (object o, int index) { return Map.DefaultSchema.ConvertToInt64  (GetValue(o, index)); }

		public virtual Byte    GetByte   (object o, int index) { return Map.DefaultSchema.ConvertToByte   (GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt16  GetUInt16 (object o, int index) { return Map.DefaultSchema.ConvertToUInt16 (GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt32  GetUInt32 (object o, int index) { return Map.DefaultSchema.ConvertToUInt32 (GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt64  GetUInt64 (object o, int index) { return Map.DefaultSchema.ConvertToUInt64 (GetValue(o, index)); }

		public virtual Boolean GetBoolean(object o, int index) { return Map.DefaultSchema.ConvertToBoolean(GetValue(o, index)); }
		public virtual Char    GetChar   (object o, int index) { return Map.DefaultSchema.ConvertToChar   (GetValue(o, index)); }
		public virtual Single  GetSingle (object o, int index) { return Map.DefaultSchema.ConvertToSingle (GetValue(o, index)); }
		public virtual Double  GetDouble (object o, int index) { return Map.DefaultSchema.ConvertToDouble (GetValue(o, index)); }
		public virtual Decimal GetDecimal(object o, int index) { return Map.DefaultSchema.ConvertToDecimal(GetValue(o, index)); }
		public virtual Guid    GetGuid   (object o, int index) { return Map.DefaultSchema.ConvertToGuid   (GetValue(o, index)); }

		#endregion

		#region IMapDataDestination Members

		public abstract int  GetOrdinal(string name);
		public abstract void SetValue  (object o, int index, object value);
		public abstract void SetValue  (object o, string name, object value);

		public virtual void SetNull   (object o, int index)                { SetValue(o, index, null); }

		[CLSCompliant(false)]
		public virtual void SetSByte  (object o, int index, SByte   value) { SetValue(o, index, value); }
		public virtual void SetInt16  (object o, int index, Int16   value) { SetValue(o, index, value); }
		public virtual void SetInt32  (object o, int index, Int32   value) { SetValue(o, index, value); }
		public virtual void SetInt64  (object o, int index, Int64   value) { SetValue(o, index, value); }

		public virtual void SetByte   (object o, int index, Byte    value) { SetValue(o, index, value); }
		[CLSCompliant(false)]
		public virtual void SetUInt16 (object o, int index, UInt16  value) { SetValue(o, index, value); }
		[CLSCompliant(false)]
		public virtual void SetUInt32 (object o, int index, UInt32  value) { SetValue(o, index, value); }
		[CLSCompliant(false)]
		public virtual void SetUInt64 (object o, int index, UInt64  value) { SetValue(o, index, value); }

		public virtual void SetBoolean(object o, int index, Boolean value) { SetValue(o, index, value); }
		public virtual void SetChar   (object o, int index, Char    value) { SetValue(o, index, value); }
		public virtual void SetSingle (object o, int index, Single  value) { SetValue(o, index, value); }
		public virtual void SetDouble (object o, int index, Double  value) { SetValue(o, index, value); }
		public virtual void SetDecimal(object o, int index, Decimal value) { SetValue(o, index, value); }
		public virtual void SetGuid   (object o, int index, Guid    value) { SetValue(o, index, value); }

		#endregion
	}
}
