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

			if (sequence.SqlQuery.Select.IsDistinct)
				sequence = new SubQueryContext(sequence);

			return selector.Parameters.Count == 1 ? new SelectContext (selector, sequence) : new SelectContext2(selector, sequence);
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

				var mapper = Expression.Lambda<Func<int,IDataContext,IDataReader,Expression,object[],T>>(
					expr, new []
					{
						_counterParam,
						ExpressionParser.DataContextParam,
						ExpressionParser.DataReaderParam,
						ExpressionParser.ExpressionParam,
						ExpressionParser.ParametersParam,
					});

				var func    = mapper.Compile();
				var counter = 0;

				Func<IDataContext,IDataReader,Expression,object[],T> map = (ctx,rd,e,ps) => func(counter++, ctx, rd, e, ps);

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
