using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Linq
{
	public class Table<T> : IOrderedQueryable<T>, IQueryProvider
	{
		#region Init

		public Table()
		{
			Expression = Expression.Constant(this);
		}

		public Table(DbManager dbManager)
		{
			DbManager  = dbManager;
			Expression = Expression.Constant(this);
		}

		public Table(Expression expression)
		{
			Expression = expression;
		}

		public Table(DbManager dbManager, Expression expression)
		{
			DbManager  = dbManager;
			Expression = expression;
		}

		protected Expression        Expression;
		public    DbManager         DbManager { get; set; }
		internal  ExpressionInfo<T> Info;
		internal  object[]          Parameters;

		#endregion

		#region Execute

		IEnumerable<T> Execute(DbManager db, Expression expression)
		{
			return GetExpressionInfo(expression).GetIEnumerable(null, db, expression, Parameters);
		}

		private ExpressionInfo<T> GetExpressionInfo(Expression expression)
		{
			if (Info != null)
				return Info;

			var dataProvider  = DbManager != null ? DbManager.DataProvider  : DbManager.GetDataProvider(DbManager.DefaultConfiguration);
			var mappingSchema = DbManager != null ? DbManager.MappingSchema : Map.DefaultSchema;

			return Info = ExpressionInfo<T>.GetExpressionInfo(dataProvider, mappingSchema, expression);
		}

		#endregion

		#region Overrides

		public override string ToString()
		{
			return Expression.NodeType == ExpressionType.Constant && ((ConstantExpression)Expression).Value == this?
				"Table(" + typeof(T).Name + ")":
				Expression.ToString();
		}

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

			return new Table<TElement>(DbManager, expression);
		}

		IQueryable IQueryProvider.CreateQuery(Expression expression)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			var elementType = TypeHelper.GetElementType(expression.Type) ?? expression.Type;

			try
			{
				return (IQueryable)Activator.CreateInstance(typeof(Table<>).MakeGenericType(elementType), new object[] { DbManager, expression });
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
		}

		TResult IQueryProvider.Execute<TResult>(Expression expression)
		{
			return (TResult)GetExpressionInfo(expression).GetElement(null, DbManager, expression, Parameters);
		}

		object IQueryProvider.Execute(Expression expression)
		{
			return Execute(DbManager, expression);
		}

		#endregion

		#region IEnumerable Members

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return Execute(DbManager, Expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Execute(DbManager, Expression).GetEnumerator();
		}

		#endregion
	}
}
