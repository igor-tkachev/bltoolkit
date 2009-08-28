using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FTest = System.Func<BLToolkit.Data.Linq.ParseInfo<System.Linq.Expressions.Expression>, bool>;

namespace BLToolkit.Data.Linq
{
	static class ParseInfoExtension
	{
		public static UnaryExpression ConvertTo<T>(this Expression expr)
		{
			return Expression.Convert(expr, typeof(T));
		}

		public static bool IsMethod(
			this ParseInfo<MethodCallExpression> pi,
			Type                                 declaringType,
			string                               methodName,
			FTest[]                              args,
			Func<ParseInfo<MethodCallExpression>,bool> func)
		{
			var method = pi.Expr;

			if (declaringType == method.Method.DeclaringType && method.Method.Name == methodName && method.Arguments.Count == args.Length)
			{
				for (var i = 0; i < args.Length; i++)
					if (!args[i](pi.CreateArgument(i)))
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
			string                               methodName,
			Action<ParseInfo<Expression>>        seq,
			Action<LambdaInfo>                   lambda)
		{
			return IsMethod(pi, typeof(Queryable), methodName, new FTest[] { p => { seq(p); return true; }, l => l.IsLambda(1, lambda) }, p => true);
		}

		public static bool IsQueryableMethod(
			this ParseInfo<MethodCallExpression> pi,
			string                               methodName,
			int                                  nparams1,
			int                                  nparams2,
			Action<ParseInfo<Expression>>        seq,
			Action<LambdaInfo,LambdaInfo>        parms)
		{
			LambdaInfo lambda1 = null;
			LambdaInfo lambda2 = null;

			if (IsMethod(pi, typeof(Queryable), methodName, new FTest[]
				{
					p => { seq(p); return true; },
					l => l.IsLambda(nparams1, l1 => lambda1 = l1),
					l => l.IsLambda(nparams2, l2 => lambda2 = l2),
				}, p => true))
			{
				parms(lambda1, lambda2);
				return true;
			}

			return false;
		}

		public static bool IsQueryableMethod(
			this ParseInfo<MethodCallExpression>     pi,
			string                                   methodName,
			int                                      nparams1,
			int                                      nparams2,
			int                                      nparams3,
			Action<ParseInfo<Expression>>            seq,
			Action<LambdaInfo,LambdaInfo,LambdaInfo> parms)
		{
			LambdaInfo lambda1 = null;
			LambdaInfo lambda2 = null;
			LambdaInfo lambda3 = null;

			if (IsMethod(pi, typeof(Queryable), methodName, new FTest[]
				{
					p => { seq(p); return true; },
					l => l.IsLambda(nparams1, l1 => lambda1 = l1),
					l => l.IsLambda(nparams2, l2 => lambda2 = l2),
					l => l.IsLambda(nparams3, l3 => lambda3 = l3),
				}, p => true))
			{
				parms(lambda1, lambda2, lambda3);
				return true;
			}

			return false;
		}

		public static bool IsQueryableMethod(
			this ParseInfo<MethodCallExpression> pi,
			string                               methodName,
			Action<ParseInfo<Expression>>        seq,
			Action<ParseInfo<Expression>,LambdaInfo,LambdaInfo,LambdaInfo> parms)
		{
			ParseInfo<Expression> inner   = null;
			LambdaInfo            lambda1 = null;
			LambdaInfo            lambda2 = null;
			LambdaInfo            lambda3 = null;

			if (IsMethod(pi, typeof(Queryable), methodName, new FTest[]
				{
					p => { seq(p);    return true; },
					p => { inner = p; return true; },
					l => l.IsLambda(1, l1 => lambda1 = l1),
					l => l.IsLambda(1, l2 => lambda2 = l2),
					l => l.IsLambda(2, l3 => lambda3 = l3),
				}, p => true))
			{
				parms(inner, lambda1, lambda2, lambda3);
				return true;
			}

			return false;
		}

		public static bool IsEnumerableMethod(
			this ParseInfo<MethodCallExpression> pi,
			string                               methodName,
			Func<ParseInfo<Expression>,bool>     action)
		{
			return IsMethod(pi, typeof(Enumerable), methodName, new [] { action }, p => true);
		}

		public static ParseInfo<Expression> CreateArgument(this ParseInfo<MethodCallExpression> pi, int idx)
		{
			return pi.Create(pi.Expr.Arguments[idx], pi.Indexer(ReflectionHelper.MethodCall.Arguments, ReflectionHelper.ExprItem, idx));
		}

		[Obsolete]
		static bool IsMethod(
			this ParseInfo<MethodCallExpression> pi,
			MethodInfo                           method,
			Action<ParseInfo<Expression>>        seq,
			Action<ParseInfo<ParameterExpression>,ParseInfo<Expression>> lambda)
		{
			if (pi.Expr.Method == method)
			{
				seq(pi.CreateArgument(0));
				pi.CreateArgument(1).IsLambda(lambda);

				return true;
			}

			return false;
		}
	}
}
