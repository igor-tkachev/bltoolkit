using System;
using System.Collections.Generic;

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

		#region ISqlExpressionWalkable Members

			[Obsolete]
		ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
		{
			for (int i = 0; i < _values.Length; i++)
				_values[i] = _values[i].Walk(skipColumns, func);

			return func(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if (this == other)
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

		#region ISqlExpression Members

		public bool CanBeNull()
		{
			foreach (ISqlExpression value in Values)
				if (value.CanBeNull())
					return true;

			return false;
		}

		#endregion

		#region ICloneableElement Members

		public ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
		{
			if (!doClone(this))
				return this;

			ICloneableElement clone;

			if (!objectTree.TryGetValue(this, out clone))
			{
				objectTree.Add(this, clone = new SqlExpression(
					_expr,
					_precedence,
					Array.ConvertAll<ISqlExpression,ISqlExpression>(
						_values,
						delegate(ISqlExpression e) { return (ISqlExpression)e.Clone(objectTree, doClone); })));
			}

			return clone;
		}

		#endregion

		#region IQueryElement Members

		public QueryElementType ElementType { get { return QueryElementType.SqlExpression; } }

		#endregion
	}
}
