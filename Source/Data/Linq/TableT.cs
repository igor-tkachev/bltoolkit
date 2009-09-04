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
			_expression = Expression.Constant(this);
		}

		public Table(DbManager dbManager)
		{
			_dbManager = dbManager;
			_expression = Expression.Constant(this);
		}

		public Table(Expression expression)
		{
			_expression = expression;
		}

		public Table(DbManager dbManager, Expression expression)
		{
			_dbManager  = dbManager;
			_expression = expression;
		}

		readonly Expression _expression;

		private DbManager _dbManager;
		public  DbManager  DbManager
		{
			get { return _dbManager;  }
			set { _dbManager = value; }
		}

		#endregion

		#region Execute

		IEnumerable<T> Execute(Expression expression)
		{
			var info = ExpressionInfo<T>.GetExpressionInfo(
				_dbManager != null? _dbManager.DataProvider:  DbManager.GetDataProvider(DbManager.DefaultConfiguration),
				_dbManager != null? _dbManager.MappingSchema: Map.DefaultSchema,
				expression);

			return info.GetIEnumerable(null, _dbManager, expression);
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

			return new Table<TElement>(_dbManager, expression);
		}

		IQueryable IQueryProvider.CreateQuery(Expression expression)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			var elementType   = TypeHelper.GetElementType(expression.Type) ?? expression.Type;
			//var queryableType = typeof(IQueryable<>).MakeGenericType(new [] { elementType });

			try
			{
				return (IQueryable)Activator.CreateInstance(typeof(Table<>).MakeGenericType(elementType), new object[] { _dbManager, expression });
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
		}

		TResult IQueryProvider.Execute<TResult>(Expression expression)
		{
			var info = ExpressionInfo<T>.GetExpressionInfo(
				_dbManager != null? _dbManager.DataProvider:  DbManager.GetDataProvider(DbManager.DefaultConfiguration),
				_dbManager != null? _dbManager.MappingSchema: Map.DefaultSchema,
				expression);

			return (TResult)info.GetElement(null, _dbManager, expression);
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
