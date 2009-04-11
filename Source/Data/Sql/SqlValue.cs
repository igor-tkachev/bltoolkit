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

		public override string ToString()
		{
			if (_value == null)
				return "NULL";

			if (_value is string)
				return "'" + _value.ToString().Replace("\'", "''");

			return _value.ToString();
		}

		#region ISqlExpressionScannable Members

		void ISqlExpressionScannable.ForEach(bool skipColumns, Action<ISqlExpression> action)
		{
			action(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if ((object)this == other)
				return true;

			SqlValue value = other as SqlValue;
			return _value == null && value._value == null || _value != null && _value.Equals(value._value);
		}

		#endregion
	}
}
