using System;
using System.Data;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
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

		protected override IParseContext ParseMethodCall(ExpressionParser parser, IParseContext parent, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var selector = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var sequence = parser.ParseSequence(parent, methodCall.Arguments[0], sqlQuery);

			sequence.SetAlias(selector.Parameters[0].Name);

			var body = selector.Body.Unwrap();

			// .Select(p => p)
			//
			//if (body == selector.Parameters[0])
			//	return sequence;

			switch (body.NodeType)
			{
				case ExpressionType.Parameter : break;
				default                       :
					sequence = CheckSubQueryForSelect(sequence);
					break;
			}

			return selector.Parameters.Count == 1 ?
				new SelectContext (parent, selector, sequence) :
				new SelectContext2(parent, selector, sequence);
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
			public SelectContext2(IParseContext parent, LambdaExpression lambda, IParseContext sequence)
				: base(parent, lambda, sequence)
			{
			}

			static readonly ParameterExpression _counterParam = Expression.Parameter(typeof(int), "counter");

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = this.BuildExpression(null, 0);

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
