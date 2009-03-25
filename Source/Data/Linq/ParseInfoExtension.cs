using System;
using System.Linq;
using System.Linq.Expressions;

using FTest = System.Func<BLToolkit.Data.Linq.ParseInfo<System.Linq.Expressions.Expression>, bool>;

namespace BLToolkit.Data.Linq
{
	static class ParseInfoExtension
	{
		public static UnaryExpression ConvertTo<T>(this Expression expr)
		{
			return Expression.Convert(expr, typeof(T));
		}

		static Func<Expression> GetArgument(ParseInfo<MethodCallExpression> pi, int i)
		{
			return () => pi.Indexer(ParseInfo.MethodCall.Arguments, ParseInfo.ExprItem, i);
		}

		public static bool IsMethod(
			this ParseInfo<MethodCallExpression>  pi,
			Type                                  declaringType,
			string                                methodName,
			FTest[]                               args,
			Func<ParseInfo<MethodCallExpression>,bool> func)
		{
			var method = pi.Expr;

			if (declaringType == method.Method.DeclaringType && method.Method.Name == methodName && method.Arguments.Count == args.Length)
			{
				for (int i = 0; i < args.Length; i++)
					if (!args[i](ParseInfo.Create(method.Arguments[i], GetArgument(pi, i))))
						return false;

				return func(pi);
			}

			return false;
		}

		[Obsolete]
		public static bool IsMethod(this ParseInfo<MethodCallExpression> pi, Type declaringType, string methodName, params FTest[] args)
		{
			return IsMethod(pi, declaringType, methodName, args, p => true);
		}

		[Obsolete]
		public static bool IsQueryableMethod(this ParseInfo<MethodCallExpression> pi, string methodName, params FTest[] args)
		{
			return IsMethod(pi, typeof(Queryable), methodName, args, p => true);
		}

		public static bool IsQueryableMethod(
			this ParseInfo<MethodCallExpression> pi,
			string methodName,
			Action<ParseInfo<Expression>>        seq,
			Action<ParseInfo<ParameterExpression>,ParseInfo<Expression>> lambda)
		{
			return IsMethod(pi, typeof(Queryable), methodName, new FTest[] { p => { seq(p); return true; }, l => l.IsLambda(lambda) }, p => true);
		}
	}
}
