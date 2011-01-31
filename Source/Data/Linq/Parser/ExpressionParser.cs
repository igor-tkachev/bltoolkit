using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
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
			Expression         = Prepare(expression);
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

		#region Prepare

		Expression Prepare(Expression expression)
		{
			return expression.Convert(expr =>
			{
				switch (expr.NodeType)
				{
					case ExpressionType.MemberAccess :
						{
							var ma = (MemberExpression)expr;

							// Replace Count with Count()
							//
							if (ma.Member.Name == "Count")
							{
								var isList = typeof(ICollection).IsAssignableFrom(ma.Member.DeclaringType);

								if (!isList)
									foreach (var t in ma.Member.DeclaringType.GetInterfaces())
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
										.MakeGenericMethod(TypeHelper.GetElementType(ma.Expression.Type));

									return Expression.Call(null, mi, ma.Expression);
								}
							}

							/*
							// Convert members.
							//
							var lambda = ConvertMember1(ma.Member);

							if (lambda != null)
							{
								var ef  = lambda.Body.Unwrap();
								var pie = ef.Convert(wpi => wpi.NodeType == ExpressionType.Parameter ? ma.Expression : wpi);

								return Prepare(pie);
							}
							 */

							break;
						}

					case ExpressionType.Call:
						{
							/*
							var mc = (MethodCallExpression)expr;

							// Convert methods.
							//
							var lambda = ConvertMember1(mc.Method);

							if (lambda != null)
							{
								var ef    = lambda.Body.Unwrap();
								var parms = new Dictionary<string,int>(lambda.Parameters.Count);
								var pn    = mc.Method.IsStatic ? 0 : -1;

								foreach (var p in lambda.Parameters)
									parms.Add(p.Name, pn++);

								return ef.Convert(wpi =>
								{
									if (wpi.NodeType == ExpressionType.Parameter)
									{
										int n;
										if (parms.TryGetValue(((ParameterExpression)wpi).Name, out n))
											return n < 0 ? mc.Object : mc.Arguments[n];
									}

									return wpi;
								});
							}
							 */

							break;
						}

					case ExpressionType.New:
						{
							/*
							var ne = (NewExpression)expr;

							var lambda = ConvertMember1(ne.Constructor);

							if (lambda != null)
							{
								var ef    = lambda.Body.Unwrap();
								var parms = new Dictionary<string,int>(lambda.Parameters.Count);
								var pn    = 0;

								foreach (var p in lambda.Parameters)
									parms.Add(p.Name, pn++);

								return ef.Convert(wpi =>
								{
									if (wpi.NodeType == ExpressionType.Parameter)
									{
										var pe   = (ParameterExpression)wpi;
										var n    = parms[pe.Name];
										return ne.Arguments[n];
									}

									return wpi;
								});
							}
							 */

							break;
						}
				}

				return expr;
			});
		}

		#endregion
	}
}
