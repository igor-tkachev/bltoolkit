using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	class WhereParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return methodCall.IsQueryable("Where");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var sequence  = parser.ParseSequence(methodCall.Arguments[0], sqlQuery);
			var condition = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var result    = parser.ParseWhere(sequence, condition);

			result.SetAlias(condition.Parameters[0].Name);

			return result;
		}

		public class WhereContext : SequenceContextBase
		{
			public WhereContext(IParseContext sequence, LambdaExpression lambda)
				: base(sequence, lambda)
			{
			}

			public override Expression BuildQuery()
			{
				throw new InvalidOperationException();
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				throw new InvalidOperationException();
			}

			public override ISqlExpression[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				if (level == 0)
				{
					switch (flags)
					{
						case ConvertFlags.Field :
						case ConvertFlags.All   :
							if (expression.GetRootObject() == Lambda.Parameters[0])
								return Sequence.ConvertToSql(expression, level + 1, flags);
							break;
					}

					throw new NotImplementedException();
				}

				throw new InvalidOperationException();

				//return Array<ISqlExpression>.Empty;
			}

			public override int[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new InvalidOperationException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.ScalarExpression : return false;
					case RequestFor.Root             : return expression == Lambda.Parameters[0];
					case RequestFor.Field            : return Sequence.IsExpression(expression, level + 1, requestFlag);
				}

				throw new NotImplementedException();
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				throw new NotImplementedException();
			}
		}
	}
}
