using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace BLToolkit.Data.Linq
{
	using DataAccess;

	public static class Extensions
	{
		#region Table Helpers

		static public Table<T> GetTable<T>(this IDataContext dataContext)
			where T : class
		{
			return new Table<T>(dataContext);
		}

		static public Table<T> TableName<T>([NotNull] this Table<T> table, [NotNull] string name)
		{
			if (table == null) throw new ArgumentNullException("table");
			if (name  == null) throw new ArgumentNullException("name");

			table.Expression = Expression.Call(
				null,
				((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
				new[] { table.Expression, Expression.Constant(name) });

			return table;
		}

		static public Table<T> DatabaseName<T>([NotNull] this Table<T> table, [NotNull] string name)
		{
			if (table == null) throw new ArgumentNullException("table");
			if (name  == null) throw new ArgumentNullException("name");

			table.Expression = Expression.Call(
				null,
				((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
				new[] { table.Expression, Expression.Constant(name) });

			return table;
		}

		static public Table<T> OwnerName<T>([NotNull] this Table<T> table, [NotNull] string name)
		{
			if (table == null) throw new ArgumentNullException("table");
			if (name  == null) throw new ArgumentNullException("name");

			table.Expression = Expression.Call(
				null,
				((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
				new[] { table.Expression, Expression.Constant(name) });

			return table;
		}

		#endregion

		#region Scalar Select

		static public T Select<T>([NotNull] this IDataContext dataContext, [NotNull] Expression<Func<T>> selector)
		{
			if (dataContext == null) throw new ArgumentNullException("dataContext");
			if (selector    == null) throw new ArgumentNullException("selector");

			var q = new Table<T>(dataContext, selector);

			foreach (var item in q)
				return item;

			throw new InvalidOperationException();
		}

		#endregion

		#region Delete

		public static int Delete<T>([NotNull] this IQueryable<T> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			return source.Provider.Execute<int>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { source.Expression }));
		}

		public static int Delete<T>([NotNull] this IQueryable<T> source, [NotNull] Expression<Func<T,bool>> predicate)
		{
			if (source    == null) throw new ArgumentNullException("source");
			if (predicate == null) throw new ArgumentNullException("predicate");

			return source.Provider.Execute<int>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { source.Expression, Expression.Quote(predicate) }));
		}

		#endregion

		#region Update

		public static int Update<TSource,TTarget>(
			[NotNull] this IQueryable<TSource>          source,
			[NotNull] Table<TTarget>                    target,
			[NotNull] Expression<Func<TSource,TTarget>> setter)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (target == null) throw new ArgumentNullException("target");
			if (setter == null) throw new ArgumentNullException("setter");

			return source.Provider.Execute<int>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource), typeof(TTarget) }),
					new[] { source.Expression, ((IQueryable<TTarget>)target).Expression, Expression.Quote(setter) }));
		}

		public static int Update<T>([NotNull] this IQueryable<T> source, [NotNull] Expression<Func<T,T>> setter)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (setter == null) throw new ArgumentNullException("setter");

			return source.Provider.Execute<int>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { source.Expression, Expression.Quote(setter) }));
		}

		public static int Update<T>([NotNull] this IQueryable<T> source, [NotNull] Expression<Func<T,bool>> predicate, [NotNull] Expression<Func<T,T>> setter)
		{
			if (source    == null) throw new ArgumentNullException("source");
			if (predicate == null) throw new ArgumentNullException("predicate");
			if (setter    == null) throw new ArgumentNullException("setter");

			return source.Provider.Execute<int>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { source.Expression, Expression.Quote(predicate), Expression.Quote(setter) }));
		}

		public static int Update<T>([NotNull] this IUpdateable<T> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			var query = ((Updateable<T>)source).Query;

			return query.Provider.Execute<int>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { query.Expression }));
		}

		class Updateable<T> : IUpdateable<T>
		{
			public IQueryable<T> Query;
		}

		public static IUpdateable<T> Set<T,TV>(
			[NotNull] this IQueryable<T>     source,
			[NotNull] Expression<Func<T,TV>> extract,
			[NotNull] Expression<Func<T,TV>> update)
		{
			if (source  == null) throw new ArgumentNullException("source");
			if (extract == null) throw new ArgumentNullException("extract");
			if (update  == null) throw new ArgumentNullException("update");

			var query = source.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T), typeof(TV) }),
					new[] { source.Expression, Expression.Quote(extract), Expression.Quote(update) }));

			return new Updateable<T> { Query = query };
		}

		public static IUpdateable<T> Set<T,TV>(
			[NotNull] this IUpdateable<T>    source,
			[NotNull] Expression<Func<T,TV>> extract,
			[NotNull] Expression<Func<T,TV>> update)
		{
			if (source  == null) throw new ArgumentNullException("source");
			if (extract == null) throw new ArgumentNullException("extract");
			if (update  == null) throw new ArgumentNullException("update");

			var query = ((Updateable<T>)source).Query;

			query = query.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T), typeof(TV) }),
					new[] { query.Expression, Expression.Quote(extract), Expression.Quote(update) }));

			return new Updateable<T> { Query = query };
		}

		public static IUpdateable<T> Set<T,TV>(
			[NotNull] this IQueryable<T>     source,
			[NotNull] Expression<Func<T,TV>> extract,
			[NotNull] Expression<Func<TV>>   update)
		{
			if (source  == null) throw new ArgumentNullException("source");
			if (extract == null) throw new ArgumentNullException("extract");
			if (update  == null) throw new ArgumentNullException("update");

			var query = source.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T), typeof(TV) }),
					new[] { source.Expression, Expression.Quote(extract), Expression.Quote(update) }));

			return new Updateable<T> { Query = query };
		}

		public static IUpdateable<T> Set<T,TV>(
			[NotNull] this IUpdateable<T>    source,
			[NotNull] Expression<Func<T,TV>> extract,
			[NotNull] Expression<Func<TV>>   update)
		{
			if (source  == null) throw new ArgumentNullException("source");
			if (extract == null) throw new ArgumentNullException("extract");
			if (update  == null) throw new ArgumentNullException("update");

			var query = ((Updateable<T>)source).Query;

			query = query.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T), typeof(TV) }),
					new[] { query.Expression, Expression.Quote(extract), Expression.Quote(update) }));

			return new Updateable<T> { Query = query };
		}

		public static IUpdateable<T> Set<T,TV>(
			[NotNull] this IQueryable<T>     source,
			[NotNull] Expression<Func<T,TV>> extract,
			TV                               value)
		{
			if (source  == null) throw new ArgumentNullException("source");
			if (extract == null) throw new ArgumentNullException("extract");

			var query = source.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T), typeof(TV) }),
					new[] { source.Expression, Expression.Quote(extract), Expression.Constant(value, typeof(TV)) }));

			return new Updateable<T> { Query = query };
		}

		public static IUpdateable<T> Set<T,TV>(
			[NotNull] this IUpdateable<T>    source,
			[NotNull] Expression<Func<T,TV>> extract,
			TV                               value)
		{
			if (source  == null) throw new ArgumentNullException("source");
			if (extract == null) throw new ArgumentNullException("extract");

			var query = ((Updateable<T>)source).Query;

			query = query.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T), typeof(TV) }),
					new[] { query.Expression, Expression.Quote(extract), Expression.Constant(value, typeof(TV)) }));

			return new Updateable<T> { Query = query };
		}

		#endregion

		#region Insert

		#region ValueInsertable

		public static int Insert<T>([NotNull] this Table<T> target, [NotNull] Expression<Func<T>> setter)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (setter == null) throw new ArgumentNullException("setter");

			IQueryable<T> query = target;

			return query.Provider.Execute<int>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { query.Expression, Expression.Quote(setter) }));
		}

		public static object InsertWithIdentity<T>([NotNull] this Table<T> target, [NotNull] Expression<Func<T>> setter)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (setter == null) throw new ArgumentNullException("setter");

			IQueryable<T> query = target;

			return query.Provider.Execute<object>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { query.Expression, Expression.Quote(setter) }));
		}

		class ValueInsertable<T> : IValueInsertable<T>
		{
			public IQueryable<T> Query;
		}

		public static IValueInsertable<T> Into<T>(this IDataContext dataContext, [NotNull] Table<T> target)
		{
			if (target == null) throw new ArgumentNullException("target");

			IQueryable<T> query = target;

			var q = query.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { Expression.Constant(null, typeof(IDataContext)), query.Expression }));

			return new ValueInsertable<T> { Query = q };
		}

		public static IValueInsertable<T> Value<T,TV>(
			[NotNull] this Table<T>          source,
			[NotNull] Expression<Func<T,TV>> field,
			[NotNull] Expression<Func<TV>>   value)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (field  == null) throw new ArgumentNullException("field");
			if (value  == null) throw new ArgumentNullException("value");

			var query = (IQueryable<T>)source;

			var q = query.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T), typeof(TV) }),
					new[] { query.Expression, Expression.Quote(field), Expression.Quote(value) }));

			return new ValueInsertable<T> { Query = q };
		}

		public static IValueInsertable<T> Value<T,TV>(
			[NotNull] this Table<T>          source,
			[NotNull] Expression<Func<T,TV>> field,
			TV                               value)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (field  == null) throw new ArgumentNullException("field");

			var query = (IQueryable<T>)source;

			var q = query.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T), typeof(TV) }),
					new[] { query.Expression, Expression.Quote(field), Expression.Constant(value, typeof(TV)) }));

			return new ValueInsertable<T> { Query = q };
		}

		public static IValueInsertable<T> Value<T,TV>(
			[NotNull] this IValueInsertable<T> source,
			[NotNull] Expression<Func<T,TV>>   field,
			[NotNull] Expression<Func<TV>>     value)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (field  == null) throw new ArgumentNullException("field");
			if (value  == null) throw new ArgumentNullException("value");

			var query = ((ValueInsertable<T>)source).Query;

			var q = query.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T), typeof(TV) }),
					new[] { query.Expression, Expression.Quote(field), Expression.Quote(value) }));

			return new ValueInsertable<T> { Query = q };
		}

		public static IValueInsertable<T> Value<T,TV>(
			[NotNull] this IValueInsertable<T> source,
			[NotNull] Expression<Func<T,TV>>   field,
			TV                                 value)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (field  == null) throw new ArgumentNullException("field");

			var query = ((ValueInsertable<T>)source).Query;

			var q = query.Provider.CreateQuery<T>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T), typeof(TV) }),
					new[] { query.Expression, Expression.Quote(field), Expression.Constant(value, typeof(TV)) }));

			return new ValueInsertable<T> { Query = q };
		}

		public static int Insert<T>([NotNull] this IValueInsertable<T> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			var query = ((ValueInsertable<T>)source).Query;

			return query.Provider.Execute<int>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { query.Expression }));
		}

		public static object InsertWithIdentity<T>([NotNull] this IValueInsertable<T> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			var query = ((ValueInsertable<T>)source).Query;

			return query.Provider.Execute<object>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { query.Expression }));
		}

		#endregion

		#region SelectInsertable

		public static int Insert<TSource,TTarget>(
			[NotNull] this IQueryable<TSource> source, [NotNull] Table<TTarget> target, [NotNull] Expression<Func<TSource,TTarget>> setter)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (target == null) throw new ArgumentNullException("target");
			if (setter == null) throw new ArgumentNullException("setter");

			return source.Provider.Execute<int>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource), typeof(TTarget) }),
					new[] { source.Expression, ((IQueryable<TTarget>)target).Expression, Expression.Quote(setter) }));
		}

		public static object InsertWithIdentity<TSource,TTarget>(
			[NotNull] this IQueryable<TSource> source, [NotNull] Table<TTarget> target, [NotNull] Expression<Func<TSource,TTarget>> setter)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (target == null) throw new ArgumentNullException("target");
			if (setter == null) throw new ArgumentNullException("setter");

			return source.Provider.Execute<object>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource), typeof(TTarget) }),
					new[] { source.Expression, ((IQueryable<TTarget>)target).Expression, Expression.Quote(setter) }));
		}

		class SelectInsertable<T,TT> : ISelectInsertable<T,TT>
		{
			public IQueryable<T> Query;
		}

		public static ISelectInsertable<TSource,TTarget> Into<TSource,TTarget>(
			[NotNull] this IQueryable<TSource> source,
			[NotNull] Table<TTarget>           target)
		{
			if (target == null) throw new ArgumentNullException("target");

			var q = source.Provider.CreateQuery<TSource>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource), typeof(TTarget) }),
					new[] { source.Expression, ((IQueryable<TTarget>)target).Expression }));

			return new SelectInsertable<TSource,TTarget> { Query = q };
		}

		public static ISelectInsertable<TSource,TTarget> Value<TSource,TTarget,TValue>(
			[NotNull] this ISelectInsertable<TSource,TTarget> source,
			[NotNull] Expression<Func<TTarget,TValue>>        field,
			[NotNull] Expression<Func<TSource,TValue>>        value)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (field  == null) throw new ArgumentNullException("field");
			if (value  == null) throw new ArgumentNullException("value");

			var query = ((SelectInsertable<TSource,TTarget>)source).Query;

			var q = query.Provider.CreateQuery<TSource>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource), typeof(TTarget), typeof(TValue) }),
					new[] { query.Expression, Expression.Quote(field), Expression.Quote(value) }));

			return new SelectInsertable<TSource,TTarget> { Query = q };
		}

		public static ISelectInsertable<TSource,TTarget> Value<TSource,TTarget,TValue>(
			[NotNull] this ISelectInsertable<TSource,TTarget> source,
			[NotNull] Expression<Func<TTarget,TValue>>        field,
			[NotNull] Expression<Func<TValue>>                value)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (field  == null) throw new ArgumentNullException("field");
			if (value  == null) throw new ArgumentNullException("value");

			var query = ((SelectInsertable<TSource,TTarget>)source).Query;

			var q = query.Provider.CreateQuery<TSource>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource), typeof(TTarget), typeof(TValue) }),
					new[] { query.Expression, Expression.Quote(field), Expression.Quote(value) }));

			return new SelectInsertable<TSource,TTarget> { Query = q };
		}

		public static ISelectInsertable<TSource,TTarget> Value<TSource,TTarget,TValue>(
			[NotNull] this ISelectInsertable<TSource,TTarget> source,
			[NotNull] Expression<Func<TTarget,TValue>>        field,
			TValue                                            value)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (field  == null) throw new ArgumentNullException("field");

			var query = ((SelectInsertable<TSource,TTarget>)source).Query;

			var q = query.Provider.CreateQuery<TSource>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource), typeof(TTarget), typeof(TValue) }),
					new[] { query.Expression, Expression.Quote(field), Expression.Constant(value, typeof(TValue)) }));

			return new SelectInsertable<TSource,TTarget> { Query = q };
		}

		public static int Insert<TSource,TTarget>([NotNull] this ISelectInsertable<TSource,TTarget> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			var query = ((SelectInsertable<TSource,TTarget>)source).Query;

			return query.Provider.Execute<int>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource), typeof(TTarget) }),
					new[] { query.Expression }));
		}

		public static object InsertWithIdentity<TSource,TTarget>([NotNull] this ISelectInsertable<TSource,TTarget> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			var query = ((SelectInsertable<TSource,TTarget>)source).Query;

			return query.Provider.Execute<object>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource), typeof(TTarget) }),
					new[] { query.Expression }));
		}

		#endregion

		#endregion

		#region Compile

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="dataContext"></param>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDc">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		static public Func<TDc,TResult> Compile<TDc,TResult>(
			[NotNull] this IDataContext dataContext,
			[NotNull] Expression<Func<TDc,TResult>> query)
			where TDc : IDataContext
		{
			return CompiledQuery.Compile(query);
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="dataContext"></param>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDc">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg1">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		static public Func<TDc,TArg1,TResult> Compile<TDc,TArg1, TResult>(
			[NotNull] this IDataContext dataContext,
			[NotNull] Expression<Func<TDc,TArg1,TResult>> query)
			where TDc : IDataContext
		{
			return CompiledQuery.Compile(query);
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="dataContext"></param>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDc">
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
		static public Func<TDc,TArg1,TArg2,TResult> Compile<TDc,TArg1,TArg2,TResult>(
			[NotNull] this IDataContext dataContext,
			[NotNull] Expression<Func<TDc,TArg1,TArg2,TResult>> query)
			where TDc : IDataContext
		{
			return CompiledQuery.Compile(query);
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="dataContext"></param>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDc">
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
		static public Func<TDc,TArg1,TArg2,TArg3,TResult> Compile<TDc,TArg1,TArg2,TArg3,TResult>(
			[NotNull] this IDataContext dataContext,
			[NotNull] Expression<Func<TDc,TArg1,TArg2,TArg3,TResult>> query)
			where TDc : IDataContext
		{
			return CompiledQuery.Compile(query);
		}

		#endregion

		#region Object Operations

		#region Insert

		public static int Insert<T>([NotNull] this IDataContextInfo dataContextInfo, T obj)
		{
			if (dataContextInfo == null) throw new ArgumentNullException("dataContextInfo");
			return ExpressionInfo<T>.Insert(dataContextInfo, obj);
		}

		public static int Insert<T>(this IDataContext dataContext, T obj)
		{
			return ExpressionInfo<T>.Insert(DataContextInfo.Create(dataContext), obj);
		}

		#endregion

		#region InsertBatch

#if !SILVERLIGHT

		public static int InsertBatch<T>(this DbManager dataContext, int maxBatchSize, IEnumerable<T> list)
		{
			return new SqlQuery<T>().Insert(dataContext, maxBatchSize, list);
		}

		public static int InsertBatch<T>(this DbManager dataContext, IEnumerable<T> list)
		{
			return InsertBatch(dataContext, int.MaxValue, list);
		}

		public static int InsertBatch<T>(this DbManager dataContext, T[] list)
		{
			return InsertBatch(dataContext, int.MaxValue, list);
		}

		[Obsolete("Use InsertBatch instead.")]
		public static int Insert<T>(this DbManager dataContext, T[] list)
		{
			return Insert(dataContext, int.MaxValue, list);
		}

		[Obsolete("Use InsertBatch instead.")]
		public static int Insert<T>(this DbManager dataContext, int maxBatchSize, IEnumerable<T> list)
		{
			return new SqlQuery<T>().Insert(dataContext, maxBatchSize, list);
		}

		[Obsolete("Use InsertBatch instead.")]
		public static int Insert<T>(this DbManager dataContext, IEnumerable<T> list)
		{
			return Insert(dataContext, int.MaxValue, list);
		}

#endif

		#endregion

		#region InsertWithIdentity

		public static object InsertWithIdentity<T>([NotNull] this IDataContextInfo dataContextInfo, T obj)
		{
			if (dataContextInfo == null) throw new ArgumentNullException("dataContextInfo");
			return ExpressionInfo<T>.InsertWithIdentity(dataContextInfo, obj);
		}

		public static object InsertWithIdentity<T>(this IDataContext dataContext, T obj)
		{
			return ExpressionInfo<T>.InsertWithIdentity(DataContextInfo.Create(dataContext), obj);
		}

		#endregion

		#region Update

		public static int Update<T>([NotNull] this IDataContextInfo dataContextInfo, T obj)
		{
			if (dataContextInfo == null) throw new ArgumentNullException("dataContextInfo");
			return ExpressionInfo<T>.Update(dataContextInfo, obj);
		}

		public static int Update<T>(this IDataContext dataContext, T obj)
		{
			return ExpressionInfo<T>.Update(DataContextInfo.Create(dataContext), obj);
		}

#if !SILVERLIGHT

		public static int Update<T>(this DbManager dataContext, int maxBatchSize, IEnumerable<T> list)
		{
			return new SqlQuery<T>().Update(dataContext, maxBatchSize, list);
		}

		public static int Update<T>(this DbManager dataContext, IEnumerable<T> list)
		{
			return Update(dataContext, int.MaxValue, list);
		}

#endif

		#endregion

		#region Delete

		public static int Delete<T>([NotNull] this IDataContextInfo dataContextInfo, T obj)
		{
			if (dataContextInfo == null) throw new ArgumentNullException("dataContextInfo");
			return ExpressionInfo<T>.Delete(dataContextInfo, obj);
		}

		public static int Delete<T>([NotNull] this IDataContext dataContext, T obj)
		{
			return ExpressionInfo<T>.Delete(DataContextInfo.Create(dataContext), obj);
		}

#if !SILVERLIGHT

		public static int Delete<T>(this DbManager dataContext, int maxBatchSize, IEnumerable<T> list)
		{
			return new SqlQuery<T>().Delete(dataContext, maxBatchSize, list);
		}

		public static int Delete<T>(this DbManager dataContext, IEnumerable<T> list)
		{
			return Delete(dataContext, int.MaxValue, list);
		}

#endif

		#endregion

		#endregion

		#region Take / Skip / ElementAt

		public static IQueryable<TSource> Take<TSource>([NotNull] this IQueryable<TSource> source, [NotNull] Expression<Func<int>> count)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (count  == null) throw new ArgumentNullException("count");

			return source.Provider.CreateQuery<TSource>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource) }),
					new[] { source.Expression, Expression.Quote(count) }));
		}

		public static IQueryable<TSource> Skip<TSource>([NotNull] this IQueryable<TSource> source, [NotNull] Expression<Func<int>> count)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (count  == null) throw new ArgumentNullException("count");

			return source.Provider.CreateQuery<TSource>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource) }),
					new[] { source.Expression, Expression.Quote(count) }));
		}

		public static TSource ElementAt<TSource>([NotNull] this IQueryable<TSource> source, [NotNull] Expression<Func<int>> index)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (index  == null) throw new ArgumentNullException("index");

			return source.Provider.Execute<TSource>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource) }),
					new[] { source.Expression, Expression.Quote(index) }));
		}

		public static TSource ElementAtOrDefault<TSource>([NotNull] this IQueryable<TSource> source, [NotNull] Expression<Func<int>> index)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (index  == null) throw new ArgumentNullException("index");

			return source.Provider.Execute<TSource>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(TSource) }),
					new[] { source.Expression, Expression.Quote(index) }));
		}

		#endregion
	}
}
