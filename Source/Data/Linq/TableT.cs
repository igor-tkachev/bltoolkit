using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace BLToolkit.Data.Linq
{
	using Reflection;

	public class Table<T> : IOrderedQueryable<T>, IQueryProvider
	{
		#region Init

		public Table(IDataContextInfo dataContextInfo, Expression expression)
		{
			DataContextInfo = dataContextInfo ?? new DefaultDataContextInfo();
			Expression      = expression      ?? Expression.Constant(this);
		}

		public Table(IDataContextInfo dataContextInfo)
			: this(dataContextInfo, null)
		{
		}

		public Table()
			: this((IDataContextInfo)null, null)
		{
		}

		public Table(IDataContext dataContext)
			: this(dataContext == null ? null : new DataContextInfo(dataContext), null)
		{
		}

		public Table(Expression expression)
			: this((IDataContextInfo)null, expression)
		{
		}

		public Table(IDataContext dataContext, Expression expression)
			: this(dataContext == null ? null : new DataContextInfo(dataContext), expression)
		{
		}

		[NotNull] public Expression       Expression      { get; set; }
		[NotNull] public IDataContextInfo DataContextInfo { get; set; }

		internal  ExpressionInfo<T> Info;
		internal  object[]          Parameters;

		#endregion

		#region Public Members

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string _sqlTextHolder;

// ReSharper disable InconsistentNaming
		[UsedImplicitly]
		private string _sqlText { get { return SqlText; }}
// ReSharper restore InconsistentNaming

		public  string  SqlText
		{
			get
			{
				if (_sqlTextHolder == null)
				{
					var info = GetExpressionInfo(Expression, true);
					_sqlTextHolder = info.GetSqlText(DataContextInfo.DataContext, Expression, Parameters, 0);
				}

				return _sqlTextHolder;
			}
		}

		#endregion

		#region Execute

		IEnumerable<T> Execute(IDataContextInfo dataContextInfo, Expression expression)
		{
			return GetExpressionInfo(expression, true).GetIEnumerable(null, dataContextInfo, expression, Parameters);
		}

		private ExpressionInfo<T> GetExpressionInfo(Expression expression, bool cache)
		{
			if (cache && Info != null)
				return Info;

			var info = ExpressionInfo<T>.GetExpressionInfo(DataContextInfo, expression);

			if (cache)
				Info = info;

			return info;
		}

		#endregion

		#region Overrides

#if OVERRIDETOSTRING

		public override string ToString()
		{
			return Expression.NodeType == ExpressionType.Constant && ((ConstantExpression)Expression).Value == this?
				"Table(" + typeof(T).Name + ")":
				Expression.ToString();
		}

#endif

		#endregion

		#region IQueryable Members

		Type IQueryable.ElementType
		{
			get { return typeof(T); }
		}

		Expression IQueryable.Expression
		{
			get { return Expression; }
		}

		IQueryProvider IQueryable.Provider
		{
			get { return this; }
		}

		#endregion

		#region IQueryProvider Members

		IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			return new Query<TElement>(DataContextInfo, expression);
		}

		IQueryable IQueryProvider.CreateQuery(Expression expression)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			var elementType = TypeHelper.GetElementType(expression.Type) ?? expression.Type;

			try
			{
				return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] { DataContextInfo, expression });
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
		}

		TResult IQueryProvider.Execute<TResult>(Expression expression)
		{
			return (TResult)GetExpressionInfo(expression, false).GetElement(null, DataContextInfo, expression, Parameters);
		}

		object IQueryProvider.Execute(Expression expression)
		{
			return Execute(DataContextInfo, expression);
		}

		#endregion

		#region IEnumerable Members

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return Execute(DataContextInfo, Expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Execute(DataContextInfo, Expression).GetEnumerator();
		}

		#endregion
	}
}
