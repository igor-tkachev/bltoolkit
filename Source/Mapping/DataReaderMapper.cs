using System;
using System.Data;

namespace BLToolkit.Mapping
{
	public class DataReaderMapper : IMapDataSource
	{
		public DataReaderMapper(IDataReader dataReader)
		{
			_dataReader = dataReader;
		}

		private IDataReader _dataReader;
		public  IDataReader  DataReader
		{
			get { return _dataReader; }
		}

		#region IMapDataSource Members

		public virtual int Count
		{
			get { return _dataReader.FieldCount; }
		}

		public virtual Type GetFieldType(int index)
		{
			return _dataReader.GetFieldType(index);
		}

		public virtual string GetName(int index)
		{
			return _dataReader.GetName(index);
		}

		public virtual object GetValue(object o, int index)
		{
			object value = _dataReader.GetValue(index);
			return value is DBNull? null: value;
		}

		public virtual object GetValue(object o, string name)
		{
			object value = _dataReader[name];
			return value is DBNull? null: value;
		}

		public virtual bool    IsNull       (object o, int index) { return _dataReader.IsDBNull(index);   }
		public virtual bool    SupportsValue(int index)           { return true; }

		[CLSCompliant(false)]
		public virtual SByte   GetSByte  (object o, int index) { return Map.DefaultSchema.ConvertToSByte(GetValue(o, index)); }
		public virtual Int16   GetInt16  (object o, int index) { return _dataReader.GetInt16  (index); }
		public virtual Int32   GetInt32  (object o, int index) { return _dataReader.GetInt32  (index); }
		public virtual Int64   GetInt64  (object o, int index) { return _dataReader.GetInt64  (index); }

		public virtual Byte    GetByte   (object o, int index) { return _dataReader.GetByte   (index); }
		[CLSCompliant(false)]
		public virtual UInt16  GetUInt16 (object o, int index) { return Map.DefaultSchema.ConvertToUInt16(GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt32  GetUInt32 (object o, int index) { return Map.DefaultSchema.ConvertToUInt32(GetValue(o, index)); }
		[CLSCompliant(false)]
		public virtual UInt64  GetUInt64 (object o, int index) { return Map.DefaultSchema.ConvertToUInt64(GetValue(o, index)); }

		public virtual Boolean GetBoolean(object o, int index) { return _dataReader.GetBoolean(index); }
		public virtual Char    GetChar   (object o, int index) { return _dataReader.GetChar   (index); }
		public virtual Single  GetSingle (object o, int index) { return Map.DefaultSchema.ConvertToSingle(GetValue(o, index)); }
		public virtual Double  GetDouble (object o, int index) { return _dataReader.GetDouble (index); }
		public virtual Decimal GetDecimal(object o, int index) { return _dataReader.GetDecimal(index); }
		public virtual Guid    GetGuid   (object o, int index) { return _dataReader.GetGuid   (index); }

		#endregion
	}
}
