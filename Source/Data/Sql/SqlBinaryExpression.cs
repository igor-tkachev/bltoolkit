using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLToolkit.Data.Sql
{
	[DebuggerDisplay("SQL = {SqlText}")]
	public class SqlBinaryExpression : ISqlExpression
	{
		public SqlBinaryExpression(Type systemType, ISqlExpression expr1, string operation, ISqlExpression expr2, int precedence)
		{
			if (expr1     == null) throw new ArgumentNullException("expr1");
			if (operation == null) throw new ArgumentNullException("operation");
			if (expr2     == null) throw new ArgumentNullException("expr2");

			_expr1      = expr1;
			_operation  = operation;
			_expr2      = expr2;
			_systemType = systemType;
			_precedence = precedence;
		}

		public SqlBinaryExpression(Type systemType, ISqlExpression expr1, string operation, ISqlExpression expr2)
			: this(systemType, expr1, operation, expr2, Sql.Precedence.Unknown)
		{
		}

		private  ISqlExpression _expr1;      public ISqlExpression Expr1      { get { return _expr1;      } }
		readonly string         _operation;  public string         Operation  { get { return _operation;  } }
		private  ISqlExpression _expr2;      public ISqlExpression Expr2      { get { return _expr2;      } }
		readonly Type           _systemType; public Type           SystemType { get { return _systemType; } }
		readonly int            _precedence; public int            Precedence { get { return _precedence; } }

		#region Overrides

		public string SqlText { get { return ToString(); } }

#if OVERRIDETOSTRING

		public override string ToString()
		{
			return ((IQueryElement)this).ToString(new StringBuilder(), new Dictionary<IQueryElement,IQueryElement>()).ToString();
		}

#endif

		#endregion

		#region ISqlExpressionWalkable Members

		[Obsolete]
		ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, Func<ISqlExpression,ISqlExpression> func)
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

			var expr = other as SqlBinaryExpression;

			return
				expr        != null &&
				_operation  == expr._operation &&
				_systemType == expr._systemType &&
				_expr1.Equals(expr._expr1) &&
				_expr2.Equals(expr._expr2);
		}

		#endregion

		#region ISqlExpression Members

		public bool CanBeNull()
		{
			return Expr1.CanBeNull() || Expr2.CanBeNull();
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
				objectTree.Add(this, clone = new SqlBinaryExpression(
					_systemType,
					(ISqlExpression)_expr1.Clone(objectTree, doClone),
					_operation,
					(ISqlExpression)_expr2.Clone(objectTree, doClone),
					_precedence));
			}

			return clone;
		}

		#endregion

		#region IQueryElement Members

		public QueryElementType ElementType { get { return QueryElementType.SqlBinaryExpression; } }

		StringBuilder IQueryElement.ToString(StringBuilder sb, Dictionary<IQueryElement,IQueryElement> dic)
		{
			Expr1
				.ToString(sb, dic)
				.Append(' ')
				.Append(Operation)
				.Append(' ');

			return Expr2.ToString(sb, dic);
		}

		#endregion
	}
}
