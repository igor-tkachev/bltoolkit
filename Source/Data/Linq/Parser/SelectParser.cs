using System;
using System.Data;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	class SelectParser : MethodCallParser
	{
		#region SelectParser

		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			if (methodCall.IsQueryable("Select"))
			{
				switch (((LambdaExpression)methodCall.Arguments[1].Unwrap()).Parameters.Count)
				{
					case 1 :
					case 2 : return true;
					default: break;
				}
			}

			return false;
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var selector = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var sequence = parser.ParseSequence(methodCall.Arguments[0], sqlQuery);

			sequence.SetAlias(selector.Parameters[0].Name);

			var body = selector.Body.Unwrap();

			// .Select(p => p)
			//
			//if (body == selector.Parameters[0])
			//	return sequence;

			switch (body.NodeType)
			{
				case ExpressionType.Parameter  :
					// .Select(p => p)
					//
					//if (body == selector.Parameters[0])
					//	return sequence;

					//foreach (var parent in parser.ParentContext)
					//	if (parent.IsExpression(body, 0, RequestFor.Root))
					//		return parent;

					break;
					//throw new InvalidOperationException();

				case ExpressionType.MemberAccess:
					{
						sequence = CheckSubQueryForSelect(sequence);

						/*
						var src = GetSource(l, l.Body, sources);

						if (src != null)
						{
							src = _convertSource(src, l);

							if (CurrentSql.From.Tables.Count == 0)
							{
								var table = src as QuerySource.Table;

								if (table != null)
								{
									while (table.ParentAssociation != null)
										table = table.ParentAssociation;

									CurrentSql = table.SqlQuery;
								}
							}

							if (src.Lambda == null && src is QuerySource.SubQuerySourceColumn)
								src = new QuerySource.Expr(CurrentSql, l, Concat(sources, ParentQueries));

							return src;
						}
						*/
					}

					goto default;

				case ExpressionType.New        :
					{
						sequence = CheckSubQueryForSelect(sequence);

						/*
						if (_sequenceNumber > 1)
						{
							var pie = ConvertNew((NewExpression)l.Body);
							if (pie != null)
								return ParseSelect(new LambdaInfo(pie, l.Parameters), sources)[0];
						}

						return new QuerySource.Expr(CurrentSql, l, Concat(sources, ParentQueries));
						*/

						break;
					}

				case ExpressionType.MemberInit :
					{
						sequence = CheckSubQueryForSelect(sequence);
						//return new QuerySource.Expr(CurrentSql, l, Concat(sources, ParentQueries));
						break;
					}

				default                        :
					{
						sequence = CheckSubQueryForSelect(sequence);
						//var scalar = new QuerySource.Scalar(CurrentSql, l, Concat(sources, ParentQueries));
						//return scalar.Fields[0] is QuerySource ? (QuerySource)scalar.Fields[0] : scalar;
						break;
					}
			}

			return selector.Parameters.Count == 1 ? new SelectContext (selector, sequence) : new SelectContext2(selector, sequence);
		}

		static IParseContext CheckSubQueryForSelect(IParseContext context)
		{
			if (/*_parsingMethod[0] != ParsingMethod.OrderBy &&*/ context.SqlQuery.Select.IsDistinct)
				return new SubQueryContext(context);

			return context;
		}

		#endregion

		#region SelectContext2

		class SelectContext2 : SelectContext
		{
			public SelectContext2(LambdaExpression lambda, IParseContext sequence)
				: base(lambda, sequence)
			{
			}

			static readonly ParameterExpression _counterParam = Expression.Parameter(typeof(int), "counter");

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = BuildExpression(null, 0);

				var mapper = Expression.Lambda<Func<int,QueryContext,IDataContext,IDataReader,Expression,object[],T>>(
					expr, new []
					{
						_counterParam,
						ExpressionParser.ContextParam,
						ExpressionParser.DataContextParam,
						ExpressionParser.DataReaderParam,
						ExpressionParser.ExpressionParam,
						ExpressionParser.ParametersParam,
					});

				var func    = mapper.Compile();
				var counter = 0;

				Func<QueryContext,IDataContext,IDataReader,Expression,object[],T> map = (ctx,db,rd,e,ps) => func(counter++, ctx, db, rd, e, ps);

				query.SetQuery(map);
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.Expression :
					case RequestFor.Root       :
						if (expression == Lambda.Parameters[1])
							return true;
						break;
				}

				return base.IsExpression(expression, level, requestFlag);
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				if (expression == Lambda.Parameters[1])
					return _counterParam;

				return base.BuildExpression(expression, level);
			}
		}

		#endregion
	}
}
