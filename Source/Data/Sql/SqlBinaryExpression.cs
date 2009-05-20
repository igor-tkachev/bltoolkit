using System;

namespace BLToolkit.Data.Sql
{
	public class SqlBinaryExpression : ISqlExpression
	{
		public SqlBinaryExpression(ISqlExpression expr1, string operation, ISqlExpression expr2, Type type, int precedence)
		{
			if (expr1     == null) throw new ArgumentNullException("expr1");
			if (operation == null) throw new ArgumentNullException("operation");
			if (expr2     == null) throw new ArgumentNullException("expr2");

			_expr1      = expr1;
			_operation  = operation;
			_expr2      = expr2;
			_type       = type;
			_precedence = precedence;
		}

		public SqlBinaryExpression(ISqlExpression expr1, string operation, ISqlExpression expr2, Type type)
			: this(expr1, operation, expr2, type, Sql.Precedence.Unknown)
		{
		}

		private  ISqlExpression _expr1;      public ISqlExpression Expr1      { get { return _expr1;      } }
		readonly string         _operation;  public string         Operation  { get { return _operation;  } }
		private  ISqlExpression _expr2;      public ISqlExpression Expr2      { get { return _expr2;      } }
		readonly Type           _type;       public Type           Type       { get { return _type;       } }
		readonly int            _precedence; public int            Precedence { get { return _precedence; } }

		#region Overrides

		public override string ToString()
		{
			return string.Format("{0} {1} {2}", Expr1, Operation, Expr2);
		}

		#endregion

		#region ISqlExpressionWalkable Members

		ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
		{
			_expr1 = _expr1.Walk(skipColumns, func);
			_expr2 = _expr2.Walk(skipColumns, func);

			return func(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if (this == other)
				return true;

			SqlBinaryExpression expr = other as SqlBinaryExpression;

			return expr != null && _operation == expr._operation && _expr1.Equals(expr._expr1) && _expr2.Equals(expr._expr2);
		}

		#endregion
	}
}
