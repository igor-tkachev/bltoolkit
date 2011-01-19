using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Parser
{
	abstract class MethodCallParser : ISequenceParser
	{
		public int ParsingCounter { get; set; }

		public virtual IParseInfo ParseSequence(IParseInfo parseInfo, Expression expression)
		{
			if (expression.NodeType == ExpressionType.Call)
				return ParseMethodCall(parseInfo, (MethodCallExpression)expression);

			return null;
		}

		protected abstract IParseInfo ParseMethodCall(IParseInfo parseInfo, MethodCallExpression expression);

		protected bool IsQueryable(string name, MethodInfo methodInfo)
		{
			if (methodInfo.Name != name)
				return false;

			var type = methodInfo.DeclaringType;

			return type == typeof(Queryable) || type == typeof(Enumerable) || type == typeof(LinqExtensions);
		}
	}
}
