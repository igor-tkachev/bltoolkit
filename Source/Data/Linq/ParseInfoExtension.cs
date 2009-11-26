using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	using FTest = Func<ParseInfo<Expression>, bool>;

	static class ParseInfoExtension
	{
		public static UnaryExpression ConvertTo<T>(this Expression expr)
		{
			return Expression.Convert(expr, typeof(T));
		}

		//[DebuggerStepThrough]
		public static bool IsMethod(
			this ParseInfo<MethodCallExpression> pi,
			Type                                 declaringType,
			string                               methodName,
			FTest[]                              args,
			Func<ParseInfo<MethodCallExpression>,bool> func)
		{
			var method = pi.Expr;
			var dtype  = method.Method.DeclaringType;

			if ((declaringType == null && (dtype == typeof(Queryable) || dtype == typeof(Enumerable)) || declaringType == dtype) &&
				(methodName == null || method.Method.Name == methodName) &&
				method.Arguments.Count == args.Length)
			{
				for (var i = 0; i < args.Length; i++)
					if (!args[i](pi.CreateArgument(i)))
						return false;

				return func(pi);
			}

			return false;
		}

		[DebuggerStepThrough]
		public static bool IsQueryableMethod(
			this ParseInfo<MethodCallExpression>        pi,
			Func<ParseInfo<Expression>,LambdaInfo,bool> lambda)
		{
			ParseInfo<Expression> seq = null;
			return IsMethod(pi, null, null, new FTest[]
			{
				p => { seq = p; return true; },
				l => l.CheckIfLambda(1, p => lambda(seq, p))
			}, p => true);
		}

		[DebuggerStepThrough]
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

			if (IsMethod(pi, null, methodName, new FTest[]
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

		[DebuggerStepThrough]
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

			if (IsMethod(pi, null, methodName, new FTest[]
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

		[DebuggerStepThrough]
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

			if (IsMethod(pi, null, methodName, new FTest[]
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

		//[DebuggerStepThrough]
		public static bool IsEnumerableMethod(
			this ParseInfo<MethodCallExpression> pi,
			string                               methodName,
			Func<ParseInfo<Expression>,bool>     action)
		{
			return IsMethod(pi, typeof(Enumerable), methodName, new [] { action }, p => true);
		}

		//[DebuggerStepThrough]
		public static ParseInfo<Expression> CreateArgument(this ParseInfo<MethodCallExpression> pi, int idx)
		{
			return pi.Create(pi.Expr.Arguments[idx], pi.Indexer(ReflectionHelper.MethodCall.Arguments, ReflectionHelper.ExprItem, idx));
		}

		[DebuggerStepThrough]
		public static bool IsQueryableMethod(
			this ParseInfo<MethodCallExpression> pi,
			string                               methodName,
			Action<ParseInfo<Expression>>        seq,
			Func<ParseInfo<Expression>,bool>     action)
		{
			return IsMethod(pi, null, methodName, new [] { p => { seq(p); return true; }, action }, _ => true);
		}

		[DebuggerStepThrough]
		public static bool IsQueryableMethod(
			this ParseInfo<MethodCallExpression> pi,
			Func<ParseInfo<Expression>,bool>     func)
		{
			return IsMethod(pi, null, null, new [] { func }, _ => true);
		}
	}
}
