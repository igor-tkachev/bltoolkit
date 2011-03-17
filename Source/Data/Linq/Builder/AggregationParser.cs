using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class AggregationParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			return methodCall.IsQueryable("Average", "Min", "Max", "Sum");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			var sequence = parser.ParseSequence(new ParseInfo(parseInfo, methodCall.Arguments[0]));

			if (sequence.SqlQuery.Select.IsDistinct ||
				sequence.SqlQuery.Select.TakeValue != null ||
				sequence.SqlQuery.Select.SkipValue != null)
			{
				//sequence.ConvertToIndex(null, 0, ConvertFlags.All);
				sequence = new SubQueryContext(sequence);
			}

			if (sequence.SqlQuery.OrderBy.Items.Count > 0)
			{
				if (sequence.SqlQuery.Select.TakeValue == null && sequence.SqlQuery.Select.SkipValue == null)
					sequence.SqlQuery.OrderBy.Items.Clear();
				else
					sequence = new SubQueryContext(sequence);
			}

			if (methodCall.Arguments.Count == 2)
			{
				var lambda  = (LambdaExpression)methodCall.Arguments[1].Unwrap();
				var context = new AggregationContext(parseInfo.Parent, sequence, lambda, methodCall.Method.ReturnType);
				var expr    = parser.ParseExpression(context, lambda.Body.Unwrap());

				if (expr.ElementType == QueryElementType.SqlQuery && expr != parseInfo.SqlQuery)
				{
					expr     = sequence.SqlQuery.Select.Columns[sequence.SqlQuery.Select.Add(expr)];
					sequence = new SubQueryContext(sequence);
					context  = new AggregationContext(parseInfo.Parent, sequence, lambda, methodCall.Method.ReturnType);
				}
				else
				{
					expr = parser.ConvertSearchCondition(context, expr);
				}

				context.FieldIndex = context.SqlQuery.Select.Add(
					new SqlFunction(
						methodCall.Type,
						methodCall.Method.Name,
						expr));

				return context;
			}
			else
			{
				var context = new AggregationContext(parseInfo.Parent, sequence, null, methodCall.Method.ReturnType);

				context.FieldIndex = context.SqlQuery.Select.Add(
					new SqlFunction(
						methodCall.Type,
						methodCall.Method.Name,
						sequence.ConvertToSql(null, 0, ConvertFlags.Field).Select(_ => _.Sql).ToArray()));

				return context;
			}
		}

		class AggregationContext : SequenceContextBase
		{
			public AggregationContext(IParseContext parent, IParseContext sequence, LambdaExpression lambda, Type returnType)
				: base(parent, sequence, lambda)
			{
				_returnType = returnType;
			}

			readonly Type _returnType;

			public int FieldIndex;

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = Expression.Convert(Parser.BuildSql(_returnType, FieldIndex), typeof(object));

				var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],object>>(
					expr, new []
					{
						ExpressionParser.ContextParam,
						ExpressionParser.DataContextParam,
						ExpressionParser.DataReaderParam,
						ExpressionParser.ExpressionParam,
						ExpressionParser.ParametersParam,
					});

				query.SetElementQuery(mapper.Compile());
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				return Parser.BuildSql(_returnType, FieldIndex);
			}

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.All   :
					case ConvertFlags.Key   :
					case ConvertFlags.Field : return Sequence.ConvertToSql(expression, level + 1, flags);
				}

				throw new NotImplementedException();
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.Root : return Lambda != null && expression == Lambda.Parameters[0];
				}

				return false;
			}

			public override IParseContext GetContext(Expression expression, int level, ParseInfo parseInfo)
			{
				throw new NotImplementedException();
			}
		}
	}
}
