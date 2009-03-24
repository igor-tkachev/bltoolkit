using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	class ParseInfo<T> : ParseInfo
		where T : Expression
	{
		public new T Expr { get { return (T)base.Expr; } set { base.Expr = value; } }

		public bool Match(params Func<ParseInfo<T>,bool>[] matches)
		{
			foreach (var match in matches)
				if (match(this))
					return true;
			return false;
		}

		#region Walk

		/*
		[Obsolete]
		ParseInfo<Expression> Convert<P>()
		{
			return Create(Expr as Expression, () => ConvertExpressionTo<P>());
		}

		[Obsolete]
		bool Walk1(Expression e, MethodInfo property, Func<ParseInfo<Expression>, bool> func)
		{
			return Create(e, () => Property(property)).Walk1(func);
		}

		[Obsolete]
		bool Walk1<P>(ReadOnlyCollection<P> col, MethodInfo property, Func<ParseInfo<Expression>, bool> func)
		{
			for (var i = 0; i < col.Count; i++)
				if (!Create(col[i] as Expression, () => Indexer(property, IndexExpressor<P>.Item, i)).Walk1(func))
					return false;
			return true;
		}

		[Obsolete]
		public bool Walk1(Func<ParseInfo<Expression>,bool> func)
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
							pi.Walk1(e.Conversion, Binary.Conversion, func) &&
							pi.Walk1(e.Left,       Binary.Left,       func) &&
							pi.Walk1(e.Right,      Binary.Right,      func);
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
							pi.Walk1(e.Operand, Unary.Operand, func);
					}

				case ExpressionType.Call:
					{
						var e  = Expr as MethodCallExpression;
						var pi = Convert<MethodCallExpression>();
						return
							func(pi) &&
							pi.Walk1(e.Arguments, MethodCall.Arguments, func);
					}

				case ExpressionType.Conditional:
					{
						var e  = Expr as ConditionalExpression;
						var pi = Convert<ConditionalExpression>();
						return
							func(pi) &&
							pi.Walk1(e.Test,    Conditional.Test,    func) &&
							pi.Walk1(e.IfTrue,  Conditional.IfTrue,  func) &&
							pi.Walk1(e.IfFalse, Conditional.IfFalse, func);
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
							pi.Walk1(e.Expression, Invocation.Expression, func) &&
							pi.Walk1(e.Arguments,  Invocation.Arguments,  func);
					}

				case ExpressionType.Lambda:
					{
						var e  = Expr as LambdaExpression;
						var pi = Convert<LambdaExpression>();
						return
							func(pi) &&
							pi.Walk1(e.Body,       Lambda.Body,       func) &&
							pi.Walk1(e.Parameters, Lambda.Parameters, func);
					}

				case ExpressionType.ListInit:
					{
						var e  = Expr as ListInitExpression;
						var pi = Convert<ListInitExpression>();

						if (!func(pi) || !pi.Walk1(e.NewExpression, ListInit.NewExpression, func))
							return false;

						for (var i = 0; i < e.Initializers.Count; i++)
						{
							var elem = Create(e, () => pi.Indexer(ListInit.Initializers, ElemItem, i));

							for (var j = 0; j < elem.Expr.Initializers[i].Arguments.Count; j++)
								if (!Create(elem.Expr.Initializers[i].Arguments[j], () => elem.Indexer(ElementInit.Arguments, ExprItem, j)).Walk1(func))
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
							pi.Walk1(e.Expression, Member.Expression, func);
					}

				case ExpressionType.MemberInit:
					{
						var e  = Expr as MemberInitExpression;
						var pi = Convert<MemberInitExpression>();

						if (!func(pi) || !pi.Walk1(e.NewExpression, MemberInit.NewExpression, func))
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
							pi.Walk1(e.Arguments, New.Arguments, func);
					}

				case ExpressionType.NewArrayBounds:
				case ExpressionType.NewArrayInit:
					{
						var e  = Expr as NewArrayExpression;
						var pi = Convert<NewArrayExpression>();
						return
							func(pi) &&
							pi.Walk1(e.Expressions, NewArray.Expressions, func);
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
							pi.Walk1(e.Expression, TypeBinary.Expression, func);
					}
			}

			throw new InvalidOperationException();
		}

		static bool IsConstant(Type type)
		{
			return type == typeof(int) || type == typeof(string);
		}
		 */

		#endregion
	}
}
