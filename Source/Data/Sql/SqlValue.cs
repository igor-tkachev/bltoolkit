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

		#region ISqlExpressionScannable Members

		void ISqlExpressionScannable.ForEach(Action<ISqlExpression> action)
		{
			action(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			SqlValue value = other as SqlValue;
			return _value == null && value._value == null || _value != null && _value.Equals(value._value);
		}

		#endregion
	}
}
