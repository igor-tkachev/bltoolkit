using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;
	using Data.Sql.SqlProvider;
	using Mapping;

	partial class ExpressionParser
	{
		#region Static Members

		static List<ISequenceParser> _sequenceParsers = new List<ISequenceParser>
		{
			new TableParser      (),
			new SelectParser     (),
			new WhereParser      (),
			new DistinctParser   (),
			new FirstSingleParser(),
			new AggregationParser(),
		};

		static readonly object _sync = new object();

		#endregion

		#region Init

		readonly List<ISequenceParser>             _parsers = _sequenceParsers;
		private  bool                              _reorder;
		readonly Dictionary<Expression,Expression> _expressionAccessors;
		readonly List<ParameterAccessor>           _currentSqlParameters = new List<ParameterAccessor>();

		public ExpressionParser(IDataContextInfo dataContext, Expression expression, ParameterExpression[] compiledParameters)
		{
			DataContextInfo    = dataContext;
			Expression         = expression;
			CompiledParameters = compiledParameters;

			_expressionAccessors = expression.GetExpressionAccessors(ExpressionParam);
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
		public static readonly ParameterExpression MapSchemaParam   = Expression.Parameter(typeof(MappingSchema), "ms");
		public static readonly ParameterExpression ParametersParam  = Expression.Parameter(typeof(object[]),      "ps");
		public static readonly ParameterExpression ExpressionParam  = Expression.Parameter(typeof(Expression),    "expr");

		public MappingSchema MappingSchema
		{
			get { return DataContextInfo.MappingSchema; }
		}

		#endregion

		#region Parse

		public Query<T> Parse<T>()
		{
			var sequence = ParseSequence(Expression, new SqlQuery());

			if (_reorder)
				lock (_sync)
				{
					_reorder = false;
					_sequenceParsers = _sequenceParsers.OrderByDescending(_ => _.ParsingCounter).ToList();
				}

			sequence.Root = sequence;

			var expr = sequence.BuildQuery();

			var infoParam = Expression.Parameter(typeof(Query<T>), "info");

			var mapper = Expression.Lambda<Query<T>.Mapper<T>>(
				expr, new [] { infoParam, ContextParam, DataContextParam, DataReaderParam, MapSchemaParam, ExpressionParam, ParametersParam });

			var query = new Query<T>(sequence, _currentSqlParameters);
			query.SetQuery(mapper.Compile(), mapper);

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
	}
}
