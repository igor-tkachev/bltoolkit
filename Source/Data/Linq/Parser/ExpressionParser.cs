using System;
using System.Collections.Generic;
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
		public          SqlQuery              SqlQuery = new SqlQuery();

		public MappingSchema MappingSchema
		{
			get { return DataContextInfo.MappingSchema; }
		}

		#endregion

		#region Parse

		public Query<T> Parse<T>()
		{
			var parseInfo = ParseSequence(Expression);

			if (_reorder)
				lock (_sync)
					_sequenceParsers = _sequenceParsers.OrderByDescending(_ => _.ParsingCounter).ToList();

			return parseInfo.BuildQuery<T>();
		}

		[JetBrains.Annotations.NotNull]
		public ParseInfo ParseSequence(Expression expression)
		{
			var n = _parsers[0].ParsingCounter;

			foreach (var parser in _parsers)
			{
				var info = parser.ParseSequence(this, expression);

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
