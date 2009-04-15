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

		#region Overrides

		public override string ToString()
		{
			if (_value == null)
				return "NULL";

			if (_value is string)
				return "'" + _value.ToString().Replace("\'", "''");

			return _value.ToString();
		}

		#endregion

		#region ISqlExpression Members

		public int Precedence
		{
			get { return Sql.Precedence.Primary; }
		}

		#endregion

		#region ISqlExpressionScannable Members

		void ISqlExpressionScannable.ForEach(bool skipColumns, Action<ISqlExpression> action)
		{
			action(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if (this == other)
				return true;

			SqlValue value = other as SqlValue;
			return _value == null && value._value == null || _value != null && _value.Equals(value._value);
		}

		#endregion
	}
}
