using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	abstract class MethodCallParser : ISequenceParser
	{
		public int ParsingCounter { get; set; }

		public bool CanParse(ExpressionParser parser, Expression expression, SqlQuery sqlQuery)
		{
			if (expression.NodeType == ExpressionType.Call)
				return CanParseMethodCall(parser, (MethodCallExpression)expression, sqlQuery);
			return false;
		}

		public IParseContext ParseSequence(ExpressionParser parser, IParseContext parent, Expression expression, SqlQuery sqlQuery)
		{
			if (expression.NodeType == ExpressionType.Call)
				return ParseMethodCall(parser, parent, (MethodCallExpression)expression, sqlQuery);
			return null;
		}

		protected abstract bool          CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery);
		protected abstract IParseContext ParseMethodCall   (ExpressionParser parser, IParseContext parent, MethodCallExpression methodCall, SqlQuery sqlQuery);
	}
}
