using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using FExpr = System.Func<System.Linq.Expressions.Expression>;
using FParm = System.Func<BLToolkit.Data.Linq.ParseInfo<System.Linq.Expressions.ParameterExpression>, bool>;
using FTest = System.Func<BLToolkit.Data.Linq.ParseInfo<System.Linq.Expressions.Expression>, bool>;

namespace BLToolkit.Data.Linq
{
	class ParseInfo<T> : ParseInfo
		where T : Expression
	{
		public ParseInfo (T expr, Func<Expression> func)
		{
			Expr          = expr;
			ParamAccessor = func;
		}

		public T Expr;

		public ExpressionType NodeType { get { return Expr.NodeType; } }

		#region Match

		public bool Match(params Func<ParseInfo<T>,bool>[] matches)
		{
			foreach (var match in matches)
				if (match(this))
					return true;
			return false;
		}

		#region IsLambda

		public bool IsLambda(FParm[] parameters, FTest body, Func<ParseInfo<LambdaExpression>,bool> func)
		{
			var pi = this;

			if (pi.NodeType == ExpressionType.Quote)
			{
				pi = Create((pi.Expr as UnaryExpression).Operand as T, () => pi.Property<UnaryExpression>(Unary.Operand));
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

		public bool IsLambda<P>(FTest body, Func<ParseInfo<LambdaExpression>,bool> func)
		{
			return IsLambda(new FParm[] { e => e.Expr.Type == typeof(P) }, body, func);
		}

		static FParm[] _singleParam = new FParm[] { p => true };

		[Obsolete]
		public bool IsLambda(int parameters, Func<ParseInfo<LambdaExpression>,bool> func)
		{
			var parms = parameters != 1? new FParm[parameters]: _singleParam;

			if (parameters != 1)
				for (int i = 0; i < parms.Length; i++)
					parms[i] = _singleParam[0];

			return IsLambda(parms, p => true, func);
		}

		public bool IsLambda(Action<ParseInfo<ParameterExpression>,ParseInfo<Expression>> lambda)
		{
			ParseInfo<ParameterExpression> parameter = null;

			return IsLambda(
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

		[Obsolete]
		public bool IsConstant(Func<ParseInfo<ConstantExpression>,bool> func)
		{
			return
				NodeType == ExpressionType.Constant?
					func(Create(Expr as ConstantExpression, () => ConvertExpressionTo<ConstantExpression>())):
					false;
		}

		public bool IsConstant<CT>(Func<CT,FExpr,bool> func)
		{
			if (NodeType == ExpressionType.Constant)
			{
				var c = Expr as ConstantExpression;
				return c.Value is CT? func((CT)c.Value, () => Property<ConstantExpression>(Constant.Value).ConvertTo<CT>()): false;
			}

			return false;
		}

		public bool IsConstant<CT>()
		{
			return IsConstant<CT>((p1, p2) => true);
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

		#endregion

		#region Helpers

		public ParseInfo<P> ConvertTo<P>()
			where P : Expression
		{
			return Create(Expr as P, () => Expression.Convert(ParamAccessor(), typeof(P)));
		}

		#endregion

		#region Walk

		ParseInfo<Expression> Convert<P>()
		{
			return Create(Expr as Expression, () => ConvertExpressionTo<P>());
		}

		bool Walk(Expression e, MethodInfo property, Func<ParseInfo<Expression>, bool> func)
		{
			return Create(e, () => Property(property)).Walk(func);
		}

		bool Walk<P>(ReadOnlyCollection<P> col, MethodInfo property, Func<ParseInfo<Expression>, bool> func)
		{
			for (var i = 0; i < col.Count; i++)
				if (!Create(col[i] as Expression, () => Indexer(property, IndexExpressor<P>.Item, i)).Walk(func))
					return false;
			return true;
		}

		public bool Walk(Func<ParseInfo<Expression>,bool> func)
		{
			if (Expr == null)
				return true;

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
						var e  = Expr as BinaryExpression;
						var pi = Convert<BinaryExpression>();
						return
							func(pi) &&
							pi.Walk(e.Left,  Binary.Left,  func) &&
							pi.Walk(e.Right, Binary.Right, func);
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
						var e  = Expr as UnaryExpression;
						var pi = Convert<UnaryExpression>();
						return
							func(pi) &&
							pi.Walk(e.Operand, Unary.Operand, func);
					}

				case ExpressionType.Call:
					{
						var e  = Expr as MethodCallExpression;
						var pi = Convert<MethodCallExpression>();
						return
							func(pi) &&
							pi.Walk(e.Arguments, MethodCall.Arguments, func);
					}

				case ExpressionType.Conditional:
					{
						var e  = Expr as ConditionalExpression;
						var pi = Convert<ConditionalExpression>();
						return
							func(pi) &&
							pi.Walk(e.Test,    Conditional.Test,    func) &&
							pi.Walk(e.IfTrue,  Conditional.IfTrue,  func) &&
							pi.Walk(e.IfFalse, Conditional.IfFalse, func);
					}

				case ExpressionType.Constant:
					{
						var e  = Expr as ConstantExpression;
						var pi = Convert<ConstantExpression>();
						return
							func(pi);
					}

				case ExpressionType.Invoke:
					{
						var e  = Expr as InvocationExpression;
						var pi = Convert<InvocationExpression>();
						return
							func(pi) &&
							pi.Walk(e.Expression, Invocation.Expression, func) &&
							pi.Walk(e.Arguments,  Invocation.Arguments,  func);
					}

				case ExpressionType.Lambda:
					{
						var e  = Expr as LambdaExpression;
						var pi = Convert<LambdaExpression>();
						return
							func(pi) &&
							pi.Walk(e.Body,       Lambda.Body,       func) &&
							pi.Walk(e.Parameters, Lambda.Parameters, func);
					}

				case ExpressionType.ListInit:
					{
						var e  = Expr as ListInitExpression;
						var pi = Convert<ListInitExpression>();

						if (!func(pi) || !pi.Walk(e.NewExpression, ListInit.NewExpression, func))
							return false;

						for (var i = 0; i < e.Initializers.Count; i++)
						{
							var elem = Create(e, () => pi.Indexer(ListInit.Initializers, ElemItem, i));

							for (var j = 0; j < elem.Expr.Initializers[i].Arguments.Count; j++)
								if (!Create(elem.Expr.Initializers[i].Arguments[j], () => elem.Indexer(ElementInit.Arguments, ExprItem, j)).Walk(func))
									return false;
						}

						return true;
					}

				case ExpressionType.MemberAccess:
					{
						var e  = Expr as MemberExpression;
						var pi = Convert<MemberExpression>();
						return
							func(pi) &&
							pi.Walk(e.Expression, Member.Expression, func);
					}

				case ExpressionType.MemberInit:
					{
						var e  = Expr as MemberInitExpression;
						var pi = Convert<MemberInitExpression>();

						if (!func(pi) || !pi.Walk(e.NewExpression, MemberInit.NewExpression, func))
							return false;

						Func<MemberBinding,bool> walkBinding = null; walkBinding = b =>
						{
							switch (b.BindingType)
							{
								//case MemberBindingType.Assignment:
								//	return Walk(((MemberAssignment)b1).Expression, ((MemberAssignment)b2).Expression);

								case MemberBindingType.ListBinding:
									var ml = (MemberListBinding)b;

									for (int i = 0; i < ml.Initializers.Count; i++)
									{
										var ei1 = ml.Initializers[i];

										//for (int j = 0; j < ei1.Arguments.Count; j++)
										//	if (!Compare(ei1.Arguments[j], ei2.Arguments[j]))
										//		return false;
									}

									break;

								case MemberBindingType.MemberBinding:
									var mm1 = (MemberMemberBinding)b;

									for (int i = 0; i < mm1.Bindings.Count; i++)
										if (!walkBinding(mm1.Bindings[i]))
											return false;

									break;
							}

							return true;
						};

						for (var i = 0; i < e.Bindings.Count; i++)
						{
							var b = e.Bindings[i];

							if (!walkBinding(b))
								return false;
						}

						return true;
					}

				case ExpressionType.New:
					{
						var e  = Expr as NewExpression;
						var pi = Convert<NewExpression>();
						return
							func(pi) &&
							pi.Walk(e.Arguments, New.Arguments, func);
					}

				case ExpressionType.NewArrayBounds:
				case ExpressionType.NewArrayInit:
					{
						var e  = Expr as NewArrayExpression;
						var pi = Convert<NewArrayExpression>();
						return
							func(pi) &&
							pi.Walk(e.Expressions, NewArray.Expressions, func);
					}

				case ExpressionType.Parameter:
					{
						var e  = Expr as ParameterExpression;
						var pi = Convert<ParameterExpression>();
						return
							func(pi);
					}

				case ExpressionType.TypeIs:
					{
						var e  = Expr as TypeBinaryExpression;
						var pi = Convert<TypeBinaryExpression>();
						return
							func(pi) &&
							pi.Walk(e.Expression, TypeBinary.Expression, func);
					}
			}

			throw new InvalidOperationException();
		}

		static bool IsConstant(Type type)
		{
			return type == typeof(int) || type == typeof(string);
		}

		#endregion
	}
}
