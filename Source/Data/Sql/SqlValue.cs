using System;

namespace BLToolkit.Data.Sql
{
	public class SqlValue : ISqlExpression
	{
		public SqlValue(object value)
		{
			_value = value;
		}

		readonly object _value;
		public   object  Value { get { return _value; } }

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			SqlValue value = other as SqlValue;
			return _value == null && value._value == null || _value.Equals(value._value);
		}
	}
}
