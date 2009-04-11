using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FExpr = System.Func<System.Linq.Expressions.Expression>;
using FParm = System.Func<BLToolkit.Data.Linq.ParseInfo<System.Linq.Expressions.ParameterExpression>, bool>;
using FTest = System.Func<BLToolkit.Data.Linq.ParseInfo<System.Linq.Expressions.Expression>, bool>;

namespace BLToolkit.Data.Linq
{
	abstract class ParseInfo : ReflectionHelper
	{
		public Expression     Expr;
		public ParseInfo      Parent;
		public Expression     ParamAccessor;
		public bool           IsReplaced;

		public ExpressionType NodeType { get { return Expr.NodeType; } }

		#region Create

		public static ParseInfo<T> CreateRoot<T>(T expr, Expression paramAccesor)
			where T : Expression
		{
			return new ParseInfo<T> { Expr = expr, Parent = null, ParamAccessor = paramAccesor };
		}

		public ParseInfo<T> Create<T>(T expr, Expression paramAccesor)
			where T : Expression
		{
			return new ParseInfo<T> { Expr = expr, Parent = this, ParamAccessor = paramAccesor };
		}

		public ParseInfo<T> Replace<T>(T expr, Expression paramAccesor)
			where T : Expression
		{
			return new ParseInfo<T> { Expr = expr, Parent = this, ParamAccessor = paramAccesor, IsReplaced = true };
		}

		#endregion

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
				pi = Create(((UnaryExpression)Expr).Operand as T, Property<UnaryExpression>(Unary.Operand));
			}

			if (pi.NodeType == ExpressionType.Lambda)
			{
				var lambda = pi.ConvertTo<LambdaExpression>();

				if (lambda.Expr.Parameters.Count == parameters.Length)
					for (int i = 0; i < parameters.Length; i++)
						if (!parameters[i](lambda.Create(lambda.Expr.Parameters[i], lambda.Indexer(Lambda.Parameters, ParamItem, i))))
							return false;

				return body(lambda.Create(lambda.Expr.Body, lambda.Property(Lambda.Body))) && func(lambda);
			}
			
			return false;
		}

		public bool IsLambda<T>(FTest body, Func<ParseInfo<LambdaExpression>,bool> func)
		{
			return IsLambda<Expression>(new FParm[] { e => e.Expr.Type == typeof(T) }, body, func);
		}

		static readonly FParm[] _singleParam = new FParm[] { p => true };

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
					func(pi.Create(((UnaryExpression)pi.Expr).Operand, pi.Property<UnaryExpression>(Unary.Operand))):
					false;
		}

		#endregion

		#region IsConstant

		public bool IsConstant(Func<ParseInfo<ConstantExpression>,bool> func)
		{
			if (NodeType == ExpressionType.Constant)
			{
				var c = Expr as ConstantExpression;
				return func(Parent.Create(c, Property<ConstantExpression>(Constant.Value)));
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
					test(Create(ex.Expression, Property(Member.Expression))) &&
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
					Create(ex.Expression, Property(Member.Expression)));
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

		public bool IsMethod(Func<ParseInfo<MethodCallExpression>,bool> func)
		{
			return NodeType == ExpressionType.Call? func(ConvertTo<MethodCallExpression>()): false;
		}

		#endregion

		#region Helpers

		public ParseInfo<T> ConvertTo<T>()
			where T : Expression
		{
			return Parent == null?
				CreateRoot   (Expr as T, ConvertExpressionTo<T>()):
				Parent.Create(Expr as T, ConvertExpressionTo<T>());
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
			return (object)pi == null && expr == null || pi.Expr == expr;
		}

		public static bool operator != (ParseInfo pi, Expression expr)
		{
			return pi.Expr != expr;
		}

		public static implicit operator Expression(ParseInfo pi)
		{
			return pi.Expr;
		}

		public ParseInfo<Expression> Convert<T>()
		{
			var pi = Parent.Create(Expr, ConvertExpressionTo<T>());
			return pi;
		}

		ParseInfo Walk(Expression e, MethodInfo property, Func<ParseInfo,ParseInfo> func)
		{
			return Create(e, Property(property)).Walk(func);
		}

		static IEnumerable<T> Walk<T>(IEnumerable<T> source, Func<T,int,T> func)
			where T : class
		{
			var modified = false;
			var list     = new List<T>();
			var i        = 0;

			foreach (var item in source)
			{
				var e = func(item, i++);
				list.Add(e);
				modified = modified || e != item;
			}

			return modified? list: source;
		}

		IEnumerable<T> Walk<T>(IEnumerable<T> source, MethodInfo property, Func<T,ParseInfo,T> func)
			where T : class
		{
			return Walk(source, (e,i) => func(e, Create(Expr, Indexer(property, IndexExpressor<T>.Item, i))));
		}

		IEnumerable<T> Walk<T>(IEnumerable<T> source, MethodInfo property, Func<ParseInfo,ParseInfo> func)
			where T : Expression
		{
			return Walk(source, (e,i) => Create((Expression)e, Indexer(property, IndexExpressor<T>.Item, i)).Walk(func).Expr as T);
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
						var pi = func(Convert<BinaryExpression>());
						if (pi.IsReplaced)
							return pi;

						var e = Expr as BinaryExpression;
						var c = pi.Walk(e.Conversion, Binary.Conversion, func);
						var l = pi.Walk(e.Left,       Binary.Left,       func);
						var r = pi.Walk(e.Right,      Binary.Right,      func);

						if (c != e.Conversion || l != e.Left || r != e.Right)
							pi.Expr = Expression.MakeBinary(Expr.NodeType, l, r, e.IsLiftedToNull, e.Method, (LambdaExpression) c);

						return pi;
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
						var pi = func(Convert<UnaryExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as UnaryExpression;
						var o  = pi.Walk(e.Operand, Unary.Operand, func);

						if (o != e.Operand)
							pi.Expr = Expression.MakeUnary(Expr.NodeType, o.Expr, e.Type, e.Method);

						return pi;
					}

				case ExpressionType.Call:
					{
						var pi = func(Convert<MethodCallExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as MethodCallExpression;
						var o  = pi.Walk(e.Object,    MethodCall.Object,    func);
						var a  = pi.Walk(e.Arguments, MethodCall.Arguments, func);

						if (o != e.Object || a != e.Arguments)
							pi.Expr = Expression.Call(o.Expr, e.Method, a);

						return pi;
					}

				case ExpressionType.Conditional:
					{
						var pi = func(Convert<ConditionalExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as ConditionalExpression;
						var s  = pi.Walk(e.Test,    Conditional.Test,    func);
						var t  = pi.Walk(e.IfTrue,  Conditional.IfTrue,  func);
						var f  = pi.Walk(e.IfFalse, Conditional.IfFalse, func);

						if (s != e.Test || t != e.IfTrue || f != e.IfFalse)
							pi.Expr = Expression.Condition(s.Expr, t.Expr, f.Expr);

						return pi;
					}

				case ExpressionType.Invoke:
					{
						var pi = func(Convert<InvocationExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as InvocationExpression;
						var ex = pi.Walk(e.Expression, Invocation.Expression, func);
						var a  = pi.Walk(e.Arguments,  Invocation.Arguments,  func);

						if (ex != e.Expression || a != e.Arguments)
							pi.Expr = Expression.Invoke(ex, a);

						return pi;
					}

				case ExpressionType.Lambda:
					{
						var pi = func(Convert<LambdaExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as LambdaExpression;
						var b  = pi.Walk(e.Body,       Lambda.Body,       func);
						var p  = pi.Walk(e.Parameters, Lambda.Parameters, func);

						if (b != e.Body || p != e.Parameters)
							pi.Expr = Expression.Lambda(b, p.ToArray());

						return pi;
					}

				case ExpressionType.ListInit:
					{
						var pi = func(Convert<ListInitExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as ListInitExpression;
						var n  = pi.Walk(e.NewExpression, ListInit.NewExpression, func);
						var i  = pi.Walk(e.Initializers,  ListInit.Initializers, (p,pinf) =>
						{
							var args = pinf.Walk(p.Arguments, ElementInit.Arguments, func);
							return args != p.Arguments? Expression.ElementInit(p.AddMethod, args): p;
						});

						if (n != e.NewExpression || i != e.Initializers)
							pi.Expr = Expression.ListInit((NewExpression)n, i);

						return pi;
					}

				case ExpressionType.MemberAccess:
					{
						var pi = func(Convert<MemberExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as MemberExpression;
						var ex = pi.Walk(e.Expression, Member.Expression, func);

						if (ex != e.Expression)
							pi.Expr = Expression.MakeMemberAccess(ex, e.Member);

						return pi;
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

							return b;
						};

						var pi = func(Convert<MemberInitExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as MemberInitExpression;
						var ne = pi.Walk(e.NewExpression, MemberInit.NewExpression, func);
						var bb = pi.Walk(e.Bindings,      MemberInit.Bindings,      modify);

						if (ne != e.NewExpression || bb != e.Bindings)
							pi.Expr = Expression.MemberInit((NewExpression)ne, bb);

						return pi;
					}

				case ExpressionType.New:
					{
						var pi = func(Convert<NewExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as NewExpression;
						var a  = pi.Walk(e.Arguments, New.Arguments, func);

						if (a != e.Arguments)
							pi.Expr = e.Members == null?
								Expression.New(e.Constructor, a):
								Expression.New(e.Constructor, a, e.Members);

						return pi;
					}

				case ExpressionType.NewArrayBounds:
					{
						var pi = func(Convert<NewArrayExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as NewArrayExpression;
						var ex = pi.Walk(e.Expressions, NewArray.Expressions, func);

						if (ex != e.Expressions)
							pi.Expr = Expression.NewArrayBounds(e.Type, ex);

						return pi;
					}

				case ExpressionType.NewArrayInit:
					{
						var pi = func(Convert<NewArrayExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as NewArrayExpression;
						var ex = pi.Walk(e.Expressions, NewArray.Expressions, func);

						if (ex != e.Expressions)
							pi.Expr = Expression.NewArrayInit(e.Type, ex);

						return pi;
					}

				case ExpressionType.TypeIs:
					{
						var pi = func(Convert<TypeBinaryExpression>());
						if (pi.IsReplaced)
							return pi;

						var e  = Expr as TypeBinaryExpression;
						var ex = pi.Walk(e.Expression, TypeBinary.Expression, func);

						if (ex != e.Expression)
							pi.Expr = Expression.TypeIs(ex, e.Type);

						return pi;
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
			return Expression.Convert(ParamAccessor, typeof(T));
		}

		public MemberExpression Property(MethodInfo mi)
		{
			return Expression.Property(ParamAccessor, mi);
		}

		public MemberExpression Property<T>(MethodInfo mi)
		{
			return Expression.Property(ConvertExpressionTo<T>(), mi);
		}

		public MethodCallExpression Indexer(MethodInfo pmi, MethodInfo mi, int idx)
		{
			return Expression.Call(Property(pmi), mi, new Expression[] { Expression.Constant(idx, typeof(int)) });
		}

		public MethodCallExpression Index<T>(IEnumerable<T> source, MethodInfo property, int idx)
		{
			return Indexer(property, IndexExpressor<T>.Item, idx);
		}

		#endregion
	}
}
