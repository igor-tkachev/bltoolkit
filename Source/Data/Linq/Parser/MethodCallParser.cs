using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Parser
{
	abstract class MethodCallParser : ISequenceParser
	{
		public int ParsingCounter { get; set; }

		public virtual ParseInfo ParseSequence(ExpressionParser parser, Expression expression)
		{
			if (expression.NodeType == ExpressionType.Call)
				return ParseMethodCall(parser, (MethodCallExpression)expression);

			return null;
		}

		protected abstract ParseInfo ParseMethodCall(ExpressionParser parser, MethodCallExpression expression);

		protected bool IsQueryable(string name, MethodInfo methodInfo)
		{
			if (methodInfo.Name != name)
				return false;

			var type = methodInfo.DeclaringType;

			return type == typeof(Queryable) || type == typeof(Enumerable) || type == typeof(LinqExtensions);
		}
	}
}
