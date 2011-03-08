using System;
using System.Data;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class ScalarSelectParser : ISequenceParser
	{
		public int ParsingCounter { get; set; }

		public bool CanParse(ExpressionParser parser, ParseInfo parseInfo)
		{
			return
				parseInfo.Expression.NodeType == ExpressionType.Lambda &&
				((LambdaExpression)parseInfo.Expression).Parameters.Count == 0;
		}

		public IParseContext ParseSequence(ExpressionParser parser, ParseInfo parseInfo)
		{
			return new ScalarSelectContext
			{
				Parser     = parser,
				Parent     = parseInfo.Parent,
				Expression = parseInfo.Expression,
				SqlQuery   = parseInfo.SqlQuery
			};
		}

		class ScalarSelectContext : IParseContext
		{
#if DEBUG
			public string _sqlQueryText { get { return SqlQuery == null ? "" : SqlQuery.SqlText; } }
#endif

			public ExpressionParser Parser     { get; set; }
			public Expression       Expression { get; set; }
			public SqlQuery         SqlQuery   { get; set; }
			public IParseContext    Parent     { get; set; }

			public void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = BuildExpression(null, 0);

				var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],T>>(
					expr, new []
					{
						ExpressionParser.ContextParam,
						ExpressionParser.DataContextParam,
						ExpressionParser.DataReaderParam,
						ExpressionParser.ExpressionParam,
						ExpressionParser.ParametersParam,
					});

				query.SetQuery(mapper.Compile());
			}

			public Expression BuildExpression(Expression expression, int level)
			{
				if (expression == null)
					expression = ((LambdaExpression)Expression).Body.Unwrap();

				switch (expression.NodeType)
				{
					case ExpressionType.New:
					case ExpressionType.MemberInit:
						{
							var expr = Parser.BuildExpression(this, expression);

							if (SqlQuery.Select.Columns.Count == 0)
								SqlQuery.Select.Expr(new SqlValue(1));

							return expr;
						}

					default :
						{
							var expr = Parser.ParseExpression(this, expression);
							var idx  = SqlQuery.Select.Add(expr);

							return Parser.BuildSql(expression.Type, idx);
						}
				}

			}

			public SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.Expression : return true;
					default                    : return false;
				}
			}

			public IParseContext GetContext(Expression expression, int level, ParseInfo parseInfo)
			{
				throw new NotImplementedException();
			}

			public int ConvertToParentIndex(int index, IParseContext context)
			{
				throw new NotImplementedException();
			}

			public void SetAlias(string alias)
			{
			}
		}
	}
}
