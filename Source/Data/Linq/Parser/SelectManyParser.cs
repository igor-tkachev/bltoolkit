using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class SelectManyParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return
				methodCall.IsQueryable("SelectMany") &&
				((LambdaExpression)methodCall.Arguments[1].Unwrap()).Parameters.Count == 1;
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var sequence           = parser.ParseSequence(methodCall.Arguments[0], sqlQuery);
			var collectionSelector = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var resultSelector     = methodCall.Arguments.Count == 3 ? (LambdaExpression)methodCall.Arguments[2].Unwrap() : null;

			//if (collectionSelector.Parameters[0] == collectionSelector.Body.Unwrap())
			//{
			//	return resultSelector == null ? sequence : new SelectContext(resultSelector, sequence, sequence);
			//}

			var context = new SelectManyContext(sequence, collectionSelector);

			parser.ParentContext.Insert(0, context);

			var collection = parser.ParseSequence(collectionSelector.Body.Unwrap(), context.SqlQuery);

			parser.ParentContext.Remove(context);

			return resultSelector == null ? collection : new SelectContext(resultSelector, sequence, collection);
		}

		class SelectManyContext : PathThroughContext
		{
			public SelectManyContext(IParseContext sequence, LambdaExpression lambda)
				: base(sequence, lambda)
			{
			}
		}
	}
}
