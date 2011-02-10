using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;
	using Data.Sql.SqlProvider;
	using Mapping;
	using Reflection;

	public partial class ExpressionParser
	{
		#region Sequence

		static readonly object _sync = new object();

		static List<ISequenceParser> _sequenceParsers = new List<ISequenceParser>
		{
			new TableParser       (),
			new SelectParser      (),
			new WhereParser       (),
			new OrderByParser     (),
			new GroupByParser     (),
			new JoinParser        (),
			new DistinctParser    (),
			new FirstSingleParser (),
			new AggregationParser (),
			new ScalarSelectParser(),
			new CountParser       (),
		};

		public static void AddParser(ISequenceParser parser)
		{
			_sequenceParsers.Add(parser);
		}

		#endregion

		#region Init

		readonly List<ISequenceParser>             _parsers = _sequenceParsers;
		private  bool                              _reorder;
		readonly Dictionary<Expression,Expression> _expressionAccessors;
		readonly List<ParameterAccessor>           _currentSqlParameters = new List<ParameterAccessor>();

		public ExpressionParser(IDataContextInfo dataContext, Expression expression, ParameterExpression[] compiledParameters)
		{
			_expressionAccessors = expression.GetExpressionAccessors(ExpressionParam);

			DataContextInfo    = dataContext;
			Expression         = ConvertExpression(expression, compiledParameters);
			CompiledParameters = compiledParameters;
		}

		#endregion

		#region Public Members

		public readonly IDataContextInfo      DataContextInfo;
		public readonly Expression            Expression;
		public readonly ParameterExpression[] CompiledParameters;
		public readonly List<IParseContext>   ParentContext = new List<IParseContext>();

		public bool IsSubQueryParsing { get { return ParentContext.Count > 0; } }

		private ISqlProvider _sqlProvider;
		public  ISqlProvider  SqlProvider
		{
			get { return _sqlProvider ?? (_sqlProvider = DataContextInfo.CreateSqlProvider()); }
		}

		public static readonly ParameterExpression ContextParam     = Expression.Parameter(typeof(QueryContext),  "context");
		public static readonly ParameterExpression DataContextParam = Expression.Parameter(typeof(IDataContext),  "dctx");
		public static readonly ParameterExpression DataReaderParam  = Expression.Parameter(typeof(IDataReader),   "rd");
		//public static readonly ParameterExpression MapSchemaParam   = Expression.Parameter(typeof(MappingSchema), "ms");
		public static readonly ParameterExpression ParametersParam  = Expression.Parameter(typeof(object[]),      "ps");
		public static readonly ParameterExpression ExpressionParam  = Expression.Parameter(typeof(Expression),    "expr");

		public MappingSchema MappingSchema
		{
			get { return DataContextInfo.MappingSchema; }
		}

		#endregion

		#region Parse

		internal Query<T> Parse<T>()
		{
			var sequence = ParseSequence(Expression, new SqlQuery());

			if (_reorder)
				lock (_sync)
				{
					_reorder = false;
					_sequenceParsers = _sequenceParsers.OrderByDescending(_ => _.ParsingCounter).ToList();
				}

			var query = new Query<T>(sequence, _currentSqlParameters);
			var param = Expression.Parameter(typeof(Query<T>), "info");

			sequence.BuildQuery(query, param);

			return query;
		}

		[JetBrains.Annotations.NotNull]
		public IParseContext ParseSequence(Expression expression, SqlQuery sqlQuery)
		{
			var n = _parsers[0].ParsingCounter;

			foreach (var parser in _parsers)
			{
				if (parser.CanParse(this, expression, sqlQuery))
				{
					var sequence = parser.ParseSequence(this, expression, sqlQuery);

					lock (parser)
						parser.ParsingCounter++;

					_reorder = _reorder || n < parser.ParsingCounter;

					return sequence;
				}

				n = parser.ParsingCounter;
			}

			throw new LinqException("Sequence '{0}' cannot be converted to SQL.", expression);
		}

		#endregion

		#region ConvertExpression

		Expression ConvertExpression(Expression expression, ParameterExpression[] parameters)
		{
			expression = ConvertParameters  (expression, parameters);
			expression = ConverLetSubqueries(expression);

			return OptimizeExpression(expression);
		}

		static Expression ConvertParameters(Expression expression, ParameterExpression[] parameters)
		{
			return expression.Convert(expr =>
			{
				switch (expr.NodeType)
				{
					case ExpressionType.Parameter:
						if (parameters != null)
						{
							var idx = Array.IndexOf(parameters, (ParameterExpression)expr);

							if (idx > 0)
								return
									Expression.Convert(
										Expression.ArrayIndex(
											ParametersParam,
											Expression.Constant(Array.IndexOf(parameters, (ParameterExpression)expr))),
										expr.Type);
						}

						break;
				}

				return expr;
			});
		}

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
									var mi = typeof(Enumerable)
										.GetMethods()
										.First(m => m.Name == "Count" && m.GetParameters().Length == 1)
										.MakeGenericMethod(TypeHelper.GetElementType(me.Expression.Type));

									return Expression.Call(null, mi, me.Expression);
								}
							}
						}

						break;

					case ExpressionType.Call :
						{
							var me = (MethodCallExpression)expr;

							if (me.IsQueryable("GroupBy"))
								return ConvertGroupBy(me);

							break;
						}
				}

				return expr;
			});
		}

		#region ConvertGroupBy

		public class GroupSubQuery<TKey,TElement>
		{
			public TKey     Key;
			public TElement Element;
		}

		interface IGroupByHelper
		{
			void Set(bool wrapInSubQuery, Expression sourceExpression, LambdaExpression keySelector, LambdaExpression elementSelector, LambdaExpression resultSelector);

			Expression AddElementSelector    ();
			Expression AddResult             ();
			Expression WrapInSubQuery        ();
			Expression WrapInSubQueryResult  ();
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

			public Expression AddElementSelector()
			{
				Expression<Func<IQueryable<TSource>,TKey,TElement,TResult,IQueryable<IGrouping<TKey,TSource>>>> func = (source,key,e,r) => source
					.GroupBy(keyParam => key, _ => _)
					;

				var body   = func.Body.Unwrap();
				var keyArg = GetLambda(body, 1).Parameters[0]; // .GroupBy(keyParam

				return Convert(func, keyArg, null, null);
			}

			public Expression AddResult()
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

			public Expression WrapInSubQuery()
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

			public Expression WrapInSubQueryResult()
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

			var needSubQuery = null != keySelector.Body.Unwrap().Find(IsExpression);

			if (!needSubQuery && resultSelector == null && elementSelector != null)
				return method;

			var gtype  = typeof(GroupByHelper<,,,>).MakeGenericType(
				types["TSource"],
				types["TKey"],
				types.ContainsKey("TElement") ? types["TElement"] : types["TSource"],
				types.ContainsKey("TResult")  ? types["TResult"]  : types["TSource"]);
			var helper = (IGroupByHelper)Activator.CreateInstance(gtype);

			helper.Set(needSubQuery, sourceExpression, keySelector, elementSelector, resultSelector);

			if (!needSubQuery)
			{
				if (resultSelector == null)
					return helper.AddElementSelector();
				return helper.AddResult();
			}

			if (resultSelector == null)
				return helper.WrapInSubQuery();

			return helper.WrapInSubQueryResult();
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
						var ma = (MemberExpression)ex;
						var l  = ConvertMember(ma.Member);

						if (l != null)
						{
							var ef  = l.Body.Unwrap();
							var pie = ef.Convert(wpi => wpi.NodeType == ExpressionType.Parameter ? ma.Expression : wpi);

							return pie.Find(IsExpression) != null;
						}

						var attr = GetFunctionAttribute(ma.Member);

						if (attr != null)
							return true;

						if (ma.Member.DeclaringType == typeof(TimeSpan))
						{
							switch (ma.Expression.NodeType)
							{
								case ExpressionType.Subtract        :
								case ExpressionType.SubtractChecked : return true;
							}
						}

						return false;
					}
			}

			return true;
		}

		#endregion

		#endregion
	}
}
