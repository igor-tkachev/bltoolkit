using System;
using System.Collections.Generic;
using System.Linq;
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
			/*
			MethodType methodType;

			switch (methodCall.Method.Name)
			{
				case "Average" : methodType = MethodType.Average; break;
				case "Min"     : methodType = MethodType.Min;     break;
				case "Max"     : methodType = MethodType.Max;     break;
				case "Sum"     : methodType = MethodType.Sum;     break;
				default        : return null;
			}
			*/

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

			var lambda = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var func   = new ParsingContext(sequence, lambda);

			func.SqlQuery.Select.Add(
				new SqlFunction(
					methodCall.Type,
					methodCall.Method.Name,
					parser.ParseExpression(func, func.Lambda.Body.Unwrap())));

			return new BuildingContext(sequence, lambda);
		}

		enum MethodType
		{
			Average,
			Min,
			Max,
			Sum,
		}

		class ParsingContext : SequenceContextBase
		{
			public ParsingContext(IParseContext sequence, LambdaExpression lambda)
				: base(sequence, lambda)
			{
			}

			public override Expression BuildQuery()
			{
				throw new NotImplementedException();
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

				throw new NotImplementedException();
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				throw new NotImplementedException();
			}
		}

		class BuildingContext : SequenceContextBase
		{
			public BuildingContext(IParseContext sequence, LambdaExpression lambda)
				: base(sequence, lambda)
			{
			}

			public override Expression BuildQuery()
			{
				throw new NotImplementedException();
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				throw new NotImplementedException();
			}

			public override ISqlExpression[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.Field :
					case ConvertFlags.All   :
						break;
				}

				throw new NotImplementedException();
			}

			public override int[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.Field :
					case ConvertFlags.All   :
						break;
				}

				throw new NotImplementedException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor testFlag)
			{
				throw new NotImplementedException();
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				throw new NotImplementedException();
			}
		}
	}
}
