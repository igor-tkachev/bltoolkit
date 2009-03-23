using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using FExpr = System.Func<System.Linq.Expressions.Expression>;

namespace BLToolkit.Data.Linq
{
	class ParseInfo
	{
		public FExpr ParamAccessor;

		public static ParseInfo<T> Create<T>(T expr, FExpr func)
			where T : Expression
		{
			return new ParseInfo<T>(expr, func);
		}

		#region Parameter accessor helpers

		public UnaryExpression ConvertExpressionTo<T>()
		{
			return Expression.Convert(ParamAccessor(), typeof(T));
		}

		public MemberExpression Property(MethodInfo mi)
		{
			return Expression.Property(ParamAccessor(), mi);
		}

		public MemberExpression Property<T>(MethodInfo mi)
		{
			return Expression.Property(ConvertExpressionTo<T>(), mi);
		}

		public MethodCallExpression Indexer(MethodInfo pmi, MethodInfo mi, int idx)
		{
			return Expression.Call(Property(pmi), mi, new Expression[] { Expression.Constant(idx, typeof(int)) });
		}

		#endregion

		#region Reflection Helpers

		public class Expressor<T>
		{
			public static MethodInfo PropertyExpressor(Expression<Func<T, object>> func)
			{
				return ((PropertyInfo)((MemberExpression)func.Body).Member).GetGetMethod();
			}
		}

		public class Binary : Expressor<BinaryExpression>
		{
			public static MethodInfo Left  = PropertyExpressor(e => e.Left);
			public static MethodInfo Right = PropertyExpressor(e => e.Right);
		}

		public class Unary : Expressor<UnaryExpression>
		{
			public static MethodInfo Operand = PropertyExpressor(e => e.Operand);
		}

		public class Lambda : Expressor<LambdaExpression>
		{
			public static MethodInfo Body       = PropertyExpressor(e => e.Body);
			public static MethodInfo Parameters = PropertyExpressor(e => e.Parameters);
		}

		public class Constant : Expressor<ConstantExpression>
		{
			public static MethodInfo Value = PropertyExpressor(e => e.Value);
		}

		public class MethodCall : Expressor<MethodCallExpression>
		{
			public static MethodInfo Arguments = PropertyExpressor(e => e.Arguments);
		}

		public class Conditional : Expressor<ConditionalExpression>
		{
			public static MethodInfo Test    = PropertyExpressor(e => e.Test);
			public static MethodInfo IfTrue  = PropertyExpressor(e => e.IfTrue);
			public static MethodInfo IfFalse = PropertyExpressor(e => e.IfFalse);
		}

		public class Invocation : Expressor<InvocationExpression>
		{
			public static MethodInfo Expression = PropertyExpressor(e => e.Expression);
			public static MethodInfo Arguments  = PropertyExpressor(e => e.Arguments);
		}

		public class ListInit : Expressor<ListInitExpression>
		{
			public static MethodInfo NewExpression = PropertyExpressor(e => e.NewExpression);
			public static MethodInfo Initializers  = PropertyExpressor(e => e.Initializers);
		}

		public class ElementInit : Expressor<System.Linq.Expressions.ElementInit>
		{
			public static MethodInfo Arguments = PropertyExpressor(e => e.Arguments);
		}

		public class Member : Expressor<MemberExpression>
		{
			public static MethodInfo Expression = PropertyExpressor(e => e.Expression);
		}

		public class MemberInit : Expressor<MemberInitExpression>
		{
			public static MethodInfo NewExpression = PropertyExpressor(e => e.NewExpression);
		}

		public class New : Expressor<NewExpression>
		{
			public static MethodInfo Arguments = PropertyExpressor(e => e.Arguments);
		}

		public class NewArray : Expressor<NewArrayExpression>
		{
			public static MethodInfo Expressions = PropertyExpressor(e => e.Expressions);
		}

		public class TypeBinary : Expressor<TypeBinaryExpression>
		{
			public static MethodInfo Expression = PropertyExpressor(e => e.Expression);
		}

		public class IndexExpressor<T>
		{
			public static MethodInfo IndexerExpressor(Expression<Func<ReadOnlyCollection<T>, object>> func)
			{
				return ((MethodCallExpression)((UnaryExpression)func.Body).Operand).Method;
			}

			public static MethodInfo Item = IndexerExpressor(c => c[0]);
		}

		public static MethodInfo ExprItem  = IndexExpressor<Expression>         .Item;
		public static MethodInfo ParamItem = IndexExpressor<ParameterExpression>.Item;
		public static MethodInfo ElemItem  = IndexExpressor<ElementInit>        .Item;

		#endregion
	}
}
