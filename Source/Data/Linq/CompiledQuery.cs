using System;
using System.Linq;
using System.Linq.Expressions;

using BLToolkit.Reflection;

namespace BLToolkit.Data.Linq
{
	/// <summary>
	/// Provides for compilation and caching of queries for reuse.
	/// </summary>
	public class CompiledQuery
	{
		protected CompiledQuery(LambdaExpression query)
		{
			_query = query;
		}

		readonly object                _sync = new object();
		readonly LambdaExpression      _query;
		private  Func<object[],object> _compiledQuery;

		TResult ExecuteQuery<TResult>(params object[] args)
		{
			if (_compiledQuery == null)
				lock (_sync)
					if (_compiledQuery == null)
						_compiledQuery = CompileQuery(_query);

			return (TResult)_compiledQuery(args);
		}

		private interface ITableHelper
		{
			Expression CallTable(LambdaExpression query, Expression expr, ParameterExpression ps, bool isQueriable);
		}

		internal class TableHelper<T> : ITableHelper
		{
			public Expression CallTable(LambdaExpression query, Expression expr, ParameterExpression ps, bool isQueriable)
			{
				var table = new CompiledTable<T>(query, expr);

				return Expression.Call(
					Expression.Constant(table),
					isQueriable ?
						ReflectionHelper.Expressor<CompiledTable<T>>.MethodExpressor(t => t.Create (null)) :
						ReflectionHelper.Expressor<CompiledTable<T>>.MethodExpressor(t => t.Execute(null)),
					ps);
			}
		}

		static Func<object[],object> CompileQuery(LambdaExpression query)
		{
			var ps = Expression.Parameter(typeof(object[]), "ps");

			var info = query.Body.Convert(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.Parameter:
						return 
							Expression.Convert(
								Expression.ArrayIndex(
									ps,
									Expression.Constant(query.Parameters.IndexOf((ParameterExpression)pi))),
								pi.Type);

					case ExpressionType.Call:
						{
							var expr = (MethodCallExpression)pi;

							if (expr.Method.DeclaringType == typeof(Queryable))
							{
								var qtype  = TypeHelper.GetGenericType(typeof (IQueryable<>), expr.Type);
								var helper = (ITableHelper)Activator.CreateInstance(
									typeof (TableHelper<>).MakeGenericType(qtype == null ? expr.Type : qtype.GetGenericArguments()[0]));

								return helper.CallTable(query, expr, ps, qtype != null);
							}
						}

						break;
				}

				return pi;
			});

			return Expression.Lambda<Func<object[],object>>(Expression.Convert(info, typeof(object)), ps).Compile();
		}

		#region Invoke

		public TResult Invoke<TDb,TResult>(TDb dbManager)
		{
			return ExecuteQuery<TResult>(dbManager);
		}

		public TResult Invoke<TDb,T1,TResult>(TDb dbManager, T1 arg1)
		{
			return ExecuteQuery<TResult>(dbManager, arg1);
		}

		public TResult Invoke<TDb,T1,T2,TResult>(TDb dbManager, T1 arg1, T2 arg2)
		{
			return ExecuteQuery<TResult>(dbManager, arg1, arg2);
		}

		public TResult Invoke<TDb,T1,T2,T3,TResult>(TDb dbManager, T1 arg1, T2 arg2, T3 arg3)
		{
			return ExecuteQuery<TResult>(dbManager, arg1, arg2, arg3);
		}

		public TResult Invoke<TDb,T1,T2,T3,T4,TResult>(TDb dbManager, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			return ExecuteQuery<TResult>(dbManager, arg1, arg2, arg3, arg4);
		}

		public TResult Invoke<TDb,T1,T2,T3,T4,T5,TResult>(TDb dbManager, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			return ExecuteQuery<TResult>(dbManager, arg1, arg2, arg3, arg4, arg5);
		}

		#endregion

		#region Compile

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDb">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		public static Func<TDb,TResult> Compile<TDb,TResult>(
			[JetBrains.Annotations.NotNull] Expression<Func<TDb,TResult>> query)
			  where TDb : DbManager
		{
			if (query == null) throw new ArgumentNullException("query");
			return new CompiledQuery(query).Invoke<TDb,TResult>;
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDb">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg1">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		public static Func<TDb,TArg1,TResult> Compile<TDb,TArg1,TResult>(
			[JetBrains.Annotations.NotNull] Expression<Func<TDb,TArg1,TResult>> query)
			where TDb : DbManager
		{
			if (query == null) throw new ArgumentNullException("query");
			return new CompiledQuery(query).Invoke<TDb,TArg1,TResult>;
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDb">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg1">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg2">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		public static Func<TDb,TArg1,TArg2,TResult> Compile<TDb,TArg1,TArg2,TResult>(
			[JetBrains.Annotations.NotNull] Expression<Func<TDb,TArg1,TArg2,TResult>> query)
			where TDb : DbManager
		{
			if (query == null) throw new ArgumentNullException("query");
			return new CompiledQuery(query).Invoke<TDb,TArg1,TArg2,TResult>;
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDb">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg1">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg2">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg3">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		public static Func<TDb,TArg1,TArg2,TArg3,TResult> Compile<TDb,TArg1,TArg2,TArg3,TResult>(
			[JetBrains.Annotations.NotNull] Expression<Func<TDb,TArg1,TArg2,TArg3,TResult>> query)
			where TDb : DbManager
		{
			if (query == null) throw new ArgumentNullException("query");
			return new CompiledQuery(query).Invoke<TDb,TArg1,TArg2,TArg3,TResult>;
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDb">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg1">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg2">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg3">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg4">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		public static Func<TDb,TArg1,TArg2,TArg3,TArg4,TResult> Compile<TDb,TArg1,TArg2,TArg3,TArg4,TResult>(
			[JetBrains.Annotations.NotNull] Expression<Func<TDb,TArg1,TArg2,TArg3,TArg4,TResult>> query)
			where TDb : DbManager
		{
			if (query == null) throw new ArgumentNullException("query");
			return new CompiledQuery(query).Invoke<TDb,TArg1,TArg2,TArg3,TArg4,TResult>;
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDb">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg1">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg2">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg3">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg4">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg5">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		public static Func<TDb,TArg1,TArg2,TArg3,TArg4,TArg5,TResult> Compile<TDb,TArg1,TArg2,TArg3,TArg4,TArg5,TResult>(
			[JetBrains.Annotations.NotNull] Expression<Func<TDb,TArg1,TArg2,TArg3,TArg4,TArg5,TResult>> query)
			where TDb : DbManager
		{
			if (query == null) throw new ArgumentNullException("query");
			return new CompiledQuery(query).Invoke<TDb,TArg1,TArg2,TArg3,TArg4,TArg5,TResult>;
		}

		#endregion
	}
}
