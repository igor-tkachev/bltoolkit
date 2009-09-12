using System;
using System.Linq.Expressions;

namespace BLToolkit.Data
{
	using Linq;

	public partial class DbManager
	{
		public Table<T> GetTable<T>()
			where T : class
		{
			return new Table<T>(this);
		}

		public T Select<T>(Expression<Func<T>> selector)
		{
			if (selector == null) throw new ArgumentNullException("selector");

			var q = new Table<T>(this, selector);

			foreach (var item in q)
				return item;

			throw new InvalidOperationException();
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
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		public Func<TDb,TResult> Compile<TDb,TResult>(
			[JetBrains.Annotations.NotNull] Expression<Func<TDb,TResult>> query)
			where TDb : DbManager
		{
			return CompiledQuery.Compile(query);
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
		public Func<TDb,TArg1,TResult> Compile<TDb,TArg1, TResult>(
			[JetBrains.Annotations.NotNull] Expression<Func<TDb,TArg1,TResult>> query)
			where TDb : DbManager
		{
			return CompiledQuery.Compile(query);
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
		public Func<TDb,TArg1,TArg2,TResult> Compile<TDb,TArg1,TArg2,TResult>(
			[JetBrains.Annotations.NotNull] Expression<Func<TDb,TArg1,TArg2,TResult>> query)
			where TDb : DbManager
		{
			return CompiledQuery.Compile(query);
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
		public Func<TDb,TArg1,TArg2,TArg3,TResult> Compile<TDb,TArg1,TArg2,TArg3,TResult>(
			[JetBrains.Annotations.NotNull] Expression<Func<TDb,TArg1,TArg2,TArg3,TResult>> query)
			where TDb : DbManager
		{
			return CompiledQuery.Compile(query);
		}
	}
}
