using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	abstract class MethodCallParser : ISequenceParser
	{
		public int ParsingCounter { get; set; }

		public bool CanParse(ExpressionParser parser, ParseInfo parseInfo)
		{
			if (parseInfo.Expression.NodeType == ExpressionType.Call)
				return CanParseMethodCall(parser, (MethodCallExpression)parseInfo.Expression, parseInfo);
			return false;
		}

		public IParseContext ParseSequence(ExpressionParser parser, ParseInfo parseInfo)
		{
			return ParseMethodCall(parser, (MethodCallExpression)parseInfo.Expression, parseInfo);
		}

		protected abstract bool          CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo);
		protected abstract IParseContext ParseMethodCall   (ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo);
	}
}
