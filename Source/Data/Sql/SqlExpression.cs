using System;

namespace BLToolkit.Data.Sql
{
	public class SqlExpression : ISqlExpression
	{
		public SqlExpression(string expr, params ISqlExpression[] values)
		{
			if (values == null) throw new ArgumentNullException("values");

			foreach (ISqlExpression value in values)
				if (value == null) throw new ArgumentNullException("values");

			_expr   = expr;
			_values = values;
		}

		readonly string           _expr;
		readonly ISqlExpression[] _values;

		#region ISqlExpressionScannable Members

		void ISqlExpressionScannable.ForEach(Action<ISqlExpression> action)
		{
			action(this);
			Array.ForEach(_values, action);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			SqlExpression expr = other as SqlExpression;

			if (expr == null || _expr != expr._expr || _values.Length != expr._values.Length)
				return false;

			for (int i = 0; i < _values.Length; i++)
				if (!_values[i].Equals(expr._values[i]))
					return false;

			return true;
		}

		#endregion
	}
}
