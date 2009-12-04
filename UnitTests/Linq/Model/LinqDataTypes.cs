using System;

namespace Data.Linq.Model
{
	public class LinqDataTypes : IEquatable<LinqDataTypes>
	{
		public int      ID;
		public decimal  MoneyValue;
		public DateTime DateTimeValue;

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public bool Equals(LinqDataTypes other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.ID == ID && other.MoneyValue == MoneyValue && other.DateTimeValue.Equals(DateTimeValue);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = ID;
				result = (result * 397) ^ MoneyValue.GetHashCode();
				result = (result * 397) ^ DateTimeValue.GetHashCode();
				return result;
			}
		}

		public static bool operator ==(LinqDataTypes left, LinqDataTypes right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(LinqDataTypes left, LinqDataTypes right)
		{
			return !Equals(left, right);
		}
	}

	public class LinqDataTypes2
	{
		public int       ID;
		public decimal   MoneyValue;
		public DateTime? DateTimeValue;
	}
}
