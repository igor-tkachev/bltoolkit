using System;

namespace BLToolkit.Data.SqlBuilder
{
	public class SqlValue : ISqlExpression
	{
		public SqlValue(object value)
		{
			_value = value;
		}

		readonly object _value;
		public   object  Value { get { return _value; } }

		#region IExpressionScannable Members

		void IExpressionScannable.ForEach(Action<ISqlExpression> action)
		{
			action(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			SqlValue value = other as SqlValue;
			return _value == null && value._value == null || _value.Equals(value._value);
		}

		#endregion
	}
}
