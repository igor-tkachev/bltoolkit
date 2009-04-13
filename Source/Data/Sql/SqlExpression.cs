using System;

namespace BLToolkit.Data.Sql
{
	public class SqlExpression : ISqlExpression
	{
		public SqlExpression(string expr, int precedence, params ISqlExpression[] values)
		{
			if (values == null) throw new ArgumentNullException("values");

			foreach (ISqlExpression value in values)
				if (value == null) throw new ArgumentNullException("values");

			_expr       = expr;
			_precedence = precedence;
			_values     = values;
		}

		public SqlExpression(string expr, params ISqlExpression[] values)
			: this(expr, Sql.Precedence.Unknown, values)
		{
		}

		readonly string           _expr;       public string           Expr       { get { return _expr;       } }
		readonly int              _precedence; public int              Precedence { get { return _precedence; } }
		readonly ISqlExpression[] _values;     public ISqlExpression[] Values     { get { return _values;     } }

		#region Overrides

		public override string ToString()
		{
			return string.Format(Expr, Values);
		}

		#endregion

		#region ISqlExpressionScannable Members

		void ISqlExpressionScannable.ForEach(bool skipColumns, Action<ISqlExpression> action)
		{
			action(this);
			Array.ForEach(_values, e => e.ForEach(skipColumns, action));
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if ((object)this == other)
				return true;

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
