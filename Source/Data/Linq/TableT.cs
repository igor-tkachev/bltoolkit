using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using BLToolkit.Mapping;
using BLToolkit.Reflection;

using JetBrains.Annotations;

namespace BLToolkit.Data.Linq
{
	public class Table<T> : IOrderedQueryable<T>, IQueryProvider
	{
		#region Init

		public Table()
		{
			Expression = Expression.Constant(this);
		}

		public Table(IDataContext dataContext)
		{
			DataContext = dataContext;
			Expression  = Expression.Constant(this);
		}

		public Table(Expression expression)
		{
			Expression = expression;
		}

		public Table(IDataContext dataContext, Expression expression)
		{
			DataContext = dataContext;
			Expression  = expression;
		}

		public    Expression        Expression  { get; set; }
		public    IDataContext      DataContext { get; set; }

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
					_sqlTextHolder = info.GetSqlText(DataContext ?? new DbManager(), Expression, Parameters, 0);
				}

				return _sqlTextHolder;
			}
		}

		#endregion

		#region Execute

		IEnumerable<T> Execute(IDataContext dataContext, Expression expression)
		{
			return GetExpressionInfo(expression, true).GetIEnumerable(null, dataContext, expression, Parameters);
		}

		private ExpressionInfo<T> GetExpressionInfo(Expression expression, bool cache)
		{
			if (cache && Info != null)
				return Info;

			var contextID     = DataContext != null ? DataContext.ContextID         : Settings.GetDefaultContextID;
			var mappingSchema = DataContext != null ? DataContext.MappingSchema     : Map.DefaultSchema;
			var sqlProvider   = DataContext != null ? DataContext.CreateSqlProvider : Settings.GetDefaultCreateSqlProvider;

			var info = ExpressionInfo<T>.GetExpressionInfo(contextID, mappingSchema, sqlProvider, expression);

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

			return new Query<TElement>(DataContext, expression);
		}

		IQueryable IQueryProvider.CreateQuery(Expression expression)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			var elementType = TypeHelper.GetElementType(expression.Type) ?? expression.Type;

			try
			{
				return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] { DataContext, expression });
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
		}

		TResult IQueryProvider.Execute<TResult>(Expression expression)
		{
			return (TResult)GetExpressionInfo(expression, false).GetElement(null, DataContext, expression, Parameters);
		}

		object IQueryProvider.Execute(Expression expression)
		{
			return Execute(DataContext, expression);
		}

		#endregion

		#region IEnumerable Members

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return Execute(DataContext, Expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Execute(DataContext, Expression).GetEnumerator();
		}

		#endregion
	}
}
