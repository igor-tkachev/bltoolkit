using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;

	class InsertParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			return methodCall.IsQueryable("Insert", "InsertWithIdentity");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			var sequence = parser.ParseSequence(new ParseInfo(parseInfo, methodCall.Arguments[0]));

			return new InsertContext(parseInfo.Parent, sequence, null);
		}
	}

	class InsertContext : SequenceContextBase
	{
		public InsertContext(IParseContext parent, IParseContext sequence, LambdaExpression lambda)
			: base(parent, sequence, lambda)
		{
		}

		public override Expression BuildExpression(Expression expression, int level)
		{
			throw new NotImplementedException();
		}

		public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
		{
			throw new NotImplementedException();
		}

		public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
		{
			throw new NotImplementedException();
		}

		public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
		{
			throw new NotImplementedException();
		}

		public override IParseContext GetContext(Expression expression, int level, ParseInfo parseInfo)
		{
			throw new NotImplementedException();
		}
	}
}
