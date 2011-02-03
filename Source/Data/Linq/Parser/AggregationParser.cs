using System;
using System.Data;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	class AggregationParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			if (!methodCall.IsQueryable())
				return false;

			switch (methodCall.Method.Name)
			{
				case "Average" :
				case "Min"     :
				case "Max"     :
				case "Sum"     : return true;
				default        : return false;
			}
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var sequence = parser.ParseSequence(methodCall.Arguments[0], sqlQuery);

			if (sequence.SqlQuery.Select.IsDistinct || sequence.SqlQuery.Select.TakeValue != null || sequence.SqlQuery.Select.SkipValue != null)
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
				var context = new AggregationContext(sequence, lambda, methodCall.Method.ReturnType);

				context.FieldIndex = context.SqlQuery.Select.Add(
					new SqlFunction(
						methodCall.Type,
						methodCall.Method.Name,
						parser.ParseExpression(context, context.Lambda.Body.Unwrap())));

				return context;
			}
			else
			{
				var context = new AggregationContext(sequence, null, methodCall.Method.ReturnType);

				context.FieldIndex = context.SqlQuery.Select.Add(
					new SqlFunction(
						methodCall.Type,
						methodCall.Method.Name,
						sequence.ConvertToSql(null, 0, ConvertFlags.Field)));

				return context;
			}
		}

		class AggregationContext : SequenceContextBase
		{
			public AggregationContext(IParseContext sequence, LambdaExpression lambda, Type returnType)
				: base(sequence, lambda)
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
				throw new NotImplementedException();
			}

			public override ISqlExpression[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.All   :
					case ConvertFlags.Key   :
					case ConvertFlags.Field : return Sequence.ConvertToSql(expression, level + 1, flags);
				}

				throw new NotImplementedException();
			}

			public override int[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.Root : return expression == Lambda.Parameters[0];
				}

				return false;
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				throw new NotImplementedException();
			}
		}
	}
}
