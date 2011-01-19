using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;
	using Mapping;

	class ExpressionParser
	{
		#region Static Members

		static List<ISequenceParser> _sequenceParsers = new List<ISequenceParser>
		{
			new TableParser (),
			new SelectParser(),
		};

		static readonly object _sync = new object();

		#endregion

		#region Init

		readonly List<ISequenceParser> _parsers = _sequenceParsers;
		private  bool                  _reorder;

		public ExpressionParser(IDataContextInfo dataContext, Expression expression, ParameterExpression[] compiledParameters)
		{
			DataContextInfo    = dataContext;
			Expression         = expression;
			CompiledParameters = compiledParameters;
		}

		#endregion

		#region Public Members

		public readonly IDataContextInfo      DataContextInfo;
		public readonly Expression            Expression;
		public readonly ParameterExpression[] CompiledParameters;

		static readonly ParameterExpression ContextParam     = Expression.Parameter(typeof(QueryContext),  "context");
		public static readonly ParameterExpression DataContextParam = Expression.Parameter(typeof(IDataContext),  "dctx");
		public static readonly ParameterExpression DataReaderParam  = Expression.Parameter(typeof(IDataReader),   "rd");
		static readonly ParameterExpression MapSchemaParam   = Expression.Parameter(typeof(MappingSchema), "ms");
		static readonly ParameterExpression ParametersParam  = Expression.Parameter(typeof(object[]),   "ps");
		static readonly ParameterExpression ExpressionParam  = Expression.Parameter(typeof(Expression), "expr");

		public MappingSchema MappingSchema
		{
			get { return DataContextInfo.MappingSchema; }
		}

		#endregion

		#region Parse

		class RootInfo : IParseInfo
		{
			public RootInfo(ExpressionParser parser)
			{
				SqlQuery = new SqlQuery();
				Parser   = parser;
			}

			public SqlQuery         SqlQuery { get; private set; }
			public ExpressionParser Parser   { get; private set; }

			public Expression BuildExpression(IParseInfo rootParse, Expression expression, int level)
			{
				throw new InvalidOperationException();
			}

			public IEnumerable<ISqlExpression> ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				throw new InvalidOperationException();
			}

			public IEnumerable<int> ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new InvalidOperationException();
			}

			public void SetAlias(string alias)
			{
			}
		}

		public Query<T> Parse<T>()
		{
			var parseInfo = ParseSequence(new RootInfo(this), Expression);

			if (_reorder)
				lock (_sync)
					_sequenceParsers = _sequenceParsers.OrderByDescending(_ => _.ParsingCounter).ToList();

			var expr = parseInfo.BuildExpression(parseInfo, null, 0);

			var infoParam = Expression.Parameter(typeof(Query<T>), "info");

			var mapper = Expression.Lambda<Query<T>.Mapper<T>>(
				expr, new [] { infoParam, ContextParam, DataContextParam, DataReaderParam, MapSchemaParam, ExpressionParam, ParametersParam });

			var query = new Query<T>(parseInfo);
			query.SetQuery(mapper.Compile(), mapper);

			return query;
		}

		[JetBrains.Annotations.NotNull]
		public IParseInfo ParseSequence([JetBrains.Annotations.NotNull] IParseInfo parseInfo, Expression expression)
		{
			if (parseInfo == null) throw new ArgumentNullException("parseInfo");

			var n = _parsers[0].ParsingCounter;

			foreach (var parser in _parsers)
			{
				var info = parser.ParseSequence(parseInfo, expression);

				if (info == null)
				{
					n = parser.ParsingCounter;
				}
				else
				{
					lock (parser)
						parser.ParsingCounter++;

					_reorder = _reorder || n < parser.ParsingCounter;

					return info;
				}
			}

			throw new LinqException("Sequence '{0}' cannot be converted to SQL.", expression);
		}

		#endregion
	}
}
