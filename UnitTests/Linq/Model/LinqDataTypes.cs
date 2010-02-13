using System;
using System.Data.Linq;

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
				other.DateTimeValue.Date   == DateTimeValue.Date &&
				other.DateTimeValue.Hour   == DateTimeValue.Hour &&
				other.DateTimeValue.Minute == DateTimeValue.Minute &&
				other.DateTimeValue.Second == DateTimeValue.Second;
		}

		public override int GetHashCode()
		{
			return ID;
		}

		public int CompareTo(object obj)
		{
			return ID - ((LinqDataTypes)obj).ID;
		}

		public static bool operator ==(LinqDataTypes left, LinqDataTypes right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(LinqDataTypes left, LinqDataTypes right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			return string.Format("{{{0,2}, {1,7}, {2}, {3,5}, {4}}}", ID, MoneyValue, DateTimeValue, BoolValue, GuidValue);
		}
	}

	public class LinqDataTypes2
	{
		public int       ID;
		public decimal   MoneyValue;
		public DateTime? DateTimeValue;
		public bool?     BoolValue;
		public Guid?     GuidValue;
	}
}
