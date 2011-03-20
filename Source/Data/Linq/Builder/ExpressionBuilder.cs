using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;
	using Data.Sql.SqlProvider;
	using Mapping;
	using Reflection;

	public partial class ExpressionBuilder
	{
		#region Sequence

		static readonly object _sync = new object();

		static List<ISequenceBuilder> _sequenceBuilders = new List<ISequenceBuilder>
		{
			new TableBuilder         (),
			new SelectBuilder        (),
			new SelectManyBuilder    (),
			new WhereBuilder         (),
			new OrderByBuilder       (),
			new GroupByBuilder       (),
			new JoinBuilder          (),
			new TakeSkipBuilder      (),
			new DefaultIfEmptyBuilder(),
			new DistinctBuilder      (),
			new FirstSingleBuilder   (),
			new AggregationBuilder   (),
			new ScalarSelectBuilder  (),
			new CountBuilder         (),
			new AsQueryableBuilder   (),
			new TableAttributeBuilder(),
			new InsertBuilder        (),
			new InsertBuilder.Into   (),
			new InsertBuilder.Value  (),
			new UpdateBuilder        (),
			new UpdateBuilder.Set    (),
			new DeleteBuilder        (),
		};

		public static void AddBuilder(ISequenceBuilder builder)
		{
			_sequenceBuilders.Add(builder);
		}

		#endregion

		#region Init

		readonly Query                             _query;
		readonly List<ISequenceBuilder>            _builders = _sequenceBuilders;
		private  bool                              _reorder;
		readonly Dictionary<Expression,Expression> _expressionAccessors;

		readonly public List<ParameterAccessor>    CurrentSqlParameters = new List<ParameterAccessor>();

		public ExpressionBuilder(
			Query                 query,
			IDataContextInfo      dataContext,
			Expression            expression,
			ParameterExpression[] compiledParameters)
		{
			_query               = query;
			_expressionAccessors = expression.GetExpressionAccessors(ExpressionParam);

			CompiledParameters = compiledParameters;
			DataContextInfo    = dataContext;
			Expression         = ConvertExpressionTree(expression);
		}

		#endregion

		#region Public Members

		public readonly IDataContextInfo      DataContextInfo;
		public readonly Expression            Expression;
		public readonly ParameterExpression[] CompiledParameters;

		private ISqlProvider _sqlProvider;
		public  ISqlProvider  SqlProvider
		{
			get { return _sqlProvider ?? (_sqlProvider = DataContextInfo.CreateSqlProvider()); }
		}

		public static readonly ParameterExpression ContextParam     = Expression.Parameter(typeof(QueryContext), "context");
		public static readonly ParameterExpression DataContextParam = Expression.Parameter(typeof(IDataContext), "dctx");
		public static readonly ParameterExpression DataReaderParam  = Expression.Parameter(typeof(IDataReader),  "rd");
		public static readonly ParameterExpression ParametersParam  = Expression.Parameter(typeof(object[]),     "ps");
		public static readonly ParameterExpression ExpressionParam  = Expression.Parameter(typeof(Expression),   "expr");

		public MappingSchema MappingSchema
		{
			get { return DataContextInfo.MappingSchema; }
		}

		#endregion

		#region Builder SQL

		internal Query<T> Build<T>()
		{
			var sequence = BuildSequence(new BuildInfo((IBuildContext)null, Expression, new SqlQuery()));

			if (_reorder)
				lock (_sync)
				{
					_reorder = false;
					_sequenceBuilders = _sequenceBuilders.OrderByDescending(_ => _.BuildCounter).ToList();
				}

			_query.Init(sequence, CurrentSqlParameters);

			var param = Expression.Parameter(typeof(Query<T>), "info");

			sequence.BuildQuery((Query<T>)_query, param);

			return (Query<T>)_query;
		}

		[JetBrains.Annotations.NotNull]
		public IBuildContext BuildSequence(BuildInfo buildInfo)
		{
			buildInfo.Expression = buildInfo.Expression.Unwrap();

			var n = _builders[0].BuildCounter;

			foreach (var builder in _builders)
			{
				if (builder.CanBuild(this, buildInfo))
				{
					var sequence = builder.BuildSequence(this, buildInfo);

					lock (builder)
						builder.BuildCounter++;

					_reorder = _reorder || n < builder.BuildCounter;

					return sequence;
				}

				n = builder.BuildCounter;
			}

			throw new LinqException("Sequence '{0}' cannot be converted to SQL.", buildInfo.Expression);
		}

		#endregion

		#region ConvertExpression

		Expression ConvertExpressionTree(Expression expression)
		{
			expression = ConvertParameters  (expression);
			expression = ConverLetSubqueries(expression);

			return OptimizeExpression(expression);
		}

		#region ConvertParameters

		Expression ConvertParameters(Expression expression)
		{
			return expression.Convert(expr =>
			{
				switch (expr.NodeType)
				{
					case ExpressionType.Parameter:
						if (CompiledParameters != null)
						{
							var idx = Array.IndexOf(CompiledParameters, (ParameterExpression)expr);

							if (idx > 0)
								return
									Expression.Convert(
										Expression.ArrayIndex(
											ParametersParam,
											Expression.Constant(Array.IndexOf(CompiledParameters, (ParameterExpression)expr))),
										expr.Type);
						}

						break;
				}

				return expr;
			});
		}

		#endregion

		#region ConverLetSubqueries

		static Expression ConverLetSubqueries(Expression expression)
		{
			var result = expression;

			do
			{
				expression = result;

				// Find let subqueries.
				//
				var dic = new Dictionary<MemberInfo,Expression>();

				expression.Visit(ex =>
				{
					switch (ex.NodeType)
					{
						case ExpressionType.Call:
							{
								var me = (MethodCallExpression)ex;

								LambdaInfo lambda = null;

								if (me.Method.Name == "Select" &&
								    (me.IsQueryableMethod((_, l) => { lambda = l; return true; }) ||
								     me.IsQueryableMethod(null, 2, _ => { }, l => lambda = l)))
								{
									lambda.Body.Visit(e =>
									{
										switch (e.NodeType)
										{
											case ExpressionType.New:
												{
													var ne = (NewExpression)e;

													if (ne.Members == null || ne.Arguments.Count != ne.Members.Count)
														break;

													var args = ne.Arguments.Zip(ne.Members, (a,m) => new { a, m }).ToList();

													var q =
														from a in args
														where
															a.a.NodeType == ExpressionType.Call &&
															a.a.Type != typeof(string) &&
															!a.a.Type.IsArray &&
															TypeHelper.GetGenericType(typeof(IEnumerable<>), a.a.Type) != null
														select a;

													foreach (var item in q)
														dic.Add(item.m, item.a);
												}

												break;
										}
									});
								}
							}

							break;
					}
				});

				if (dic.Count == 0)
					return expression;

				result = expression.Convert(ex =>
				{
					switch (ex.NodeType)
					{
						case ExpressionType.MemberAccess:
							{
								var me     = (MemberExpression)ex;
								var member = me.Member;

								if (member is PropertyInfo)
									member = ((PropertyInfo)member).GetGetMethod();

								Expression arg;

								if (dic.TryGetValue(member, out arg))
									return arg;
							}

							break;
					}

					return ex;
				});
			} while (result != expression);

			return expression;
		}

		#endregion

		#region OptimizeExpression

		private MethodInfo[] _enumerableMethods;
		public  MethodInfo[]  EnumerableMethods
		{
			get { return _enumerableMethods ?? (_enumerableMethods = typeof(Enumerable).GetMethods()); }
		}

		private MethodInfo[] _queryableMethods;
		public  MethodInfo[]  QueryableMethods
		{
			get { return _queryableMethods ?? (_queryableMethods = typeof(Queryable).GetMethods()); }
		}

		Expression OptimizeExpression(Expression expression)
		{
			return expression.Convert(expr =>
			{
				switch (expr.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var me = (MemberExpression)expr;

							// Replace Count with Count()
							//
							if (me.Member.Name == "Count")
							{
								var isList = typeof(ICollection).IsAssignableFrom(me.Member.DeclaringType);

								if (!isList)
									foreach (var t in me.Member.DeclaringType.GetInterfaces())
										if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>))
										{
											isList = true;
											break;
										}

								if (isList)
								{
									var mi = EnumerableMethods
										.First(m => m.Name == "Count" && m.GetParameters().Length == 1)
										.MakeGenericMethod(TypeHelper.GetElementType(me.Expression.Type));

									return Expression.Call(null, mi, me.Expression);
								}
							}

							if (CompiledParameters == null && TypeHelper.IsSameOrParent(typeof(IQueryable), expr.Type))
							{
								var ex = ConvertIQueriable(expr);

								if (ex != expr)
									return ConvertExpressionTree(ex);
							}
						}

						break;

					case ExpressionType.Call :
						{
							var me = (MethodCallExpression)expr;

							if (me.IsQueryable()) switch (me.Method.Name)
							{
								case "GroupBy"    : return ConvertGroupBy   (me);
								case "SelectMany" : return ConvertSelectMany(me);
								case "LongCount"  :
								case "Count"      : return ConvertCount     (me);
							}

							break;
						}
				}

				return expr;
			});
		}

		#endregion

		#region ConvertGroupBy

		public class GroupSubQuery<TKey,TElement>
		{
			public TKey     Key;
			public TElement Element;
		}

		interface IGroupByHelper
		{
			void Set(bool wrapInSubQuery, Expression sourceExpression, LambdaExpression keySelector, LambdaExpression elementSelector, LambdaExpression resultSelector);

			Expression AddElementSelectorQ  ();
			Expression AddElementSelectorE  ();
			Expression AddResultQ           ();
			Expression AddResultE           ();
			Expression WrapInSubQueryQ      ();
			Expression WrapInSubQueryE      ();
			Expression WrapInSubQueryResultQ();
			Expression WrapInSubQueryResultE();
		}

		class GroupByHelper<TSource,TKey,TElement,TResult> : IGroupByHelper
		{
			bool             _wrapInSubQuery;
			Expression       _sourceExpression;
			LambdaExpression _keySelector;
			LambdaExpression _elementSelector;
			LambdaExpression _resultSelector;

			public void Set(
				bool             wrapInSubQuery,
				Expression       sourceExpression,
				LambdaExpression keySelector,
				LambdaExpression elementSelector,
				LambdaExpression resultSelector)
			{
				_wrapInSubQuery   = wrapInSubQuery;
				_sourceExpression = sourceExpression;
				_keySelector      = keySelector;
				_elementSelector  = elementSelector;
				_resultSelector   = resultSelector;
			}

			public Expression AddElementSelectorQ()
			{
				Expression<Func<IQueryable<TSource>,TKey,TElement,TResult,IQueryable<IGrouping<TKey,TSource>>>> func = (source,key,e,r) => source
					.GroupBy(keyParam => key, _ => _)
					;

				var body   = func.Body.Unwrap();
				var keyArg = GetLambda(body, 1).Parameters[0]; // .GroupBy(keyParam

				return Convert(func, keyArg, null, null);
			}

			public Expression AddElementSelectorE()
			{
				Expression<Func<IEnumerable<TSource>,TKey,TElement,TResult,IEnumerable<IGrouping<TKey,TSource>>>> func = (source,key,e,r) => source
					.GroupBy(keyParam => key, _ => _)
					;

				var body   = func.Body.Unwrap();
				var keyArg = GetLambda(body, 1).Parameters[0]; // .GroupBy(keyParam

				return Convert(func, keyArg, null, null);
			}

			public Expression AddResultQ()
			{
				Expression<Func<IQueryable<TSource>,TKey,TElement,TResult,IQueryable<TResult>>> func = (source,key,e,r) => source
					.GroupBy(keyParam => key, elemParam => e)
					.Select (resParam => r)
					;

				var body    = func.Body.Unwrap();
				var keyArg  = GetLambda(body, 0, 1).Parameters[0]; // .GroupBy(keyParam
				var elemArg = GetLambda(body, 0, 2).Parameters[0]; // .GroupBy(..., elemParam
				var resArg  = GetLambda(body, 1).   Parameters[0]; // .Select (resParam

				return Convert(func, keyArg, elemArg, resArg);
			}

			public Expression AddResultE()
			{
				Expression<Func<IEnumerable<TSource>,TKey,TElement,TResult,IEnumerable<TResult>>> func = (source,key,e,r) => source
					.GroupBy(keyParam => key, elemParam => e)
					.Select (resParam => r)
					;

				var body    = func.Body.Unwrap();
				var keyArg  = GetLambda(body, 0, 1).Parameters[0]; // .GroupBy(keyParam
				var elemArg = GetLambda(body, 0, 2).Parameters[0]; // .GroupBy(..., elemParam
				var resArg  = GetLambda(body, 1).   Parameters[0]; // .Select (resParam

				return Convert(func, keyArg, elemArg, resArg);
			}

			public Expression WrapInSubQueryQ()
			{
				Expression<Func<IQueryable<TSource>,TKey,TElement,TResult,IQueryable<IGrouping<TKey,TElement>>>> func = (source,key,e,r) => source
					.Select(selectParam => new GroupSubQuery<TKey,TSource>
					{
						Key     = key,
						Element = selectParam
					})
					.GroupBy(_ => _.Key, elemParam => e)
					;

				var body    = func.Body.Unwrap();
				var keyArg  = GetLambda(body, 0, 1).Parameters[0]; // .Select (selectParam
				var elemArg = GetLambda(body, 2).   Parameters[0]; // .GroupBy(..., elemParam

				return Convert(func, keyArg, elemArg, null);
			}

			public Expression WrapInSubQueryE()
			{
				Expression<Func<IEnumerable<TSource>,TKey,TElement,TResult,IEnumerable<IGrouping<TKey,TElement>>>> func = (source,key,e,r) => source
					.Select(selectParam => new GroupSubQuery<TKey,TSource>
					{
						Key     = key,
						Element = selectParam
					})
					.GroupBy(_ => _.Key, elemParam => e)
					;

				var body    = func.Body.Unwrap();
				var keyArg  = GetLambda(body, 0, 1).Parameters[0]; // .Select (selectParam
				var elemArg = GetLambda(body, 2).   Parameters[0]; // .GroupBy(..., elemParam

				return Convert(func, keyArg, elemArg, null);
			}

			public Expression WrapInSubQueryResultQ()
			{
				Expression<Func<IQueryable<TSource>,TKey,TElement,TResult,IQueryable<TResult>>> func = (source,key,e,r) => source
					.Select(selectParam => new GroupSubQuery<TKey,TSource>
					{
						Key     = key,
						Element = selectParam
					})
					.GroupBy(_ => _.Key, elemParam => e)
					.Select (resParam => r)
					;

				var body    = func.Body.Unwrap();
				var keyArg  = GetLambda(body, 0, 0, 1).Parameters[0]; // .Select (selectParam
				var elemArg = GetLambda(body, 0, 2).   Parameters[0]; // .GroupBy(..., elemParam
				var resArg  = GetLambda(body, 1).      Parameters[0]; // .Select (resParam

				return Convert(func, keyArg, elemArg, resArg);
			}

			public Expression WrapInSubQueryResultE()
			{
				Expression<Func<IEnumerable<TSource>,TKey,TElement,TResult,IEnumerable<TResult>>> func = (source,key,e,r) => source
					.Select(selectParam => new GroupSubQuery<TKey,TSource>
					{
						Key     = key,
						Element = selectParam
					})
					.GroupBy(_ => _.Key, elemParam => e)
					.Select (resParam => r)
					;

				var body    = func.Body.Unwrap();
				var keyArg  = GetLambda(body, 0, 0, 1).Parameters[0]; // .Select (selectParam
				var elemArg = GetLambda(body, 0, 2).   Parameters[0]; // .GroupBy(..., elemParam
				var resArg  = GetLambda(body, 1).      Parameters[0]; // .Select (resParam

				return Convert(func, keyArg, elemArg, resArg);
			}

			Expression Convert(
				LambdaExpression    func,
				ParameterExpression keyArg,
				ParameterExpression elemArg,
				ParameterExpression resArg)
			{
				var body = func.Body.Unwrap();
				var expr = body.Convert(ex =>
				{
					if (ex == func.Parameters[0])
						return _sourceExpression;

					if (ex == func.Parameters[1])
						return _keySelector.Body.Convert(e => e == _keySelector.Parameters[0] ? keyArg : e);

					if (ex == func.Parameters[2])
					{
						Expression obj = elemArg;

						if (_wrapInSubQuery)
							obj = Expression.PropertyOrField(elemArg, "Element");

						if (_elementSelector == null)
							return obj;

						return _elementSelector.Body.Convert(e => e == _elementSelector.Parameters[0] ? obj : e);
					}

					if (ex == func.Parameters[3])
						return _resultSelector.Body.Convert(e =>
						{
							if (e == _resultSelector.Parameters[0])
								return Expression.PropertyOrField(resArg, "Key");

							if (e == _resultSelector.Parameters[1])
								return resArg;

							return e;
						});

					return ex;
				});

				return expr;
			}
		}

		static LambdaExpression GetLambda(Expression expression, params int[] n)
		{
			foreach (var i in n)
				expression = ((MethodCallExpression)expression).Arguments[i].Unwrap();
			return (LambdaExpression)expression;
		}

		Expression ConvertGroupBy(MethodCallExpression method)
		{
			if (method.Arguments[method.Arguments.Count - 1].Unwrap().NodeType != ExpressionType.Lambda)
				return method;

			var types = method.Method.GetGenericMethodDefinition().GetGenericArguments()
				.Zip(method.Method.GetGenericArguments(), (n, t) => new { n = n.Name, t })
				.ToDictionary(_ => _.n, _ => _.t);

			var sourceExpression = OptimizeExpression(method.Arguments[0].Unwrap());
			var keySelector      = (LambdaExpression)OptimizeExpression(method.Arguments[1].Unwrap());
			var elementSelector  = types.ContainsKey("TElement") ? (LambdaExpression)OptimizeExpression(method.Arguments[2].Unwrap()) : null;
			var resultSelector   = types.ContainsKey("TResult")  ?
				(LambdaExpression)OptimizeExpression(method.Arguments[types.ContainsKey("TElement") ? 3 : 2].Unwrap()) : null;

			var needSubQuery = null != ConvertExpression(keySelector.Body.Unwrap()).Find(IsExpression);

			if (!needSubQuery && resultSelector == null && elementSelector != null)
				return method;

			var gtype  = typeof(GroupByHelper<,,,>).MakeGenericType(
				types["TSource"],
				types["TKey"],
				types.ContainsKey("TElement") ? types["TElement"] : types["TSource"],
				types.ContainsKey("TResult")  ? types["TResult"]  : types["TSource"]);
			var helper = (IGroupByHelper)Activator.CreateInstance(gtype);

			helper.Set(needSubQuery, sourceExpression, keySelector, elementSelector, resultSelector);

			if (method.Method.DeclaringType == typeof(Queryable))
			{
				if (!needSubQuery)
					return resultSelector == null ? helper.AddElementSelectorQ() : helper.AddResultQ();

				return resultSelector == null ? helper.WrapInSubQueryQ() : helper.WrapInSubQueryResultQ();
			}
			else
			{
				if (!needSubQuery)
					return resultSelector == null ? helper.AddElementSelectorE() : helper.AddResultE();

				return resultSelector == null ? helper.WrapInSubQueryE() : helper.WrapInSubQueryResultE();
			}
		}

		bool IsExpression(Expression ex)
		{
			switch (ex.NodeType)
			{
				case ExpressionType.Convert        :
				case ExpressionType.ConvertChecked :
				case ExpressionType.MemberInit     :
				case ExpressionType.New            :
				case ExpressionType.NewArrayBounds :
				case ExpressionType.NewArrayInit   :
				case ExpressionType.Parameter      : return false;
				case ExpressionType.MemberAccess   :
					{
						var ma   = (MemberExpression)ex;
						var attr = GetFunctionAttribute(ma.Member);

						if (attr != null)
							return true;

						return false;
					}
			}

			return true;
		}

		#endregion

		#region ConvertSelectMany

		interface ISelectManyHelper
		{
			void Set(Expression sourceExpression, LambdaExpression colSelector);

			Expression AddElementSelectorQ();
			Expression AddElementSelectorE();
		}

		class SelectManyHelper<TSource,TCollection> : ISelectManyHelper
		{
			Expression       _sourceExpression;
			LambdaExpression _colSelector;

			public void Set(Expression sourceExpression, LambdaExpression colSelector)
			{
				_sourceExpression = sourceExpression;
				_colSelector      = colSelector;
			}

			public Expression AddElementSelectorQ()
			{
				Expression<Func<IQueryable<TSource>,IEnumerable<TCollection>,IQueryable<TCollection>>> func = (source,col) => source
					.SelectMany(colParam => col, (s,c) => c)
					;

				var body   = func.Body.Unwrap();
				var colArg = GetLambda(body, 1).Parameters[0]; // .SelectMany(colParam

				return Convert(func, colArg);
			}

			public Expression AddElementSelectorE()
			{
				Expression<Func<IEnumerable<TSource>,IEnumerable<TCollection>,IEnumerable<TCollection>>> func = (source,col) => source
					.SelectMany(colParam => col, (s,c) => c)
					;

				var body   = func.Body.Unwrap();
				var colArg = GetLambda(body, 1).Parameters[0]; // .SelectMany(colParam

				return Convert(func, colArg);
			}

			Expression Convert(LambdaExpression func, ParameterExpression colArg)
			{
				var body = func.Body.Unwrap();
				var expr = body.Convert(ex =>
				{
					if (ex == func.Parameters[0])
						return _sourceExpression;

					if (ex == func.Parameters[1])
						return _colSelector.Body.Convert(e => e == _colSelector.Parameters[0] ? colArg : e);

					return ex;
				});

				return expr;
			}
		}

		Expression ConvertSelectMany(MethodCallExpression method)
		{
			if (method.Arguments.Count != 2 || ((LambdaExpression)method.Arguments[1].Unwrap()).Parameters.Count != 1)
				return method;

			var types = method.Method.GetGenericMethodDefinition().GetGenericArguments()
				.Zip(method.Method.GetGenericArguments(), (n, t) => new { n = n.Name, t })
				.ToDictionary(_ => _.n, _ => _.t);

			var sourceExpression = OptimizeExpression(method.Arguments[0].Unwrap());
			var colSelector      = (LambdaExpression)OptimizeExpression(method.Arguments[1].Unwrap());

			var gtype  = typeof(SelectManyHelper<,>).MakeGenericType(types["TSource"], types["TResult"]);
			var helper = (ISelectManyHelper)Activator.CreateInstance(gtype);

			helper.Set(sourceExpression, colSelector);

			return method.Method.DeclaringType == typeof(Queryable) ?
				helper.AddElementSelectorQ() :
				helper.AddElementSelectorE();
		}

		#endregion

		#region ConvertCount

		Expression ConvertCount(MethodCallExpression method)
		{
			if (method.Arguments.Count != 2)
				return method;

			MethodInfo wm;
			MethodInfo cm;

			if (method.Method.DeclaringType == typeof(Enumerable))
			{
				wm = EnumerableMethods
					.Where(m => m.Name == "Where" && m.GetParameters().Length == 2)
					.First(m => m.GetParameters()[1].ParameterType.GetGenericArguments().Length == 2);

				cm = EnumerableMethods
					.First(m => m.Name == method.Method.Name && m.GetParameters().Length == 1);
			}
			else
			{
				wm = QueryableMethods
					.Where(m => m.Name == "Where" && m.GetParameters().Length == 2)
					.First(m => m.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments().Length == 2);

				cm = QueryableMethods
					.First(m => m.Name == method.Method.Name && m.GetParameters().Length == 1);
			}

			var argType = method.Method.GetGenericArguments()[0];

			wm = wm.MakeGenericMethod(argType);
			cm = cm.MakeGenericMethod(argType);

			return Expression.Call(null, cm, Expression.Call(null, wm,
				OptimizeExpression(method.Arguments[0]),
				OptimizeExpression(method.Arguments[1])));
		}

		#endregion

		#region ConvertIQueriable

		Expression ConvertIQueriable(Expression expression)
		{
			if (expression.NodeType == ExpressionType.MemberAccess || expression.NodeType == ExpressionType.Call)
			{
				var p    = Expression.Parameter(typeof(Expression), "exp");
				var exas = expression.GetExpressionAccessors(p);
				var expr = ReplaceParameter(exas, expression, _ => {});
				var l    = Expression.Lambda<Func<Expression,IQueryable>>(Expression.Convert(expr, typeof(IQueryable)), new [] { p });
				var qe   = l.Compile();
				var n    = _query.AddQueryableAccessors(expression, qe);

				Expression accessor;

				_expressionAccessors.TryGetValue(expression, out accessor);

				var path =
					Expression.Call(
						Expression.Constant(_query),
						ReflectionHelper.Expressor<Query>.MethodExpressor(a => a.GetIQueryable(0, null)),
						new[] { Expression.Constant(n), accessor ?? Expression.Constant(null) });

				var qex  = qe(expression).Expression;

				foreach (var a in qex.GetExpressionAccessors(path))
					if (!_expressionAccessors.ContainsKey(a.Key))
						_expressionAccessors.Add(a.Key, a.Value);

				return qex;
			}

			throw new InvalidOperationException();
		}

		#endregion

		#endregion
	}
}
