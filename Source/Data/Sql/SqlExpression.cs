using System;
using System.Collections.Generic;
using System.Text;

namespace BLToolkit.Data.Sql
{
	public class SqlExpression : ISqlExpression
	{
		public SqlExpression(Type systemType, string expr, int precedence, params ISqlExpression[] parameters)
		{
			if (parameters == null) throw new ArgumentNullException("parameters");

			foreach (var value in parameters)
				if (value == null) throw new ArgumentNullException("parameters");

			_systemType = systemType;
			_expr       = expr;
			_precedence = precedence;
			_parameters     = parameters;
		}

		public SqlExpression(string expr, int precedence, params ISqlExpression[] parameters)
			: this(null, expr, precedence, parameters)
		{
		}

		public SqlExpression(Type systemType, string expr, params ISqlExpression[] parameters)
			: this(systemType, expr, Sql.Precedence.Unknown, parameters)
		{
		}

		public SqlExpression(string expr, params ISqlExpression[] parameters)
			: this(null, expr, Sql.Precedence.Unknown, parameters)
		{
		}

		readonly Type             _systemType; public Type             SystemType { get { return _systemType; } }
		readonly string           _expr;       public string           Expr       { get { return _expr;       } }
		readonly int              _precedence; public int              Precedence { get { return _precedence; } }
		readonly ISqlExpression[] _parameters; public ISqlExpression[] Parameters { get { return _parameters;     } }

		#region Overrides

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
			for (var i = 0; i < _parameters.Length; i++)
				_parameters[i] = _parameters[i].Walk(skipColumns, func);

			return func(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if (this == other)
				return true;

			var expr = other as SqlExpression;

			if (expr == null || _systemType != expr._systemType || _expr != expr._expr || _parameters.Length != expr._parameters.Length)
				return false;

			for (var i = 0; i < _parameters.Length; i++)
				if (!_parameters[i].Equals(expr._parameters[i]))
					return false;

			return true;
		}

		#endregion

		#region ISqlExpression Members

		public bool CanBeNull()
		{
			foreach (var value in Parameters)
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
					_systemType,
					_expr,
					_precedence,
					Array.ConvertAll(_parameters, e => (ISqlExpression)e.Clone(objectTree, doClone))));
			}

			return clone;
		}

		#endregion

		#region IQueryElement Members

		public QueryElementType ElementType { get { return QueryElementType.SqlExpression; } }

		StringBuilder IQueryElement.ToString(StringBuilder sb, Dictionary<IQueryElement,IQueryElement> dic)
		{
			var len = sb.Length;
			var ss  = Array.ConvertAll(Parameters, p =>
			{
				p.ToString(sb, dic);
				var s = sb.ToString(len, sb.Length - len);
				sb.Length = len;
				return s;
			});
			
			return sb.AppendFormat(Expr, ss);
		}

		#endregion

		#region Public Static Members

		public static bool NeedsEqual(IQueryElement ex)
		{
			switch (ex.ElementType)
			{
				case QueryElementType.SqlParameter:
				case QueryElementType.SqlField    :
				case QueryElementType.Column      :
				case QueryElementType.SqlFunction : return true;
			}

			return false;
		}

		#endregion
	}
}
