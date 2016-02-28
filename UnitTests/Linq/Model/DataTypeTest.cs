using System;
using System.Data.Linq;
using System.Xml.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Data.Linq.Model
{
	public class DataTypeTest
	{
		[Identity, PrimaryKey]
		public int       DataTypeID;
		public Binary    Binary_;
		public bool?     Boolean_;
		public byte?     Byte_;
		public Binary    Bytes_;
		public char?     Char_;
		public DateTime? DateTime_;
		public decimal?  Decimal_;
		public double?   Double_;
		public Guid?     Guid_;
		public short?    Int16_;
		public int?      Int32_;
		public long?     Int64_;
		public decimal?  Money_;
		public byte?     SByte_;
		public float?    Single_;
		public Binary    Stream_;
		public string    String_;
		public short?    UInt16_;
		public int?      UInt32_;
		public long?     UInt64_;
		public XElement  Xml_;
	}

	[TableName("DataTypeTest")]
	public class DataTypeTest2
	{
		[Identity, PrimaryKey]
		public int DataTypeID;
		public Binary Binary_;
		public bool? Boolean_;
		[Nullable] public byte Byte_;
		public Binary Bytes_;
		public char? Char_;
		public DateTime? DateTime_;
		[Nullable]public decimal Decimal_;
		[Nullable]public double Double_;
		public Guid? Guid_;
		[Nullable]public short Int16_;
		[Nullable]public int Int32_;
		[Nullable]public long Int64_;
		[Nullable]public decimal Money_;
		[Nullable]public byte SByte_;
		[Nullable]public float Single_;
		public Binary Stream_;
		public string String_;
		[Nullable]public short UInt16_;
		[Nullable]public int UInt32_;
		[Nullable]public long UInt64_;
		public XElement Xml_;
	}


	[TableName("DataTypeTest")]
	public class DataTypeTest3
	{
		[Identity, PrimaryKey]
		public int DataTypeID;
		public Binary Binary_;
		public bool? Boolean_;
		public byte? Byte_;
		public Binary Bytes_;
		public char? Char_;
		public DateTime? DateTime_;
		public decimal? Decimal_;
		public double? Double_;
		public Guid? Guid_;
		public short? Int16_;
		public int? Int32_;
		public long? Int64_;
		public decimal? Money_;
		public byte? SByte_;
		public float? Single_;
		public Binary Stream_;
		public string String_;
		public short? UInt16_;
		public int? UInt32_;
		public long? UInt64_;
		public string Xml_;
	}
}
