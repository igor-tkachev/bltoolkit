using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Linq
{
	public class Table<T> : IOrderedQueryable<T>, IQueryProvider
	{
		#region Init

		public Table()
		{
			_expression = Expression.Constant(this);
		}

		public Table(DbManager dbManager)
		{
			DbManager   = dbManager;
			_expression = Expression.Constant(this);
		}

		public Table(Expression expression)
		{
			_expression = expression;
		}

		public Table(DbManager dbManager, Expression expression)
		{
			DbManager   = dbManager;
			_expression = expression;
		}

		readonly Expression _expression;

		public   DbManager   DbManager { get; set; }

		#endregion

		#region Execute

		IEnumerable<T> Execute(Expression expression)
		{
			return GetExpressionInfo(expression).GetIEnumerable(null, DbManager, expression);
		}

		private ExpressionInfo<T> GetExpressionInfo(Expression expression)
		{
			var dataProvider  = DbManager != null ? DbManager.DataProvider  : DbManager.GetDataProvider(DbManager.DefaultConfiguration);
			var mappingSchema = DbManager != null ? DbManager.MappingSchema : Map.DefaultSchema;

			return ExpressionInfo<T>.GetExpressionInfo(dataProvider, mappingSchema, expression);
		}

		#endregion

		#region Overrides

		public override string ToString()
		{
			return _expression.NodeType == ExpressionType.Constant && ((ConstantExpression)_expression).Value == this?
				"Table(" + typeof(T).Name + ")":
				_expression.ToString();
		}

		#endregion

		#region IQueryable Members

		Type IQueryable.ElementType
		{
			get { return typeof(T); }
		}

		Expression IQueryable.Expression
		{
			get { return _expression; }
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
			return (TResult)GetExpressionInfo(expression).GetElement(null, DbManager, expression);
		}

		object IQueryProvider.Execute(Expression expression)
		{
			return Execute(expression);
		}

		#endregion

		#region IEnumerable Members

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return Execute(_expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Execute(_expression).GetEnumerator();
		}

		#endregion
	}
}
