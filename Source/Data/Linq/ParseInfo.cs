using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

using FExpr = System.Func<System.Linq.Expressions.Expression>;
using FParm = System.Func<BLToolkit.Data.Linq.ParseInfo<System.Linq.Expressions.ParameterExpression>, bool>;
using FTest = System.Func<BLToolkit.Data.Linq.ParseInfo<System.Linq.Expressions.Expression>, bool>;

namespace BLToolkit.Data.Linq
{
	abstract class ParseInfo
	{
		public Expression Expr;
		public FExpr      ParamAccessor;

		public ExpressionType NodeType { get { return Expr.NodeType; } }

		public static ParseInfo<T> Create<T>(T expr, FExpr func)
			where T : Expression
		{
			return new ParseInfo<T> { Expr = expr, ParamAccessor = func };
		}

		#region Match

		public bool Match(params Func<ParseInfo,bool>[] matches)
		{
			foreach (var match in matches)
				if (match(this))
					return true;
			return false;
		}

		#endregion

		#region IsLambda

		public bool IsLambda<T>(FParm[] parameters, FTest body, Func<ParseInfo<LambdaExpression>,bool> func)
			where T : Expression
		{
			var pi = this;

			if (pi.NodeType == ExpressionType.Quote)
			{
				pi = Create((Expr as UnaryExpression).Operand as T, () => Property<UnaryExpression>(Unary.Operand));
			}

			if (pi.NodeType == ExpressionType.Lambda)
			{
				var lambda = pi.ConvertTo<LambdaExpression>();

				if (lambda.Expr.Parameters.Count == parameters.Length)
					for (int i = 0; i < parameters.Length; i++)
						if (!parameters[i](Create(lambda.Expr.Parameters[i], () => lambda.Indexer(Lambda.Parameters, ParamItem, i))))
							return false;

				return body(Create(lambda.Expr.Body, () => lambda.Property(Lambda.Body))) && func(lambda);
			}
			
			return false;
		}

		public bool IsLambda<T>(FTest body, Func<ParseInfo<LambdaExpression>,bool> func)
		{
			return IsLambda<Expression>(new FParm[] { e => e.Expr.Type == typeof(T) }, body, func);
		}

		static FParm[] _singleParam = new FParm[] { p => true };

		[Obsolete]
		public bool IsLambda(int parameters, Func<ParseInfo<LambdaExpression>,bool> func)
		{
			var parms = parameters != 1? new FParm[parameters]: _singleParam;

			if (parameters != 1)
				for (int i = 0; i < parms.Length; i++)
					parms[i] = _singleParam[0];

			return IsLambda<Expression>(parms, p => true, func);
		}

		public bool IsLambda(Action<ParseInfo<ParameterExpression>,ParseInfo<Expression>> lambda)
		{
			ParseInfo<ParameterExpression> parameter = null;

			return IsLambda<Expression>(
				new FParm[] { p => { parameter = p; return true; } },
				body => { lambda(parameter, body); return true; },
				p => true);
		}

		#endregion

		#region IsUnary

		[Obsolete]
		static bool IsUnary(ParseInfo<Expression> pi, ExpressionType nodeType, FTest func)
		{
			return
				pi.NodeType == nodeType?
					func(Create(((UnaryExpression)pi.Expr).Operand, () => pi.Property<UnaryExpression>(Unary.Operand))):
					false;
		}

		#endregion

		#region IsConstant

		public bool IsConstant(Func<ParseInfo<ConstantExpression>,bool> func)
		{
			if (NodeType == ExpressionType.Constant)
			{
				var c = Expr as ConstantExpression;
				return func(Create(c, () => Property<ConstantExpression>(Constant.Value)));
			}

			return false;
		}

		public bool IsConstant<T>(Func<T,FExpr,bool> func)
		{
			if (NodeType == ExpressionType.Constant)
			{
				var c = Expr as ConstantExpression;
				return c.Value is T? func((T)c.Value, () => Property<ConstantExpression>(Constant.Value).ConvertTo<T>()): false;
			}

			return false;
		}

		public bool IsConstant<T>()
		{
			return IsConstant<T>((p1, p2) => true);
		}

		#endregion

		#region IsMemberAccess

		public bool IsMemberAccess(FTest test, Func<ParseInfo<MemberExpression>,bool> func)
		{
			if (NodeType == ExpressionType.MemberAccess)
			{
				var ex = Expr as MemberExpression;
				return
					test(Create(ex.Expression, () => Property(Member.Expression))) &&
					func(ConvertTo<MemberExpression>());
			}

			return false;
		}

		public bool IsMemberAccess(Func<ParseInfo<MemberExpression>,ParseInfo<Expression>,bool> func)
		{
			if (NodeType == ExpressionType.MemberAccess)
			{
				var ex = Expr as MemberExpression;
				return func(
					ConvertTo<MemberExpression>(),
					Create(ex.Expression, () => Property(Member.Expression)));
			}

			return false;
		}

		#endregion

		#region IsParameter

		public bool IsParameter()
		{
			return NodeType == ExpressionType.Parameter;
		}

		#endregion

		#region IsMethod

		public bool IsMethod(Type declaringType, string methodName, FTest[] args, Func<ParseInfo<MethodCallExpression>,bool> func)
		{
			return
				NodeType == ExpressionType.Call?
					ParseInfoExtension.IsMethod(ConvertTo<MethodCallExpression>(), declaringType, methodName, args, func):
					false;
		}

		public bool IsMethod(Type declaringType, string methodName, params FTest[] args)
		{
			return IsMethod(declaringType, methodName, args, p => true);
		}

		#endregion

		#region Helpers

		public ParseInfo<P> ConvertTo<P>()
			where P : Expression
		{
			return Create(Expr as P, () => ConvertExpressionTo<P>());
		}

		#endregion

		#region Walk

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator == (ParseInfo pi, Expression expr)
		{
			return pi.Expr == expr;
		}

		public static bool operator != (ParseInfo pi, Expression expr)
		{
			return pi.Expr != expr;
		}

		public static implicit operator Expression(ParseInfo pi)
		{
			return pi.Expr;
		}

		ParseInfo<Expression> Convert<P>()
		{
			return Create(Expr as Expression, () => ConvertExpressionTo<P>());
		}

		ParseInfo Walk(Expression e, MethodInfo property, Func<ParseInfo,ParseInfo> func)
		{
			return Create(e, () => Property(property)).Walk(func);
		}

		IEnumerable<E> Walk<E>(IEnumerable<E> source, Func<E,int,E> func)
			where E : class
		{
			var modified = false;
			var list     = new List<E>();
			var i        = 0;

			foreach (var item in source)
			{
				var e = func(item, i++);
				list.Add(e);
				modified = modified || e != item;
			}

			return modified? list: source;
		}

		IEnumerable<E> Walk<E>(IEnumerable<E> source, MethodInfo property, Func<E,ParseInfo,E> func)
			where E : class
		{
			return Walk(source, (e,i) => func(e, Create(Expr, () => Indexer(property, IndexExpressor<E>.Item, i))));
		}

		IEnumerable<E> Walk<E>(IEnumerable<E> source, MethodInfo property, Func<ParseInfo,ParseInfo> func)
			where E : Expression
		{
			return Walk(source, (e,i) => Create((Expression)e, () => Indexer(property, IndexExpressor<E>.Item, i)).Walk(func).Expr as E);
		}

		public ParseInfo Walk(Func<ParseInfo,ParseInfo> func)
		{
			if (Expr == null)
				return this;

			switch (Expr.NodeType)
			{
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				case ExpressionType.ArrayIndex:
				case ExpressionType.Coalesce:
				case ExpressionType.Divide:
				case ExpressionType.Equal:
				case ExpressionType.ExclusiveOr:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LeftShift:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.NotEqual:
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				case ExpressionType.Power:
				case ExpressionType.RightShift:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
					{
						var pi = Convert<BinaryExpression>();
						var e  = Expr as BinaryExpression;
						var c  = pi.Walk(e.Conversion, Binary.Conversion, func);
						var l  = pi.Walk(e.Left,       Binary.Left,       func);
						var r  = pi.Walk(e.Right,      Binary.Right,      func);

						if (c != e.Conversion || l != e.Left || r != e.Right)
							pi.Expr = Expression.MakeBinary(Expr.NodeType, l, r, e.IsLiftedToNull, e.Method, (LambdaExpression)c);

						return func(pi);
					}

				case ExpressionType.ArrayLength:
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
				case ExpressionType.Not:
				case ExpressionType.Quote:
				case ExpressionType.TypeAs:
				case ExpressionType.UnaryPlus:
					{
						var pi = Convert<UnaryExpression>();
						var e  = Expr as UnaryExpression;
						var o  = pi.Walk(e.Operand, Unary.Operand, func);

						if (o != e.Operand)
							pi.Expr = Expression.MakeUnary(Expr.NodeType, o.Expr, e.Type, e.Method);

						return func(pi);
					}

				case ExpressionType.Call:
					{
						var pi = Convert<MethodCallExpression>();
						var e  = Expr as MethodCallExpression;
						var o  = pi.Walk(e.Object,    MethodCall.Object,    func);
						var a  = pi.Walk(e.Arguments, MethodCall.Arguments, func);

						if (o != e.Object || a != e.Arguments)
							pi.Expr = Expression.Call(o.Expr, e.Method, a);

						return func(pi);
					}

				case ExpressionType.Conditional:
					{
						var pi = Convert<ConditionalExpression>();
						var e  = Expr as ConditionalExpression;
						var s  = pi.Walk(e.Test,    Conditional.Test,    func);
						var t  = pi.Walk(e.IfTrue,  Conditional.IfTrue,  func);
						var f  = pi.Walk(e.IfFalse, Conditional.IfFalse, func);

						if (s != e.Test || t != e.IfTrue || f != e.IfFalse)
							pi.Expr = Expression.Condition(s.Expr, t.Expr, f.Expr);

						return func(pi);
					}

				case ExpressionType.Invoke:
					{
						var pi = Convert<InvocationExpression>();
						var e  = Expr as InvocationExpression;
						var ex = pi.Walk(e.Expression, Invocation.Expression, func);
						var a  = pi.Walk(e.Arguments,  Invocation.Arguments,  func);

						if (ex != e.Expression || a != e.Arguments)
							pi.Expr = Expression.Invoke(ex, a);

						return func(pi);
					}

				case ExpressionType.Lambda:
					{
						var pi = Convert<LambdaExpression>();
						var e  = Expr as LambdaExpression;
						var b  = pi.Walk(e.Body,       Lambda.Body,       func);
						var p  = pi.Walk(e.Parameters, Lambda.Parameters, func);

						if (b != e.Body || p != e.Parameters)
							pi.Expr = Expression.Lambda(b, p.ToArray());

						return func(pi);
					}

				case ExpressionType.ListInit:
					{
						var pi = Convert<ListInitExpression>();
						var e  = Expr as ListInitExpression;
						var n  = pi.Walk(e.NewExpression, ListInit.NewExpression, func);
						var i  = pi.Walk(e.Initializers,  ListInit.Initializers, (p,pinf) =>
						{
							var args = pinf.Walk(p.Arguments, ElementInit.Arguments, func);
							return args != p.Arguments? Expression.ElementInit(p.AddMethod, args): p;
						});

						if (n != e.NewExpression || i != e.Initializers)
							pi.Expr = Expression.ListInit((NewExpression)n, i);

						return func(pi);
					}

				case ExpressionType.MemberAccess:
					{
						var pi = Convert<MemberExpression>();
						var e  = Expr as MemberExpression;
						var ex = pi.Walk(e.Expression, Member.Expression, func);

						if (ex != e.Expression)
							pi.Expr = Expression.MakeMemberAccess(ex, e.Member);

						return func(pi);
					}

				case ExpressionType.MemberInit:
					{
						Func<MemberBinding,ParseInfo,MemberBinding> modify = null; modify = (b,pinf) =>
						{
							switch (b.BindingType)
							{
								case MemberBindingType.Assignment:
									{
										var ma = (MemberAssignment)b;
										var ex = pinf.Convert<MemberAssignment>().Walk(ma.Expression, MemberAssignmentBind.Expression, func);

										if (ex != ma.Expression)
											ma = Expression.Bind(ma.Member, ex);

										return ma;
									}

								case MemberBindingType.ListBinding:
									{
										var ml = (MemberListBinding)b;
										var i  = pinf.Convert<MemberListBinding>().Walk(ml.Initializers, MemberListBind.Initializers, (p,psi) =>
										{
											var args = psi.Walk(p.Arguments, ElementInit.Arguments, func);
											return args != p.Arguments? Expression.ElementInit(p.AddMethod, args): p;
										});

										if (i != ml.Initializers)
											ml = Expression.ListBind(ml.Member, i);

										return ml;
									}

								case MemberBindingType.MemberBinding:
									{
										var mm = (MemberMemberBinding)b;
										var bs = pinf.Convert<MemberMemberBinding>().Walk(mm.Bindings, MemberMemberBind.Bindings, modify);

										if (bs != mm.Bindings)
											mm = Expression.MemberBind(mm.Member);

										return mm;
									}
							}

							return (MemberBinding)b;
						};

						var pi = Convert<MemberInitExpression>();
						var e  = Expr as MemberInitExpression;
						var ne = pi.Walk(e.NewExpression, MemberInit.NewExpression, func);
						var bb = pi.Walk(e.Bindings,      MemberInit.Bindings,      modify);

						if (ne != e.NewExpression || bb != e.Bindings)
							pi.Expr = Expression.MemberInit((NewExpression)ne, bb);

						return func(pi);
					}

				case ExpressionType.New:
					{
						var pi = Convert<NewExpression>();
						var e  = Expr as NewExpression;
						var a  = Walk(e.Arguments, New.Arguments, func);

						if (a != e.Arguments)
							pi.Expr = e.Members == null?
								Expression.New(e.Constructor, a):
								Expression.New(e.Constructor, a, e.Members);

						return func(pi);
					}

				case ExpressionType.NewArrayBounds:
					{
						var pi = Convert<NewArrayExpression>();
						var e  = Expr as NewArrayExpression;
						var ex = Walk(e.Expressions, NewArray.Expressions, func);

						if (ex != e.Expressions)
							pi.Expr = Expression.NewArrayBounds(e.Type, ex);

						return func(pi);
					}

				case ExpressionType.NewArrayInit:
					{
						var pi = Convert<NewArrayExpression>();
						var e  = Expr as NewArrayExpression;
						var ex = Walk(e.Expressions, NewArray.Expressions, func);

						if (ex != e.Expressions)
							pi.Expr = Expression.NewArrayInit(e.Type, ex);

						return func(pi);
					}

				case ExpressionType.TypeIs:
					{
						var pi = Convert<TypeBinaryExpression>();
						var e  = Expr as TypeBinaryExpression;
						var ex = Walk(e.Expression, TypeBinary.Expression, func);

						if (ex != e.Expression)
							pi.Expr = Expression.TypeIs(ex, e.Type);

						return func(pi);
					}

				case ExpressionType.Constant : return func(Convert<ConstantExpression> ());
				case ExpressionType.Parameter: return func(Convert<ParameterExpression>());
			}

			throw new InvalidOperationException();
		}

		#endregion

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
			public static MethodInfo PropertyExpressor(Expression<Func<T,object>> func)
			{
				return ((PropertyInfo)((MemberExpression)func.Body).Member).GetGetMethod();
			}

			public static MethodInfo MethodExpressor(Expression<Func<T,object>> func)
			{
				var ex = func.Body;

				if (ex is UnaryExpression)
					ex = ((UnaryExpression)func.Body).Operand;

				return ((MethodCallExpression)ex).Method;
			}
		}

		public class Binary : Expressor<BinaryExpression>
		{
			public static MethodInfo Conversion = PropertyExpressor(e => e.Conversion);
			public static MethodInfo Left       = PropertyExpressor(e => e.Left);
			public static MethodInfo Right      = PropertyExpressor(e => e.Right);
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
			public static MethodInfo Object    = PropertyExpressor(e => e.Object);
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
			public static MethodInfo Bindings      = PropertyExpressor(e => e.Bindings);
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

		public class MemberAssignmentBind : Expressor<MemberAssignment>
		{
			public static MethodInfo Expression = PropertyExpressor(e => e.Expression);
		}

		public class MemberListBind : Expressor<MemberListBinding>
		{
			public static MethodInfo Initializers = PropertyExpressor(e => e.Initializers);
		}

		public class MemberMemberBind : Expressor<MemberMemberBinding>
		{
			public static MethodInfo Bindings = PropertyExpressor(e => e.Bindings);
		}

		public static MethodInfo ExprItem  = IndexExpressor<Expression>         .Item;
		public static MethodInfo ParamItem = IndexExpressor<ParameterExpression>.Item;
		public static MethodInfo ElemItem  = IndexExpressor<ElementInit>        .Item;

		public class DataReader : Expressor<IDataReader>
		{
			public static MethodInfo GetValue = MethodExpressor(rd => rd.GetValue(0));
		}

		public class MappingSchema : Expressor<BLToolkit.Mapping.MappingSchema>
		{
			public static Dictionary<Type,MethodInfo> Converters = new Dictionary<Type,MethodInfo>
			{
				// Primitive Types
				//
				{ typeof(SByte),           MethodExpressor(m => m.ConvertToSByte                 (null)) },
				{ typeof(Int16),           MethodExpressor(m => m.ConvertToInt16                 (null)) },
				{ typeof(Int32),           MethodExpressor(m => m.ConvertToInt32                 (null)) },
				{ typeof(Int64),           MethodExpressor(m => m.ConvertToInt64                 (null)) },
				{ typeof(Byte),            MethodExpressor(m => m.ConvertToByte                  (null)) },
				{ typeof(UInt16),          MethodExpressor(m => m.ConvertToUInt16                (null)) },
				{ typeof(UInt32),          MethodExpressor(m => m.ConvertToUInt32                (null)) },
				{ typeof(UInt64),          MethodExpressor(m => m.ConvertToUInt64                (null)) },
				{ typeof(Char),            MethodExpressor(m => m.ConvertToChar                  (null)) },
				{ typeof(Single),          MethodExpressor(m => m.ConvertToSingle                (null)) },
				{ typeof(Double),          MethodExpressor(m => m.ConvertToDouble                (null)) },
				{ typeof(Boolean),         MethodExpressor(m => m.ConvertToBoolean               (null)) },

				// Simple Types
				//
				{ typeof(String),          MethodExpressor(m => m.ConvertToString                (null)) },
				{ typeof(DateTime),        MethodExpressor(m => m.ConvertToDateTime              (null)) },
				{ typeof(DateTimeOffset),  MethodExpressor(m => m.ConvertToDateTimeOffset        (null)) },
				{ typeof(Decimal),         MethodExpressor(m => m.ConvertToDecimal               (null)) },
				{ typeof(Guid),            MethodExpressor(m => m.ConvertToGuid                  (null)) },
				{ typeof(Stream),          MethodExpressor(m => m.ConvertToStream                (null)) },
				{ typeof(XmlReader),       MethodExpressor(m => m.ConvertToXmlReader             (null)) },
				{ typeof(XmlDocument),     MethodExpressor(m => m.ConvertToXmlDocument           (null)) },
				{ typeof(Byte[]),          MethodExpressor(m => m.ConvertToByteArray             (null)) },
				{ typeof(Char[]),          MethodExpressor(m => m.ConvertToCharArray             (null)) },

				// Nullable Types
				//
				{ typeof(SByte?),          MethodExpressor(m => m.ConvertToNullableSByte         (null)) },
				{ typeof(Int16?),          MethodExpressor(m => m.ConvertToNullableInt16         (null)) },
				{ typeof(Int32?),          MethodExpressor(m => m.ConvertToNullableInt32         (null)) },
				{ typeof(Int64?),          MethodExpressor(m => m.ConvertToNullableInt64         (null)) },
				{ typeof(Byte?),           MethodExpressor(m => m.ConvertToNullableByte          (null)) },
				{ typeof(UInt16?),         MethodExpressor(m => m.ConvertToNullableUInt16        (null)) },
				{ typeof(UInt32?),         MethodExpressor(m => m.ConvertToNullableUInt32        (null)) },
				{ typeof(UInt64?),         MethodExpressor(m => m.ConvertToNullableUInt64        (null)) },
				{ typeof(Char?),           MethodExpressor(m => m.ConvertToNullableChar          (null)) },
				{ typeof(Double?),         MethodExpressor(m => m.ConvertToNullableDouble        (null)) },
				{ typeof(Single?),         MethodExpressor(m => m.ConvertToNullableSingle        (null)) },
				{ typeof(Boolean?),        MethodExpressor(m => m.ConvertToNullableBoolean       (null)) },
				{ typeof(DateTime?),       MethodExpressor(m => m.ConvertToNullableDateTime      (null)) },
				{ typeof(DateTimeOffset?), MethodExpressor(m => m.ConvertToNullableDateTimeOffset(null)) },
				{ typeof(Decimal?),        MethodExpressor(m => m.ConvertToNullableDecimal       (null)) },
				{ typeof(Guid?),           MethodExpressor(m => m.ConvertToNullableGuid          (null)) },

				// SqlTypes
				//
				{ typeof(SqlByte),         MethodExpressor(m => m.ConvertToSqlByte               (null)) },
				{ typeof(SqlInt16),        MethodExpressor(m => m.ConvertToSqlInt16              (null)) },
				{ typeof(SqlInt32),        MethodExpressor(m => m.ConvertToSqlInt32              (null)) },
				{ typeof(SqlInt64),        MethodExpressor(m => m.ConvertToSqlInt64              (null)) },
				{ typeof(SqlSingle),       MethodExpressor(m => m.ConvertToSqlSingle             (null)) },
				{ typeof(SqlBoolean),      MethodExpressor(m => m.ConvertToSqlBoolean            (null)) },
				{ typeof(SqlDouble),       MethodExpressor(m => m.ConvertToSqlDouble             (null)) },
				{ typeof(SqlDateTime),     MethodExpressor(m => m.ConvertToSqlDateTime           (null)) },
				{ typeof(SqlDecimal),      MethodExpressor(m => m.ConvertToSqlDecimal            (null)) },
				{ typeof(SqlMoney),        MethodExpressor(m => m.ConvertToSqlMoney              (null)) },
				{ typeof(SqlString),       MethodExpressor(m => m.ConvertToSqlString             (null)) },
				{ typeof(SqlBinary),       MethodExpressor(m => m.ConvertToSqlBinary             (null)) },
				{ typeof(SqlGuid),         MethodExpressor(m => m.ConvertToSqlGuid               (null)) },
				{ typeof(SqlBytes),        MethodExpressor(m => m.ConvertToSqlBytes              (null)) },
				{ typeof(SqlChars),        MethodExpressor(m => m.ConvertToSqlChars              (null)) },
				{ typeof(SqlXml),          MethodExpressor(m => m.ConvertToSqlXml                (null)) },
			};
		}

		#endregion
	}
}
