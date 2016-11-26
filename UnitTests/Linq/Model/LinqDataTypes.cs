using System;
using System.Data;
using System.Data.Linq;

using BLToolkit.DataAccess;

namespace Data.Linq.Model
{
	public class LinqDataTypes : IEquatable<LinqDataTypes>, IComparable
	{
		public int      ID;
		public decimal  MoneyValue;
		public DateTime DateTimeValue;
		public bool     BoolValue;
		public Guid     GuidValue;
		public Binary   BinaryValue;
		public short    SmallIntValue;
		public UInt16   UInt16;
		public UInt32   UInt32;
		public UInt64   UInt64;

		public override bool Equals(object obj)
		{
			return Equals(obj as LinqDataTypes);
		}

		public bool Equals(LinqDataTypes other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return
				other.ID                   == ID            &&
				other.MoneyValue           == MoneyValue    &&
				other.BoolValue            == BoolValue     &&
				other.GuidValue            == GuidValue     &&
				other.SmallIntValue        == SmallIntValue &&
				other.DateTimeValue.Date   == DateTimeValue.Date &&
				other.DateTimeValue.Hour   == DateTimeValue.Hour &&
				other.DateTimeValue.Minute == DateTimeValue.Minute &&
				other.DateTimeValue.Second == DateTimeValue.Second &&
				other.UInt16               == UInt16 &&
				other.UInt32               == UInt32 &&
				other.UInt64               == UInt64;
		}

		public override int GetHashCode()
		{
			return ID;
		}

		public int CompareTo(object obj)
		{
			return ID - ((LinqDataTypes)obj).ID;
		}

		public static bool operator == (LinqDataTypes left, LinqDataTypes right)
		{
			return Equals(left, right);
		}

		public static bool operator != (LinqDataTypes left, LinqDataTypes right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			return string.Format("{{{0,2}, {1,7}, {2}, {3,5}, {4}, {5}, {6}, {7}, {8}}}", ID, MoneyValue, DateTimeValue, BoolValue, GuidValue, SmallIntValue, UInt16, UInt32, UInt64);
		}
	}

	[TableName("LinqDataTypes")]
	public class LinqDataTypes2 : IEquatable<LinqDataTypes2>, IComparable
	{
		[PrimaryKey]
		public int       ID;
		public decimal   MoneyValue;
		public DateTime? DateTimeValue;
		[DbType(DbType.DateTime2)]
		public DateTime? DateTimeValue2;
		public bool?     BoolValue;
		public Guid?     GuidValue;
		public short?    SmallIntValue;
		public int?      IntValue;
		public long?     BigIntValue;

		public override bool Equals(object obj)
		{
			return Equals(obj as LinqDataTypes2);
		}

		public bool Equals(LinqDataTypes2 other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return
				other.ID                         == ID            &&
				other.MoneyValue                 == MoneyValue    &&
				other.BoolValue                  == BoolValue     &&
				other.GuidValue                  == GuidValue     &&
				other.DateTimeValue.Value.Date   == DateTimeValue.Value.Date &&
				other.DateTimeValue.Value.Hour   == DateTimeValue.Value.Hour &&
				other.DateTimeValue.Value.Minute == DateTimeValue.Value.Minute &&
				other.DateTimeValue.Value.Second == DateTimeValue.Value.Second;
		}

		public override int GetHashCode()
		{
			return ID;
		}

		public int CompareTo(object obj)
		{
			return ID - ((LinqDataTypes2)obj).ID;
		}

		public static bool operator ==(LinqDataTypes2 left, LinqDataTypes2 right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(LinqDataTypes2 left, LinqDataTypes2 right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			return string.Format("{{{0,2}, {1,7}, {2}, {3,5}, {4}}}", ID, MoneyValue, DateTimeValue, BoolValue, GuidValue);
		}
	}
}
