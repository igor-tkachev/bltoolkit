using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using FExpr = System.Func<System.Linq.Expressions.Expression>;

namespace BLToolkit.Data.Linq
{
	class ParseInfo
	{
		public Func<Expression> ParamAccessor;

		public static ParseInfo<T> Create<T>(T expr, FExpr func)
			where T : Expression
		{
			return new ParseInfo<T>(expr, func);
		}

		#region Parameter accessor helpers

		public UnaryExpression ConvertTo<P>()
		{
			return Expression.Convert(ParamAccessor(), typeof(P));
		}

		public MemberExpression Property(MethodInfo mi)
		{
			return Expression.Property(ParamAccessor(), mi);
		}

		public MemberExpression Property<P>(MethodInfo mi)
		{
			return Expression.Property(ConvertTo<P>(), mi);
		}

		public MethodCallExpression Indexer(MethodInfo pmi, MethodInfo mi, int idx)
		{
			return Expression.Call(Property(pmi), mi, new Expression[] { Expression.Constant(idx, typeof(int)) });
		}

		#endregion

		#region Reflection Helpers

		public static Expression Expressor<P>(Expression<Func<P,object>> func)
		{
			return func.Body;
		}

		public static MethodInfo PropertyExpressor<P>(Expression<Func<P, object>> func)
		{
			return ((PropertyInfo)((MemberExpression)func.Body).Member).GetGetMethod();
		}

		public static MethodInfo IndexerExpressor<P>(Expression<Func<ReadOnlyCollection<P>, object>> func)
		{
			return ((MethodCallExpression)func.Body).Method;
		}

		public static MethodInfo _miOperand    = PropertyExpressor<UnaryExpression>     (e => e.Operand);
		public static MethodInfo _miBody       = PropertyExpressor<LambdaExpression>    (e => e.Body);
		public static MethodInfo _miValue      = PropertyExpressor<ConstantExpression>  (e => e.Value);
		public static MethodInfo _miParameters = PropertyExpressor<MethodCallExpression>(e => e.Arguments);
		public static MethodInfo _miArguments  = PropertyExpressor<LambdaExpression>    (e => e.Parameters);

		public static MethodInfo _miParamItem  = IndexerExpressor<ParameterExpression>  (c  => c[0]);
		public static MethodInfo _miExprItem   = IndexerExpressor<Expression>           (c  => c[0]);

		#endregion
	}
}
